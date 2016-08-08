namespace Replica.Services {

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;

	using Replica.Interfaces;
	using Replica.Interop.EventArgs;
	using Replica.Interop.EventHandlers;
	using Replica.Native;



	public sealed class DeviceManager : NativeWindow, IDeviceManager {

		public List<IDevice> Devices { get; }

		public event DeviceArrivedEventHandler DeviceArrived;
		public event DeviceRemovedEventHandler DeviceRemoved;

		private bool _isDisposed;
		private readonly IDebugger _debugger;
		private readonly IContainer _container;



		public DeviceManager(IContainer container, IDebugger debugger) {

			(_debugger = debugger).Trace();
			_container = container;

			Devices = new List<IDevice>();

		}



		public void Initialize() {

			_debugger.Trace();

			// Creating window
			CreateHandle(new CreateParams());

			// Removing default style and showing window without stealing user-focus
			NativeMethods.SetWindowLong(Handle, (int)WindowLongFlags.GWL_STYLE, 0);
			NativeMethods.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, WindowPosFlags.SWP_NOACTIVATE);

			// It's obvious, that there are already inserted devices,
			// and we won't get events about them getting inserted.
			OnDeviceArrival();

		}


		protected override void WndProc(ref Message m) {

			// React on WM_DEVICECHANGE message
			if (m.Msg == (int) WindowMessages.WM_DEVICECHANGE) {

				switch (m.WParam.ToInt32()) {

					// When device arrived
					case (int) DeviceBroadcastMessages.DBT_DEVICEARRIVAL:
						OnDeviceArrival();
						break;

					// When system asks device to be removed
					case (int) DeviceBroadcastMessages.DBT_DEVICEQUERYREMOVE:
						OnDeviceQueryRemove(ref m);
						break;

				}

			}

			// ...
			base.WndProc(ref m);

		}



		private void OnDeviceArrival() {

			_debugger.Trace();

			// Searching for new devices
			foreach (var driveInfo in DriveInfo.GetDrives()) {
				
				// Ignore already presented devices
				if (IsPresent(driveInfo.GetHashCode())) continue;

				// Creating and initializing new device
				var device = _container.GetInstance<IDevice>();
				device.Initialize(driveInfo);
				
				// Adding to list and throwing event about that
				Devices.Add(device);
				DeviceArrived?.Invoke(this, new DeviceArrivedEventArgs(device));

			}
			
		}



		private void OnDeviceQueryRemove(ref Message m) {
			
			_debugger.Trace();

			// Serves as a standard header for information related to a device event reported through the WM_DEVICECHANGE message.
			var deviceHeader = (DeviceBroadcastHeader)Marshal.PtrToStructure(m.LParam, typeof(DeviceBroadcastHeader));

			// ...
			if (deviceHeader.DeviceType != (int) DeviceType.DBT_DEVTYP_HANDLE) return;
			
			// DEV_BROADCAST_HANDLE structure.
			var dbh = (DeviceBroadcastHandle)Marshal.PtrToStructure(m.LParam, typeof(DeviceBroadcastHandle));

			// Searching for device, that must be removed
			for (var n = Devices.Count - 1; n >= 0; n--) {
				
				// Skipping devices, which Handle doesn't equals with one that we searching for
				if (Devices[n].Handle != dbh.DeviceHandle) continue;

				// Attempting to remove device, which can be canceled (see IDevice)
				if (!Devices[n].Remove()) {
					m.Result = (IntPtr)0;
					return;
				}

				// Since we're here, the result is successful
				m.Result = (IntPtr) 1;

				// Removing device from list and throwing event about that
				var device = Devices[n];
				Devices.RemoveAt(n);
				DeviceRemoved?.Invoke(this, new DeviceRemovedEventArgs(device));

			}

		}



		private bool IsPresent(int hashCode) {

			// Searching for IDevice, that contains DriveInfo-instance with specified hash-code
			for (var n = 0; n < Devices.Count; n++) {
				if (Devices[n].DriveInfo.GetHashCode() == hashCode) return true;
			}

			// ...
			return false;

		}



		public void Dispose() {

			_debugger?.Trace();

			// Return if already disposed
			if (_isDisposed) return;
			_isDisposed = true;

			// Destroying handle of NativeWindow
			DestroyHandle();

		}


	}


}