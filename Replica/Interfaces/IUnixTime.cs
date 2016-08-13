namespace Replica.Interfaces {

	public interface IUnixTime {

		int Now { get; }
		int Difference(int value);

	}

}
