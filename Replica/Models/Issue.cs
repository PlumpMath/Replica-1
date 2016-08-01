namespace Replica.Models {

	using Replica.Interfaces;



	public sealed class Issue : IIssue {

		public string Code { get; }



		public Issue(string code) {
			Code = code;
		}

	}

}
