namespace Replica.Services {

	using System.Collections.Concurrent;
	using System.IO;
	using System.Threading;

	using Replica.Interfaces;
	using Replica.Interop;
	using Replica.Interop.EventArgs;
	using Replica.Interop.EventHandlers;



	public sealed class FileDifferenceDetector : IFileDifferenceDetector {

		private IDebugger _debugger;



		public FileDifferenceDetector(IDebugger debugger) {
			_debugger = debugger;
		}



		public FileCompareResult Equals(string filename0, string filename1, CancelableProgressEventHandler eventHandler) {

			// ...
			_debugger.Trace($"'{filename0}' ? '{filename1}'");

			// Creating file streams
			var fileStream0 = new FileStream(filename0, FileMode.Open, FileAccess.Read);
			var fileStream1 = new FileStream(filename1, FileMode.Open, FileAccess.Read);

			// Comparing size of filestreams
			if (fileStream0.Length != fileStream1.Length) {
				fileStream0.Dispose();
				fileStream1.Dispose();
				return FileCompareResult.NotEqual;
			}

			// Creating instances of Stream
			var stream0 = new Stream(fileStream0);
			var stream1 = new Stream(fileStream1);

			// ...
			var cancelEventArgs = new CancelableProgressEventArgs();
			var comparisonResult = FileCompareResult.Equal;

			// Entering in cycle of comparing content of files
			while (comparisonResult == FileCompareResult.Equal) {

				// Notifying about progress
				if (eventHandler != null) {
					cancelEventArgs.Percentage = CalculatePercentage(stream0.FileStream.Position, stream0.FileStream.Length);
					eventHandler.Invoke(this, cancelEventArgs);

					// If reciever of event requests cancellation of process
					if (cancelEventArgs.Cancel) {
						stream0.CancellationPending = stream1.CancellationPending = true;
						comparisonResult = FileCompareResult.Cancelled;
						break;
					}
				}

				// Wait, until both queues are filled with atleast one byte-array each
				if (stream0.Queue.IsEmpty || stream1.Queue.IsEmpty) {

					// Break cycle, if streams are not alive
					if (!stream0.IsWorking && !stream1.IsWorking) break;

					// Waiting for external locks to be unlocked from secodary threads
					stream0.ExternalLock.WaitOne();
					stream1.ExternalLock.WaitOne();
					continue;

				}

				// Retrieving byte-array from each stream
				byte[] bytes0, bytes1;
				stream0.Queue.TryDequeue(out bytes0);
				stream1.Queue.TryDequeue(out bytes1);

				// Unlocking thread only if queue limit is not reached
				if (stream0.QueueLimit > stream0.Queue.Count) stream0.ThreadLock.Set();
				if (stream1.QueueLimit > stream1.Queue.Count) stream1.ThreadLock.Set();

				// Comparing byte-arrays to each other
				for (var n = 0; n < bytes0.Length; n++) {
					if (bytes0[n] != bytes1[n]) {
						comparisonResult = FileCompareResult.NotEqual;
						break;
					}
				}

			}

			// Releasing thread lock one last time
			stream0.ThreadLock.Set();
			stream1.ThreadLock.Set();

			// Releasing dispose lock, that will allow dispose
			// of all resources, that was used by each thread.
			stream0.DisposeLock.Set();
			stream1.DisposeLock.Set();

			// ...
			return comparisonResult;

		}



		private static int CalculatePercentage(long value, long max) {
			if (value == 0 || max == 0) return 0;
			return (int) ((float)value / max * 100f);
		}



		private sealed class Stream {

			public FileStream FileStream;

			public bool CancellationPending;
			public bool IsWorking = true;

			public ConcurrentQueue<byte[]> Queue;

			public readonly int QueueLimit;
			public readonly int BlockLength;

			public ManualResetEvent ThreadLock;
			public ManualResetEvent DisposeLock;
			public ManualResetEvent ExternalLock;


			
			public Stream(FileStream fileStream, int queueLimit = 50, int blockLength = 250000) {

				// ...
				Queue = new ConcurrentQueue<byte[]>();

				// ...
				ThreadLock = new ManualResetEvent(false);
				DisposeLock = new ManualResetEvent(false);
				ExternalLock = new ManualResetEvent(false);

				// ...
				FileStream = fileStream;

				// ...
				QueueLimit = queueLimit;
				BlockLength = blockLength;

				// ...
				new Thread(ThreadProcess).Start();

			}



			private void ThreadProcess(object parameter) {

				// Stay in cycle, until it cancelled or we've reached the end of file
				while (!CancellationPending && FileStream.Position != FileStream.Length) {

					// Wait, until queue is processed
					if (Queue.Count == QueueLimit) {
						ThreadLock.WaitOne();
						continue;
					}

					// Reading and adding byte-array into queue
					var data = new byte[BlockLength];
					FileStream.Read(data, 0, data.Length);
					Queue.Enqueue(data);

					// Signaling to external thread, that queue
					// has been filled with byte-array.
					ExternalLock.Set();

				}

				// Waiting for signal from external thread
				IsWorking = false;
				DisposeLock.WaitOne();

				// Releasing all resources
				DisposeLock.Dispose();
				ThreadLock.Dispose();
				ExternalLock.Dispose();
				FileStream.Dispose();

			}



		}



	}



}
