namespace Replica.Interop.EventArgs {

	using System;
	using Replica.Models;


	public sealed class InstructionChangedEventArgs : EventArgs {

		public readonly Instruction Instruction;

		public InstructionChangedEventArgs(Instruction instruction) {
			Instruction = instruction;
		}

	}

}
