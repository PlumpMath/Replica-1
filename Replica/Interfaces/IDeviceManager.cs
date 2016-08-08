namespace Replica.Interfaces {

	using System;
	using System.Collections.Generic;

	using Replica.Interop.EventHandlers;



	public interface IDeviceManager : IDisposable {

		IntPtr Handle { get; }
		List<IDevice> Devices { get; }

		event DeviceArrivedEventHandler DeviceArrived;
		event DeviceRemovedEventHandler DeviceRemoved;

		void Initialize();


	}

}