namespace Replica.Interop.EventArgs {

	using System;
	using Replica.Interfaces;



	public sealed class DeviceArrivedEventArgs : EventArgs {

		public IDevice Device { get; }

		public DeviceArrivedEventArgs(IDevice device) {
			Device = device;
		}

	}



}
