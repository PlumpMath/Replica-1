namespace Replica.Services {

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Threading;

	using Replica.Interfaces;



	public sealed class Cron : BackgroundWorker, ICron {

		private readonly List<Task> _tasks;
		private readonly IDebugger _debugger;



		private sealed class Task {

			public readonly int HashCode;
			public int Interval;
			public EventHandler EventHandler;

			public int Timer;
			public bool Enabled;

			public Task(int hashCode, EventHandler eventHandler, int interval) {
				HashCode = hashCode;
				Interval = interval;
				EventHandler = eventHandler;
				Enabled = true;
			}

		}



		public Cron(IDebugger debugger) {

			(_debugger = debugger).Trace();

			_tasks = new List<Task>();
			WorkerSupportsCancellation = true;
			RunWorkerAsync();

		}



		public int Register(EventHandler eventHandler, int interval) {

			var hashCode = eventHandler.GetHashCode();
			_debugger.Trace(hashCode.ToString());

			if (GetByIdentifier(hashCode)!= null) throw new Exception($"Task with specified event handler (hash code: '{hashCode}') already exists in table.");
			_tasks.Add(new Task(hashCode, eventHandler, interval));

			return hashCode;

		}



		public void Unregister(int hashCode) {

			_debugger.Trace(hashCode.ToString());

			for (var n = _tasks.Count - 1; n >= 0; n--) {
				if (_tasks[n].HashCode != hashCode) {
					_tasks[n].Enabled = false;
				}
			}
			
		}



		private Task GetByIdentifier(int hashCode) {

			for (var n = 0; n < _tasks.Count; n++) {
				if (_tasks[n].HashCode == hashCode) {
					return _tasks[n];
				}
			}

			return null;

		}



		protected override void OnDoWork(DoWorkEventArgs e) {

			while (!CancellationPending) {
				
				// ...
				Thread.Sleep(1000);

				// ...
				for (var n = _tasks.Count - 1; n >= 0; n--) {

					// Removing task, that has been disabled. We're doing this here,
					// since we don't want to 'catch' an exception, when we iterating
					// through list when one of the row has been deleted by another thread.
					if (_tasks[n].Enabled == false) {
						_debugger.Trace($"Removing task ({_tasks[n].HashCode}) from table.");
						_tasks.RemoveAt(n);
					}

					// Invoking event handler, if time has come
					if (++_tasks[n].Timer >= _tasks[n].Interval) {
						_tasks[n].Timer = 0;
						_tasks[n].EventHandler.Invoke(this, EventArgs.Empty);
					}

				}

			}

		}



	}



}
