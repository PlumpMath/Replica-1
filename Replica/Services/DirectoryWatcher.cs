namespace Replica.Services {

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	
	using Replica.Interfaces;
	using Replica.Interop.EventArgs;
	using Replica.Interop.EventHandlers;



	public sealed class DirectoryWatcher : BackgroundWorker, IDirectoryWatcher {

		public event DirectoryChangedEventHandler DirectoryChanged;

		private FileSystemWatcher _fileSystemWatcher;
		private bool _fileSystemWatcherLastState;
		private int _cronTaskIdentifier;

		private bool _isDisposed;

		private readonly IDebugger _debugger;
		private readonly IUnixTime _unixTime;
		private readonly ICron _cron;

		private readonly Queue<string> _queue;
		private readonly Dictionary<string, int> _directories;

		public string TargetDirectory { get; private set; }



		public DirectoryWatcher(IDebugger debugger, IUnixTime unixTime, ICron cron) {

			// ...
			(_debugger = debugger).Trace();
			_unixTime = unixTime;
			_cron = cron;

			// Registering cron-task
			_cronTaskIdentifier = _cron.Register(FileSystemWatcherCheck, 30);

			// ...
			_queue = new Queue<string>();
			_directories = new Dictionary<string, int>();

		}



		public void Initialize(string path) {

			// ...
			_debugger.Trace(path);

			// ...
			TargetDirectory = path;
			WorkerSupportsCancellation = true;

			// ...
			FileSystemWatcherCheck(this, EventArgs.Empty);
			
		}



		protected override void OnDoWork(DoWorkEventArgs e) {

			// ...
			while (_directories.Count != 0 || _queue.Count != 0) {

				_debugger.Trace();

				// First, clearing queue...
				while (_queue.Count > 0) {

					// Retireving path and going level up
					var key = Path.GetDirectoryName(_queue.Dequeue()) + Path.DirectorySeparatorChar;

					// Depending on existence of key, adding or updating time
					if (!_directories.ContainsKey(key)) {
						_directories.Add(key, _unixTime.Now);

					} else {
						_directories[key] = _unixTime.Now;

					}

				}
				
				// ...
				var path = string.Empty;

				// Searching for expired row.
				foreach (var pair in _directories) {
					if (_unixTime.Difference(pair.Value + 5) > 0) {
						path = pair.Key;
						break;
					}
				}

				// Invoking event & removing expired row
				if (!string.IsNullOrEmpty(path)) {
					_directories.Remove(path);
					DirectoryChanged?.Invoke(this, new DirectoryChangedEventArgs(path));
				}
			}

		}



		protected override void Dispose(bool disposing) {

			_debugger.Trace();

			// ...
			if (_isDisposed) return;
			_isDisposed = true;

			// ...
			_cron.Unregister(_cronTaskIdentifier);
			_fileSystemWatcher.Dispose();

			// ...
			base.Dispose(disposing);

		}



		private void FileSystemWatcherCheck(object sender, EventArgs e) {

			// ...
			_debugger.Trace();

			// Determining presence of directory
			var directoryExists = Directory.Exists(TargetDirectory);

			// Creating instance of FileSystem watcher if:
			// - Directory exists
			// - FileSystemWatcher not created
			if (directoryExists && _fileSystemWatcher == null) {

				_debugger.WriteLine("Creating instance of FileSystemWatcher.");

				_fileSystemWatcher = new FileSystemWatcher(TargetDirectory);
				_fileSystemWatcher.Changed += OnFileChanged;
				_fileSystemWatcher.Created += OnFileChanged;
				_fileSystemWatcher.Deleted += OnFileChanged;
				_fileSystemWatcher.Renamed += OnFileRenamed;

				_fileSystemWatcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite;

				_fileSystemWatcher.IncludeSubdirectories = true;
				_fileSystemWatcher.EnableRaisingEvents = true;

			}

			// Disposing from FileSystemWatcher, if directory has been deleted
			if (!directoryExists && _fileSystemWatcher != null) {

				_debugger.WriteLine("Disposing instance of FileSystemWatcher.");

				_fileSystemWatcher.Changed -= OnFileChanged;
				_fileSystemWatcher.Created -= OnFileChanged;
				_fileSystemWatcher.Deleted -= OnFileChanged;
				_fileSystemWatcher.Renamed -= OnFileRenamed;
				_fileSystemWatcher.Dispose();
				_fileSystemWatcher = null;
			}

		}



		private void OnFileChanged(object sender, FileSystemEventArgs e) {
			_queue.Enqueue(e.FullPath);
			if (!IsBusy) RunWorkerAsync();
		}



		private void OnFileRenamed(object sender, RenamedEventArgs e) {
			_queue.Enqueue(e.FullPath);
			_queue.Enqueue(e.OldFullPath);
			if (!IsBusy) RunWorkerAsync();
		}



	}



}
