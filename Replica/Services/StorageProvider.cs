namespace Replica.Services {

	using System.Collections.Generic;

	using Replica.Interfaces;
	using Replica.Models;



	public sealed class StorageProvider : IStorageProvider {

		private IInstructionManager _instructionManager;
		private IDebugger _debugger;

		private readonly List<IStorage> _storages;



		public StorageProvider(IInstructionManager instructionManager, IDebugger debugger) {

			// ...
			(_debugger = debugger).Trace();
			_instructionManager = instructionManager;

			// ...
			_storages = new List<IStorage>();

		}



		public IStorage Get(string identifier) {

			// ...
			_debugger.Trace();

			// Searching if it's already created
			foreach (var item in _storages) {
				if (item.Identifier == identifier) return item;
			}

			// Creating new instance of storage
			var instruction = _instructionManager.GetById(identifier);
			var storage = new Storage(_debugger);
			storage.Initialize(instruction);

			// Adding to collection because of two things:
			// - Garbage Collector will dispose instance, if there is no references on it.
			// - One storage per identifier
			_storages.Add(storage);

			// ...
			return storage;

		}



	}



}
