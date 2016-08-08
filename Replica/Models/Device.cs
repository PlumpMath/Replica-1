namespace Replica.Models {

	using System;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Text;

	using Replica.Interfaces;
	using Replica.Interop.EventArgs;
	using Replica.Interop.EventHandlers;
	using Replica.Native;


	
	public class Device : IDevice {

		public event DeviceQueryRemoveEventHandler QueryRemove;
		public event DeviceRemovedEventHandler Removed;

		public DriveInfo DriveInfo { get; private set; }
		public string SerialNumber { get; private set; }

		public bool IsReady => Handle != IntPtr.Zero;
		public IntPtr Handle { get; private set; }

		private readonly IDebugger _debugger;
		private readonly IDeviceManager _deviceManager;

		private IntPtr _deviceNotificationHandle;



		public Device(IDebugger debugger, IDeviceManager deviceManager) {
			(_debugger = debugger).Trace();
			_deviceManager = deviceManager;
		}



		public void Initialize(DriveInfo driveInfo) {

			_debugger.Trace();

			DriveInfo = driveInfo;
			SerialNumber = GetVolumeSerialNumber(DriveInfo.Name);

			// ...
			RegisterDeviceNotification();

		}

		

		public bool Remove() {

			_debugger.Trace();

			// Firing event, that might prevent device removal
			var e = new DeviceQueryRemoveEventArgs();
			QueryRemove?.Invoke(this, e);

			// Checking cancellation
			if (e.Cancel) return false;

			// Unregistering and firing another event, that will notify all subscribers,
			// that device-removal is approved. This will help subscribers what to do next.
			Unregister();
			Removed?.Invoke(_deviceManager, new DeviceRemovedEventArgs(this));

			// ...
			return true;

		}



		private void RegisterDeviceNotification() {

			_debugger.Trace();

			// Creating dbh-structure, that will contain all necessary information,
			// that required by system in order to register notification.
			var deviceHandle = new DeviceBroadcastHandle();
			deviceHandle.Size = Marshal.SizeOf(deviceHandle);
			deviceHandle.DeviceType = (int)DeviceType.DBT_DEVTYP_HANDLE;

			// Creating handle to an empty file. Handle will be placed in dbh-structure and this instance.
			Handle = deviceHandle.DeviceHandle = NativeMethods.CreateFile(DriveInfo.Name, (uint)FileDesiredAccess.GENERIC_READ, (uint)FileShare.ReadWrite,
					0, (uint)FileCreationDisposition.OPEN_EXISTING, (uint)FileFlags.BACKUP_SEMANTICS | (uint)Native.FileAttributes.NORMAL, 0);

			// Allocating memory and converting dbh-structure to IntPtr.
			var data = Marshal.AllocHGlobal(deviceHandle.Size);
			Marshal.StructureToPtr(deviceHandle, data, true);

			// Registering device notification and catching win32-exception
			_deviceNotificationHandle = NativeMethods.RegisterDeviceNotification(_deviceManager.Handle, data, (uint)DeviceNotifyFlags.WINDOW_HANDLE);
			var innerException = new Win32Exception(Marshal.GetLastWin32Error());

			// Rethrowing exception, if something went wrong
			if (_deviceNotificationHandle == IntPtr.Zero || Handle == IntPtr.Zero) {
				throw new Exception("Failed to register device notification handle.", innerException);
			}

		}



		private void Unregister() {

			_debugger.Trace();

			// Closing handle on file, opened in order to recieve WM_DEVICECHANGE message,
			// when device was requesting for removing from system.
			if (!NativeMethods.CloseHandle(Handle)) {
				throw new NotImplementedException();
			}

			// Unregistering notification
			if (!NativeMethods.UnregisterDeviceNotification(_deviceNotificationHandle)) {
				throw new NotImplementedException();
			}

			// Clearing values, so that this.IsLocked can properly answers on question
			Handle = IntPtr.Zero;
			_deviceNotificationHandle = IntPtr.Zero;

		}



		private string GetVolumeSerialNumber(string driveLetter) {

			_debugger.Trace(driveLetter);

			var volumeName = new StringBuilder();
			var fileSystemName = new StringBuilder();

			uint serialNumber, length;
			FileSystemFeature flags;

			// Retrieving information about specified volume
			if (!NativeMethods.GetVolumeInformation(driveLetter, volumeName, volumeName.Capacity,
				out serialNumber, out length, out flags, fileSystemName, fileSystemName.Capacity)) {
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}

			// Calculating high and low parts
			var hPart = serialNumber / 65536;
			var lPart = serialNumber - hPart * 65536L;

			// Converting
			var left = hPart.ToString("X").PadLeft(4, '0');
			var right = lPart.ToString("X").PadLeft(4, '0');

			// ...
			return $"{left}-{right}";

		}



	}



}
