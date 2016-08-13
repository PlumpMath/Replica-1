namespace Replica.Interop.EventHandlers {

	using Replica.Interop.EventArgs;
	using Replica.Interfaces;



	public delegate void DirectoryChangedEventHandler(IDirectoryWatcher fileSystemWatcher, DirectoryChangedEventArgs args);

}
