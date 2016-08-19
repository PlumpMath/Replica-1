namespace Replica.Models {

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	using Replica.Interfaces;



	public sealed class MetaLayer : IMetaLayer {

		private long _length;
		private DateTime _created;
		private DateTime _modified;

		private string _relativePath;
		private string _contentLocation;
		private bool _isDeltaFile;



		public string RelativePath {
			get { return _relativePath; }
			set { SetField(ref _relativePath, value); }
		}



		public string ContentLocation {
			get { return _contentLocation; }
			set { SetField(ref _contentLocation, value); }
		}



		public long Length {
			get { return _length; }
			set { SetField(ref _length, value); }
		}



		public DateTime Created {
			get { return _created; }
			set { SetField(ref _created, value); }
		}



		public DateTime Modified {
			get { return _modified; }
			set { SetField(ref _modified, value); }
		}



		public bool IsDeltaFile {
			get { return _isDeltaFile; }
			set { SetField(ref _isDeltaFile, value); }
		}



		public bool IsChanged { get; set; }



		public event PropertyChangedEventHandler PropertyChanged;
		public bool IsNotificationSuspended { get; set; }



		private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {

			// Do nothing, if values are equal
			if (EqualityComparer<T>.Default.Equals(field, value)) return;
			field = value;

			// ...
			IsChanged = true;

			// Firing event, if not suspended
			if (IsNotificationSuspended) return;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		}



	}



}
