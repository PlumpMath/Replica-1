namespace Replica.Native {

	internal enum FileAttributes : uint {

		/// <summary>A file or directory that is an archive file or directory. Applications typically use this attribute to mark files for backup or removal . </summary>
		ARCHIVE = 32,// (0x20)

		/// <summary>A file or directory that is compressed. For a file, all of the data in the file is compressed. For a directory, compression is the default for newly created files and subdirectories.</summary>
		COMPRESSED = 2048,// (0x800)

		/// <summary>This value is reserved for system use.</summary>
		DEVICE = 64,// (0x40)

		/// <summary>The handle that identifies a directory.</summary>
		DIRECTORY = 16,// (0x10)

		/// <summary>A file or directory that is encrypted. For a file, all data streams in the file are encrypted. For a directory, encryption is the default for newly created files and subdirectories.</summary>
		ENCRYPTED = 16384,// (0x4000)

		/// <summary>The file or directory is hidden. It is not included in an ordinary directory listing.</summary>
		HIDDEN = 2,// (0x2)

		/// <summary>The directory or user data stream is configured with integrity (only supported on ReFS volumes). It is not included in an ordinary directory listing. The integrity setting persists with the file if it's renamed. If a file is copied the destination file will have integrity set if either the source file or destination directory have integrity set. Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This flag is not supported until Windows Server 2012.</summary>
		INTEGRITY_STREAM = 32768,// (0x8000)

		/// <summary>A file that does not have other attributes set. This attribute is valid only when used alone.</summary>
		NORMAL = 128,// (0x80)

		/// <summary>The file or directory is not to be indexed by the content indexing service.</summary>
		NOT_CONTENT_INDEXED = 8192,// (0x2000)

		/// <summary>The user data stream not to be read by the background data integrity scanner (AKA scrubber). When set on a directory it only provides inheritance. This flag is only supported on Storage Spaces and ReFS volumes. It is not included in an ordinary directory listing. Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This flag is not supported until Windows 8 and Windows Server 2012.</summary>
		NO_SCRUB_DATA = 131072,// (0x20000)

		/// <summary>The data of a file is not available immediately. This attribute indicates that the file data is physically moved to offline storage. This attribute is used by Remote Storage, which is the hierarchical storage management software. Applications should not arbitrarily change this attribute.</summary>
		OFFLINE = 4096,// (0x1000)

		/// <summary>A file that is read-only. Applications can read the file, but cannot write to it or delete it. This attribute is not honored on directories. For more information, see You cannot view or change the Read-only or the System attributes of folders in Windows Server 2003, in Windows XP, in Windows Vista or in Windows 7.</summary>
		READONLY = 1,// (0x1)

		/// <summary>A file or directory that has an associated reparse point, or a file that is a symbolic link.</summary>
		REPARSE_POINT = 1024,// (0x400)

		/// <summary>A file that is a sparse file.</summary>
		SPARSE_FILE = 512,// (0x200)

		/// <summary>A file or directory that the operating system uses a part of, or uses exclusively.</summary>
		SYSTEM = 4,// (0x4)

		/// <summary>A file that is being used for temporary storage. File systems avoid writing data back to mass storage if sufficient cache memory is available, because typically, an application deletes a temporary file after the handle is closed. In that scenario, the system can entirely avoid writing the data. Otherwise, the data is written after the handle is closed.</summary>
		TEMPORARY = 256,// (0x100)

		/// <summary>This value is reserved for system use.</summary>
		VIRTUAL = 65536,// (0x10000)

	}

}
