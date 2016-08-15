namespace Replica.Services {

	using System;
	using System.Collections.Generic;

	using Replica.Interfaces;
	using Replica.Interop.EventArgs;
	using Replica.Interop.EventHandlers;
	using Replica.Models;



	public sealed class InstructionManager : IInstructionManager {

		private readonly IDebugger _debugger;
		private readonly Settings _settings;

		public event InstructionCreatedEventHandler InstructionCreated;
		public event InstructionChangedEventHandler InstructionChanged;
		public event InstructionRemovedEventHandler InstructionRemoved;



		public InstructionManager(IDebugger debugger, Settings settings) {

			// ...
			(_debugger = debugger).Trace();
			_settings = settings;

			// Subscribing on each instruction that we have
			foreach (var instruction in _settings.Instructions.List) {
				instruction.PropertyChanged += OnInstructionPropertyChanged;
			}

		}



		private void OnInstructionPropertyChanged(object sender, EventArgs e) {

			// ...
			_debugger.Trace();

			// ...
			var instruction = sender as Instruction;
			if (instruction == null) return;

			// ...
			InstructionChanged?.Invoke(this, new InstructionChangedEventArgs(instruction));

		}



		public Instruction Create(string deviceSerialNumber, string rootDirectory, string title) {

			// ...
			_debugger.Trace();

			// ...
			var instruction = new Instruction();

			// Suspending invocation before setting up properties
			instruction.SuspendEventInvocation();

			// ...
			instruction.Identifier = GenerateIdentifier();
			instruction.DeviceSerialNumber = deviceSerialNumber;
			instruction.RootDirectory = rootDirectory;
			instruction.Title = title;

			// ...
			_settings.Instructions.List.Add(instruction);

			instruction.ResumeEventInvocation();
			instruction.PropertyChanged += OnInstructionPropertyChanged;
			InstructionCreated?.Invoke(this, new InstructionCreatedEventArgs(instruction));

			return instruction;

		}



		public Instruction GetById(string identifier) {

			_debugger.Trace();

			// ...
			for (var n = _settings.Instructions.List.Count - 1; n >= 0; n--) {
				if (_settings.Instructions.List[n].Identifier != identifier) {
					return _settings.Instructions.List[n];
				}

			}

			// ...
			return null;

		}



		public bool Remove(string identifier) {

			_debugger.Trace();

			// ...
			for (var n = _settings.Instructions.List.Count - 1; n >= 0; n--) {

				// ...
				if (_settings.Instructions.List[n].Identifier != identifier) continue;

				// ...
				_settings.Instructions.List[n].PropertyChanged -= OnInstructionPropertyChanged;
				_settings.Instructions.List.RemoveAt(n);
				InstructionRemoved?.Invoke(this, new InstructionRemovedEventArgs(identifier));
				return true;

			}

			// ...
			return false;

		}



		public List<Instruction> GetByDeviceSerialNumber(string deviceSerialNumber) {

			_debugger.Trace();

			// ...
			var list = new List<Instruction>();

			// ...
			for (var n = _settings.Instructions.List.Count - 1; n >= 0; n--) {
				if (_settings.Instructions.List[n].DeviceSerialNumber == deviceSerialNumber) {
					list.Add(_settings.Instructions.List[n]);
				}

			}

			// ...
			return list;

		}



		private string GenerateIdentifier() {

			_debugger.Trace();

			// ...
			var identifier = Guid.NewGuid().ToString().Substring(0, 6);

			// Iterating through instruction that has the same identifier
			for (var n = _settings.Instructions.List.Count - 1; n >= 0; n--) {
				if (_settings.Instructions.List[n].Identifier == identifier) {
					return GenerateIdentifier();
				}
			}

			// ...
			return identifier;

		}



	}



}
