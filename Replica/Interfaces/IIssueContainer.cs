namespace Replica.Interfaces {

	public interface IIssueContainer {

		int Count { get; }

		void Add(IIssue issue);
		void Remove(string code);

		IIssue Get(string code);

	}

}
