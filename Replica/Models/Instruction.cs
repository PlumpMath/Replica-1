namespace Replica.Models {

	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;



	public sealed class Instruction : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		public string Title {
			get { return _title; }
			set { SetField(ref _title, value); }
		}

		public string Identifier {
			get { return _identifier; }
			set { SetField(ref _identifier, value); }
		}

		public string DeviceSerialNumber {
			get { return _deviceSerialNumber; }
			set { SetField(ref _deviceSerialNumber, value); }
		}

		public string RootDirectory {
			get { return _rootDirectory; }
			set { SetField(ref _rootDirectory, value); }
		}

		public bool IsActive {
			get { return _isActive; }
			set { SetField(ref _isActive, value); }
		}

		public readonly ObservableCollection<string> Directives;

		public bool IsEventInvocationSuspended { get; private set; }

		private string _title;
		private string _identifier;
		private string _deviceSerialNumber;
		private string _rootDirectory;

		private bool _isActive;



		public Instruction() {

			Directives = new ObservableCollection<string>();
			Directives.CollectionChanged += (sender, args) => {
				if (!IsEventInvocationSuspended) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Directives)));
			};

		}



		public void SuspendEventInvocation() {
			IsEventInvocationSuspended = true;
		}



		public void ResumeEventInvocation() {
			IsEventInvocationSuspended = false;
		}



		private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {

			// Do nothing, if values are equal
			if (EqualityComparer<T>.Default.Equals(field, value)) return;
			field = value;

			// Firing event, if not suspended
			if (!IsEventInvocationSuspended) {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

		}



	}

}
