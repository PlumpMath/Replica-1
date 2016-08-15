namespace Replica.Interop.EventArgs {

	using System;
	using Replica.Models;



	public sealed class InstructionCreatedEventArgs : EventArgs {

		public readonly Instruction Instruction;



		public InstructionCreatedEventArgs(Instruction instruction) {
			Instruction = instruction;
		}

	}



}
