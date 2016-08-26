namespace Replica.Services {

	using Replica.Models;
	using Replica.Interfaces;



	public sealed class DirectivesManager : IDirectivesManager {

		private Instruction _instruction;



		public void Initialize(Instruction instruction) {
			_instruction = instruction;
		}



		public bool Match(string path) {

			foreach (var directive in _instruction.Directives) {
				if (Match(directive, path)) return true;
			}

			return false;

		}



		public void Add(string directive) {

			foreach (var item in _instruction.Directives) {
				if (item == directive) return;
			}

			_instruction.Directives.Add(directive);

		}



		public void Remove(string directive) {
			_instruction.Directives.Remove(directive);
		}



		private static bool Match(string pattern, string input) {

			if (string.CompareOrdinal(pattern, input) == 0) {
				return true;

			} else if (string.IsNullOrEmpty(input)) {
				return string.IsNullOrEmpty(pattern.Trim('*'));

			} else if (pattern.Length == 0) {
				return false;

			} else if (pattern[0] == '?') {
				return Match(pattern.Substring(1), input.Substring(1));

			} else if (pattern[pattern.Length - 1] == '?') {
				return Match(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1));

			} else if (pattern[0] == '*') {
				return Match(pattern.Substring(1), input)
					|| Match(pattern, input.Substring(1));

			} else if (pattern[pattern.Length - 1] == '*') {
				return Match(pattern.Substring(0, pattern.Length - 1), input)
					|| Match(pattern, input.Substring(0, input.Length - 1));

			} else if (pattern[0] == input[0]) {
				return Match(pattern.Substring(1), input.Substring(1));

			}

			return false;

		}



	}

}
