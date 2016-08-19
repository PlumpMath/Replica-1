namespace Replica.Interfaces {

	using System;
	using System.ComponentModel;



	public interface IMetaLayer : INotifyPropertyChanged {

		long Length { get; set; }
		DateTime Created { get; set; }
		DateTime Modified { get; set; }

		string RelativePath { get; set; }
		string ContentLocation { get; set; }
		bool IsDeltaFile { get; set; }
		bool IsChanged { get; set; }

		bool IsNotificationSuspended { get; set; }

	}



}
