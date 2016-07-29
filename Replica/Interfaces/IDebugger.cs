namespace Replica.Interfaces {

	using System;
	using System.Runtime.CompilerServices;



	public interface IDebugger : IDisposable {

		void Trace(string line = null, [CallerMemberName] string methodName = null);
		void WriteLine(string line);

	}

}
