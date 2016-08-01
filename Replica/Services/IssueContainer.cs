namespace Replica.Services {

	using System.Collections.Generic;
	using Replica.Interfaces;



	public sealed class IssueContainer : IIssueContainer {

		private List<IIssue> _collection;
		private readonly IDebugger _debugger;

		public int Count => _collection?.Count ?? 0;


		public IssueContainer(IDebugger debugger) {

			// ...
			(_debugger = debugger).Trace();
			_collection = new List<IIssue>();

		}



		public void Add(IIssue issue) {

			// ...
			_debugger.Trace(issue.Code);
			_collection.Add(issue);

		}



		public void Remove(string code) {

			// ...
			_debugger.Trace(code);

			// Removing every issue from collection, that has specified code
			for (var i = _collection.Count - 1; i >= 0; i--) {
				if (_collection[i].Code == code) _collection.RemoveAt(i);
			}

		}



		public IIssue Get(string code) {

			// ...
			_debugger.Trace(code);

			// Searching for issue with specified code
			for (var i = _collection.Count - 1; i >= 0; i--) {
				if (_collection[i].Code == code) return _collection[i];
			}

			// ...
			return null;

		}



	}



}
