namespace Replica.Interfaces {

	using System;



	public interface ICron {

		int Register(EventHandler eventHandler, int interval);
		void Unregister(int hashCode);

	}

}