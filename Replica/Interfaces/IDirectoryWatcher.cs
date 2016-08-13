namespace Replica.Interfaces {

	using Replica.Interop.EventHandlers;



	public interface IDirectoryWatcher {

		event DirectoryChangedEventHandler DirectoryChanged;
		string TargetDirectory { get; }
		void Initialize(string path);

	}



}
