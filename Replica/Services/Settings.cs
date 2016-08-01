namespace Replica.Services {

	using System;
	using System.IO;
	using Newtonsoft.Json;
	using Replica.Interfaces;



	public sealed partial class Settings {

		public LocalizationSection Localization = new LocalizationSection();
		public InstructionsSection Instructions = new InstructionsSection();

		[JsonIgnore]
		public volatile bool IsChanged;

		private readonly IDebugger _debugger;



		private Settings(IDebugger debugger, bool isChanged) {
			_debugger = debugger;
			IsChanged = isChanged;
		}



		public static Settings Create(IContainer container) {

			// ...
			var debugger = container.GetInstance<IDebugger>();
			debugger.Trace();

			// Generating path to settings.json
			var path = $"{AppDomain.CurrentDomain.BaseDirectory}/settings.json";

			// Creating new Settings with default values
			if (!File.Exists(path)) {
				debugger.WriteLine($"Settings-file doesn't exists (path: '{path}'), creating new instance of Replica.Services.Settings");
				return new Settings(debugger, true);
			}

			// Attempt to read and deserialize json-content into Services.Settings class
			var jsonContent = File.ReadAllText(path);
			var settings = JsonConvert.DeserializeObject<Replica.Services.Settings>(jsonContent);

			// ...
			if (settings == null) {

				// Submiting issue
				container.GetInstance<IIssueContainer>()
					.Add(new Models.Issue("json-settings-deserialization-exception"));

				// ...
				return new Settings(debugger, true);

			}

			// ...
			return settings;

		}



		public void Save(bool overwrite = false) {

			// ...
			if (!IsChanged && !overwrite) return;
			IsChanged = false;
			
			// ...
			_debugger.Trace();

			// ...
			var path = $"{AppDomain.CurrentDomain.BaseDirectory}/settings.json";
			var jsonContent = JsonConvert.SerializeObject(this, Formatting.Indented);
			
			// ...
			File.WriteAllText(path, jsonContent);

		}



	}



}
