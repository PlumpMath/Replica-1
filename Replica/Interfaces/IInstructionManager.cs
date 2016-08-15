namespace Replica.Interfaces {

	using System;
	using System.Collections.Generic;

	using Replica.Interop.EventHandlers;
	using Replica.Models;



	public interface IInstructionManager {

		event InstructionCreatedEventHandler InstructionCreated;
		event InstructionChangedEventHandler InstructionChanged;
		event InstructionRemovedEventHandler InstructionRemoved;

		Instruction Create(string deviceSerialNumber, string rootDirectory, string title);

		Instruction GetById(string identifier);
		bool Remove(string identifier);

		List<Instruction> GetByDeviceSerialNumber(string deviceSerialNumber);

	}



}
