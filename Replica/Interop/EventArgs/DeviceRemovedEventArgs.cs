namespace Replica.Interop.EventArgs {

	using System;
	using Replica.Interfaces;



	public sealed class DeviceRemovedEventArgs : EventArgs {

		public IDevice Device { get; }

		public DeviceRemovedEventArgs(IDevice device) {
			Device = device;
		}

	}



}
