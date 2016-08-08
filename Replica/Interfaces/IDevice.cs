namespace Replica.Interfaces {

	using System;
	using System.IO;

	using Replica.Interop.EventHandlers;



	public interface IDevice {

		event DeviceQueryRemoveEventHandler QueryRemove;
		event DeviceRemovedEventHandler Removed;

		DriveInfo DriveInfo { get; }
		string SerialNumber { get; }

		bool IsReady { get; }
		IntPtr Handle { get; }

		void Initialize(DriveInfo driveInfo);
		bool Remove();

	}

}