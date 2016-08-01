namespace Replica.Native {

	using System;
	using System.Runtime.InteropServices;




	/// <summary>Contains information about a file system handle.</summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DeviceBroadcastHandle {

		/// <summary>The size of this structure, in bytes.</summary>
		public int Size;

		/// <summary>Set to DeviceType.DBT_DEVTYP_HANDLE.</summary>
		public int DeviceType;

		/// <summary>Reserved; do not use.</summary>
		public int Reserved;

		/// <summary>A handle to the device to be checked.</summary>
		public IntPtr DeviceHandle;

		/// <summary>A handle to the device notification. This handle is returned by RegisterDeviceNotification.</summary>
		public IntPtr DeviceNotificationHandle;

		/// <summary>The GUID for the custom event. For more information, see Device Events. Valid only for DBT_CUSTOMEVENT.</summary>
		public Guid EventGuid;

		/// <summary>The offset of an optional string buffer. Valid only for DBT_CUSTOMEVENT.</summary>
		public long NameOffset;

		/// <summary>Optional binary data. This member is valid only for DBT_CUSTOMEVENT.</summary>
		public byte OptionalData0;

		/// <summary>Optional binary data. This member is valid only for DBT_CUSTOMEVENT.</summary>
		public byte OptionalData1;

	}




}
