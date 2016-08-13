namespace Replica.Services {

	using System;
	using System.ComponentModel;
	using System.Threading;

	using Replica.Interfaces;



	public sealed class UnixTime : BackgroundWorker, IUnixTime {

		private volatile int _now;
		public int Now => _now;



		public UnixTime() {

			RunWorkerAsync();

		}



		protected override void OnDoWork(DoWorkEventArgs e) {

			// ...
			_now = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

			// ...
			while (true) {

				// ...
				Thread.Sleep(1000);
				_now += 1;

				// ...
				if (CancellationPending) break;

			}

		}



		protected override void Dispose(bool disposing) {

			CancelAsync();
			base.Dispose(disposing);

		}



		public int Difference(int value) {
			return _now - value;
		}



	}



}
