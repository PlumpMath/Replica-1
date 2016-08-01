namespace Replica.Models {

	using System.Collections.Generic;



	public sealed class Instruction {

		public string Identifier { get; set; }
		public string DeviceSerialNumber { get; set; }
		public bool IsActive { get; set; }

		public string Title { get; set; }
		public string RootDirectory { get; set; }
		public List<string> Directives { get; set; }

	}

}
