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
			container.RegisterSingleton<ICron, Services.Cron>();
			container.RegisterSingleton<IUnixTime, Services.UnixTime>();
			container.Register<IDirectoryWatcher, Services.DirectoryWatcher>(Lifestyle.Transient);
			container.RegisterSingleton<IDeviceManager, Services.DeviceManager>();
			container.RegisterSingleton<IStorageProvider, Services.StorageProvider>();
			container.RegisterSingleton<IFileDifferenceDetector, Services.FileDifferenceDetector>();
			container.RegisterSingleton<IDirectivesManager, Services.DirectivesManager>();
			container.RegisterSingleton<Services.Settings>(() => Services.Settings.Create(container.GetInstance<IContainer>()));

			// Models
			container.Register<IDevice, Models.Device>(Lifestyle.Transient);

			// Requesting services
			container.GetInstance<IDebugger>().Trace();
			container.GetInstance<Services.Settings>();

		}



	}

}
