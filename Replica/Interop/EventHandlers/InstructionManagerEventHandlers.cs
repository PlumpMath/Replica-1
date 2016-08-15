namespace Replica.Interop.EventHandlers {

	using Replica.Interfaces;
	using Replica.Interop.EventArgs;


	public delegate void InstructionCreatedEventHandler(IInstructionManager instructionManager, InstructionCreatedEventArgs args);
	public delegate void InstructionRemovedEventHandler(IInstructionManager instructionManager, InstructionRemovedEventArgs args);
	public delegate void InstructionChangedEventHandler(IInstructionManager instructionManager, InstructionChangedEventArgs args);



}
