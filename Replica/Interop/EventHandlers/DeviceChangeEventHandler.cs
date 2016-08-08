namespace Replica.Interop.EventHandlers {

	using Replica.Interfaces;
	using Replica.Interop.EventArgs;


	public delegate void DeviceArrivedEventHandler(IDeviceManager deviceManager, DeviceArrivedEventArgs args);
	public delegate void DeviceRemovedEventHandler(IDeviceManager deviceManager, DeviceRemovedEventArgs args);
	public delegate void DeviceQueryRemoveEventHandler(object sender, DeviceQueryRemoveEventArgs e);



}
