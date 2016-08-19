namespace Replica.Models {

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	using Replica.Interfaces;



	public sealed class MetaFile : IMetaFile {

		public ObservableCollection<IMetaLayer> Layers { get; private set; }



		public string Identifier {
			get { return _identifier; }
			set { SetField(ref _identifier, value); }
		}



		public string RelativePath {
			get { return _relativePath; }
			set { SetField(ref _relativePath, value); }
		}



		public bool IsChanged { get; set; }



		public event PropertyChangedEventHandler PropertyChanged;
		public bool IsNotificationSuspended { get; set; }



		private string _identifier;
		private string _relativePath;



		public MetaFile() {

			Layers = new ObservableCollection<IMetaLayer>();
			Layers.CollectionChanged += OnObservabbleCollectionChanged;

		}



		public MetaFile(string identifier, string relativePath) : base() {

			_identifier = identifier;
			_relativePath = relativePath;

		}



		private void OnObservabbleCollectionChanged(object sender, EventArgs e) {

			// ...
			IsChanged = true;

			// Firing event, if not suspended
			if (IsNotificationSuspended) return;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Layers)));

		}



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



		public void Dispose() {

			Layers.CollectionChanged -= OnObservabbleCollectionChanged;

		}



	}



}
