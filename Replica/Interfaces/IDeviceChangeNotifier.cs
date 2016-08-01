namespace Replica.Interfaces {

	using System;
	using System.ComponentModel;



	public delegate void DeviceQueryRemoveEventHandler(object sender, DeviceQueryRemoveEventArgs e);



	public sealed class DeviceQueryRemoveEventArgs : CancelEventArgs {

		public IntPtr DeviceHandle { get; }
		public IntPtr DeviceNotificationHandle { get; }



		public DeviceQueryRemoveEventArgs(IntPtr deviceHandle, IntPtr deviceNotificationHandle) {
			DeviceHandle = deviceHandle;
			DeviceNotificationHandle = deviceNotificationHandle;
		}

	}



	public interface IDeviceChangeNotifier : IDisposable {

		IntPtr Handle { get; }

		event EventHandler DeviceArrival;
		event DeviceQueryRemoveEventHandler DeviceQueryRemove;
		event EventHandler DeviceRemoved;

	}



}
