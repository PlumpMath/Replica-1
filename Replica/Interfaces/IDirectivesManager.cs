namespace Replica.Interfaces {

	using Replica.Models;



	public interface IDirectivesManager {

		void Initialize(Instruction instruction);

		bool Match(string path);

		void Add(string directive);
		void Remove(string directive);

	}

}
