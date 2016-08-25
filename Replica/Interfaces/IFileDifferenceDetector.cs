namespace Replica.Interfaces {

	using Replica.Interop;
	using Replica.Interop.EventHandlers;



	public interface IFileDifferenceDetector {

		FileCompareResult Equals(string filename0, string filename1, CancelableProgressEventHandler eventHandler);

	}

}
