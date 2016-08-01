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

			// Services
			container.RegisterSingleton<IDebugger>(() => new Services.Debugger(AppDomain.CurrentDomain.BaseDirectory));
			container.RegisterSingleton<IIssueContainer, Services.IssueContainer>();
			container.RegisterSingleton<IDeviceChangeNotifier, Services.DeviceChangeNotifier>();
			container.RegisterSingleton<Services.Settings>(() => Services.Settings.Create(container.GetInstance<IContainer>()));

			// Requesting services
			container.GetInstance<IDebugger>().Trace();
			container.GetInstance<Services.Settings>();

		}



	}

}
