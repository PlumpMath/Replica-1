namespace Replica.Interfaces {

	using System;
	using System.ComponentModel;
	using Replica.Models;



	public interface IStorage : IDisposable {

		event CancelEventHandler LoadAllFilesProgress;

		string Identifier { get; }
		Instruction Instruction { get; }

		void Initialize(Instruction instruction);
		void SaveChanges();
		void CloseMetaFiles();

		void LoadMetaFiles();

		IMetaFile GetMetaFile(string path);
		IMetaFile CreateMetaFile(string path);
		void CloseMetaFile(string identifier, bool saveChanges = true);

	}



}
