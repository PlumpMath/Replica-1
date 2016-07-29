namespace Replica.Interfaces {

	using System;
	using System.Runtime.CompilerServices;



	public interface IContainer {

		TService GetInstance<TService>([CallerMemberName] string methodName = null) where TService : class;
		object GetInstnace(Type type, [CallerMemberName] string methodName = null);

	}

}
