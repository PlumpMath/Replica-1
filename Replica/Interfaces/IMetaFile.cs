namespace Replica.Interfaces {

	using System;
	using System.Collections.ObjectModel;
	using System.ComponentModel;



	public interface IMetaFile : INotifyPropertyChanged, IDisposable {

		string Identifier { get; set; }
		string RelativePath { get; set; }
		bool IsChanged { get; set; }

		ObservableCollection<IMetaLayer> Layers { get; }
		bool IsNotificationSuspended { get; set; }

	}



}
