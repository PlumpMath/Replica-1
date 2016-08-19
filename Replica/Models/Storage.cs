namespace Replica.Models {

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Security.Cryptography;
	using Newtonsoft.Json;

	using Replica.Interfaces;



	public sealed class Storage : IStorage {

		public string Identifier => Instruction?.Identifier;
		public Instruction Instruction { get; private set; }

		public event CancelEventHandler LoadAllFilesProgress;

		private bool _isDisposed;

		private MD5 _hashAlgorithm;
		private readonly IDebugger _debugger;

		private Dictionary<string, List<IMetaFile>> _metaFiles;



		public Storage(IDebugger debugger) {

			(_debugger = debugger).Trace();

		}



		public void Initialize(Instruction instruction) {

			_debugger.Trace(instruction.Identifier);

			Instruction = instruction;
			_hashAlgorithm = MD5.Create();
			_metaFiles = new Dictionary<string, List<IMetaFile>>();

		}



		public void SaveChanges() {

			// ...
			_debugger.Trace();

			// ...
			foreach (var list in _metaFiles) {

				// ...
				var changed = false;
				var identifier = string.Empty;

				// ...
				foreach (var item in list.Value) {
					if (item.IsChanged) {
						changed = true;
						identifier = item.Identifier;
						break;
					}
				}

				// ...
				if (!changed) continue;

				// ...
				var path = $"{Instruction.StorageDirectory}{Path.DirectorySeparatorChar}{identifier}.meta";
				
				// ...
				var jsonContent = JsonConvert.SerializeObject(list);
				File.WriteAllText(path, jsonContent);

				for (var n = 0; n < list.Value.Count; n++) {
					list.Value[n].IsChanged = false;
				}

			}

		}



		public void CloseMetaFile(string identifier, bool saveChanges = true) {
			
			// ...
			_debugger.Trace();

			// Nothing to do, if there is no item with specified identifier
			if (!_metaFiles.ContainsKey(identifier)) return;

			// If required to save changes
			if (saveChanges) {

				// Computing path to meta-file, where we will store List<IMetaFile>
				var path = $"{Instruction.StorageDirectory}{Path.DirectorySeparatorChar}{identifier}.meta";

				// Serializing list and writing data into json-file
				var jsonContent = JsonConvert.SerializeObject(_metaFiles[identifier]);
				File.WriteAllText(path, jsonContent);

			}

			// Removing list of items with specified identifier from collection
			_metaFiles.Remove(identifier);

		}



		public void CloseMetaFiles() {

			// ...
			_debugger.Trace();

			// Disposing each element in each list in order to release event-handlers,
			// and everything, that might cancel GC to collect IMetaFile.
			foreach (var list in _metaFiles) {
				foreach (var item in list.Value) item.Dispose();
			}

			// ...
			_metaFiles.Clear();

		}



		public void LoadMetaFiles() {
			
			// ...
			_debugger.Trace();

			// Directory of storage where meta files are
			var storageDirectory = $"{Instruction.StorageDirectory}{Path.DirectorySeparatorChar}";

			// Loading each file into collection
			foreach (var file in Directory.GetFiles(storageDirectory, "*.meta")) {

				// Deserializing data
				var jsonContent = File.ReadAllText(file);
				var list = JsonConvert.DeserializeObject<List<MetaFile>>(jsonContent);

				// Do nothing if list is empty or already loaded
				if (list.Count == 0) continue;
				var identifier = list[0].Identifier;
				if (_metaFiles.ContainsKey(identifier)) continue;

				// Converting to interface list
				var metaFiles = new List<IMetaFile>();
				foreach (var row in list) metaFiles.Add(row);

				// Adding to collection
				_metaFiles.Add(identifier, metaFiles);

				// Checking if operation is cancelled
				var cancelEventArgs = new CancelEventArgs();
				LoadAllFilesProgress?.Invoke(this, cancelEventArgs);
				if (cancelEventArgs.Cancel) return;

			}

		}



		public IMetaFile GetMetaFile(string path) {

			// ...
			_debugger.Trace(path);

			// Computing relative path, identifier and path to meta-file
			var relativePath = ComputeRelativePath(path);
			var identifier = ComputeIdentifier(relativePath);
			var metaFilePath = $"{Instruction.StorageDirectory}{Path.DirectorySeparatorChar}{identifier}.meta";

			// Loading meta-file's content, if exists and not yet loaded into collection
			if (File.Exists(metaFilePath) && !_metaFiles.ContainsKey(identifier)) {
				LoadMetaFile(relativePath);
			}

			// If identifier exists in collection, attempting to search by relative path
			if (_metaFiles.ContainsKey(identifier)) {
				foreach (var metaFile in _metaFiles[identifier]) {
					if (metaFile.RelativePath == relativePath) return metaFile;
				}
			}

			// ...
			return null;

		}



		public IMetaFile CreateMetaFile(string path) {

			// ...
			_debugger.Trace(path);

			// Computing relative path, identifier and path to meta-file
			var relativePath = ComputeRelativePath(path);
			var identifier = ComputeIdentifier(relativePath);
			var metaFilePath = $"{Instruction.StorageDirectory}{Path.DirectorySeparatorChar}{identifier}.meta";

			// Loading meta-file's content, if exists and not yet loaded into collection
			if (File.Exists(metaFilePath) && !_metaFiles.ContainsKey(identifier)) {
				LoadMetaFile(relativePath);
			}

			// If identifier exists in collection, attempting to search by relative path
			if (_metaFiles.ContainsKey(identifier)) {
				foreach (var row in _metaFiles[identifier]) {
					if (row.RelativePath == relativePath)
						throw new Exception($"{nameof(IMetaFile)} with specified path '{path}' already exists in database.");
				}

			} else {

				// Adding new key-value row into collection
				_metaFiles.Add(identifier, new List<IMetaFile>());
				
			}

			// Creating new instance of MetaFile, placing into collection
			var metaFile = new MetaFile(identifier, relativePath);
			_metaFiles[identifier].Add(metaFile);
			

			// ...
			return metaFile;

		}



		private void LoadMetaFile(string metaFilePath, bool overwriteCollection = false) {

			// ...
			_debugger.Trace($"metaFilePath: '{metaFilePath}'; overwriteCollection: {overwriteCollection}");

			// Reading and converting
			var jsonContent = File.ReadAllText(metaFilePath);
			var strictList = JsonConvert.DeserializeObject<List<MetaFile>>(jsonContent);
			if (strictList.Count == 0) return;

			// ...
			var identifier = strictList[0].Identifier;
			if (_metaFiles.ContainsKey(identifier) && overwriteCollection == false) return;

			// Converting to interface list
			var metaFiles = new List<IMetaFile>();
			foreach (var row in strictList) {
				metaFiles.Add(row);
			}

			// Adding to collection
			_metaFiles.Add(identifier, metaFiles);

		}



		private string ComputeRelativePath(string absolutePath) {

			// ...
			_debugger.Trace(absolutePath);

			// ...
			var volume = Path.GetPathRoot(absolutePath)?.Length ?? 0;
			var path = absolutePath.Substring(volume + Instruction.RootDirectory.Length);

			// ...
			return path[0] == '\\'
				? path.Substring(1)
				: path;

		}



		private string ComputeIdentifier(string relativePath) {

			// ...
			_debugger.Trace(relativePath);

			// Converting string into array of bytes
			var bytes = new byte[relativePath.Length * sizeof(char)];
			Buffer.BlockCopy(relativePath.ToCharArray(), 0, bytes, 0, bytes.Length);

			// Computing hash and returning string representation
			var hash = _hashAlgorithm.ComputeHash(bytes);
			return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

		}



		public void Dispose() {

			if (_isDisposed) return;
			_isDisposed = true;

			// ...
			CloseMetaFiles();

			// ...
			_hashAlgorithm.Dispose();

		}



	}



}
