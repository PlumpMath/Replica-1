namespace Replica.Native {

	using System.Runtime.InteropServices;




	/// <summary>Serves as a standard header for information related to a device event reported through the WM_DEVICECHANGE message. The members of the DEV_BROADCAST_HDR structure are contained in each device management structure. To determine which structure you have received through WM_DEVICECHANGE, treat the structure as a DEV_BROADCAST_HDR structure and check its dbch_devicetype member.</summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DeviceBroadcastHeader {

		/// <summary>The size of this structure, in bytes.</summary>
		public int Size;

		/// <summary>Set to DeviceType.DBT_DEVTYP_HANDLE.</summary>
		public int DeviceType;

		/// <summary>Reserved; do not use.</summary>
		public int Reserved;

	}




}
