namespace Replica.Services {

	using System;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;

	using Replica.Native;
	using Replica.Interfaces;



	public sealed class DeviceChangeNotifier : NativeWindow, IDeviceChangeNotifier {

		public event EventHandler DeviceArrival;
		public event DeviceQueryRemoveEventHandler DeviceQueryRemove;
		public event EventHandler DeviceRemoved;

		private readonly IDebugger _debugger;
		private bool _isDisposed;



		public DeviceChangeNotifier(IDebugger debugger) {

			// ...
			(_debugger = debugger).Trace();

			// Creating window
			CreateHandle(new CreateParams());

			// Removing default style and showing window without stealing user-focus
			NativeMethods.SetWindowLong(Handle, (int)WindowLongFlags.GWL_STYLE, 0);
			NativeMethods.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, WindowPosFlags.SWP_NOACTIVATE);
		}



		protected override void WndProc(ref Message m) {

			// We're interested only in one message
			if (m.Msg == (int)WindowMessages.WM_DEVICECHANGE) {
				_debugger.Trace(WindowMessages.WM_DEVICECHANGE.ToString());

				switch ((DeviceBroadcastMessages)m.WParam.ToInt32()) {

					case DeviceBroadcastMessages.DBT_DEVICEARRIVAL:
						_debugger.Trace(DeviceBroadcastMessages.DBT_DEVICEARRIVAL.ToString());
						DeviceArrival?.Invoke(this, EventArgs.Empty);
						break;

					case DeviceBroadcastMessages.DBT_DEVICEQUERYREMOVE:
						_debugger.Trace(DeviceBroadcastMessages.DBT_DEVICEQUERYREMOVE.ToString());

						// Serves as a standard header for information related to a device event reported through the WM_DEVICECHANGE message.
						var deviceHeader = (DeviceBroadcastHeader)Marshal.PtrToStructure(m.LParam, typeof(DeviceBroadcastHeader));

						// ...
						if (deviceHeader.DeviceType == (int)DeviceType.DBT_DEVTYP_HANDLE) {

							// DEV_BROADCAST_HANDLE structure.
							var dbh = (DeviceBroadcastHandle)Marshal.PtrToStructure(m.LParam, typeof(DeviceBroadcastHandle));
							var args = new DeviceQueryRemoveEventArgs(dbh.DeviceHandle, dbh.DeviceNotificationHandle);
							DeviceQueryRemove?.Invoke(this, args);
							m.Result = (IntPtr)(args.Cancel ? 0 : 1);
							return;

						} else {

							// ...
							var message = $"{deviceHeader.DeviceType} => {(DeviceType)deviceHeader.DeviceType}";
							throw new Exception(message);

						}

					case DeviceBroadcastMessages.DBT_DEVICEREMOVECOMPLETE:
						_debugger.Trace(DeviceBroadcastMessages.DBT_DEVICEREMOVECOMPLETE.ToString());
						DeviceRemoved?.Invoke(this, EventArgs.Empty);
						return;

				}
			}

			// ...
			base.WndProc(ref m);

		}



		public void Dispose() {

			// ...
			if (_isDisposed) return;
			_isDisposed = false;

			// ...
			DestroyHandle();

		}



	}



}
