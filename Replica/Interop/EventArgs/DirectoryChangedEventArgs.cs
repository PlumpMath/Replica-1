namespace Replica.Interop.EventArgs {

	using System;



	public sealed class DirectoryChangedEventArgs {

		public readonly string Path;

		public DirectoryChangedEventArgs(string path) {
			Path = path;
		}

	}

}
