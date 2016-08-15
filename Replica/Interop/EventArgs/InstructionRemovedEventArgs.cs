namespace Replica.Interop.EventArgs {

	using System;



	public sealed class InstructionRemovedEventArgs : EventArgs {

		public readonly string Identifier;



		public InstructionRemovedEventArgs(string identifier) {
			Identifier = identifier;
		}

	}



}
