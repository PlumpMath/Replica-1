namespace Replica.Services {

	using System;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;

	using Interfaces;



	public sealed class Container : IContainer {

		private readonly SimpleInjector.Container _container;
		private readonly IDebugger _debugger;



		public Container(SimpleInjector.Container container, IDebugger debugger) {
			_container = container;
			_debugger = debugger;
		}



		public TService GetInstance<TService>([CallerMemberName] string methodName = null) where TService : class {

			// Gathering information about invoker and requested service
			var type = typeof(TService)?.Name;
			var method = new StackTrace().GetFrame(1).GetMethod();
			var path = method.ReflectedType?.FullName;
			var isStatic = method.IsStatic ? ":STATIC" : "";

			// ...
			methodName = methodName == ".ctor" ? "ctor" : methodName;

			// ...
			_debugger.Trace($"{path}->{methodName}(){isStatic} <- {type}");

			// Requesting and returning specified service
			return _container.GetInstance<TService>();

		}



		public object GetInstance(Type type, [CallerMemberName] string methodName = null) {

			// Gathering information about invoker and requested service
			var method = new StackTrace().GetFrame(1).GetMethod();
			var path = method.ReflectedType?.FullName;
			var isStatic = method.IsStatic ? ":STATIC" : "";

			// ...
			methodName = methodName == ".ctor" ? "ctor" : methodName;

			// ...
			_debugger.Trace($"{path}->{methodName}(){isStatic} <- {type.Name}");

			// Requesting and returning specified service
			return _container.GetInstance(type);

		}



	}



}
