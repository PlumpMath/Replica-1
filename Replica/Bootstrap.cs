namespace Replica {

	using System;
	using SimpleInjector;
	using Replica.Interfaces;



	internal static class Bootstrap {



		[STAThread]
		internal static void Main() {

			// Creating dependency injection container
			var container = new Container();

			// Registering SimpleInjector.Container and it's decorator
			container.Register(() => container, Lifestyle.Singleton);
			container.Register<IContainer, Services.Container>(Lifestyle.Singleton);

			// IDebugger
			container.RegisterSingleton(() => new Services.Debugger(AppDomain.CurrentDomain.BaseDirectory));

		}



	}

}
