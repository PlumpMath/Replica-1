namespace Replica.Native {

	using System;
	using System.Runtime.InteropServices;
	using System.Text;



	internal static class NativeMethods {



		/// <summary>
		/// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
		/// </summary>
		/// <param name="hWnd">A handle to the window.</param>
		/// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order. This parameter must be a window handle or one of the following values: <para>HWND_BOTTOM: Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.</para><para>HWND_NOTOPMOST: Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.</para><para>HWND_TOP: Places the window at the top of the Z order.</para><para>HWND_TOPMOST: Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.</para></param>
		/// <param name="x">The new position of the left side of the window, in client coordinates.</param>
		/// <param name="y">The new position of the top of the window, in client coordinates.</param>
		/// <param name="cx">The new width of the window, in pixels.</param>
		/// <param name="cy">The new height of the window, in pixels.</param>
		/// <param name="uFlags">The window sizing and positioning flags. This parameter can be a combination of the values specified in SetWindowPosFlags enumerator.</param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, WindowPosFlags uFlags);



		/// <summary>
		/// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs..</param>
		/// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the following values: GWL_EXSTYLE, GWL_HINSTANCE, GWL_ID, GWL_STYLE, GWL_USERDATA, GWL_WNDPROC </param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer. 
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError. </returns>
		[DllImport("user32.dll")]
		internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);



		/// <summary>Retrieves information about the file system and volume associated with the specified root directory.</summary>
		/// <param name="rootPathName">A pointer to a string that contains the root directory of the volume to be described. If this parameter is NULL, the root of the current directory is used. A trailing backslash is required. For example, you specify \\MyServer\MyShare as "\\MyServer\MyShare\", or the C drive as "C:\".</param>
		/// <param name="volumeNameBuffer">A pointer to a buffer that receives the name of a specified volume. The buffer size is specified by the nVolumeNameSize parameter.</param>
		/// <param name="volumeNameSize">The length of a volume name buffer, in TCHARs. The maximum buffer size is MAX_PATH+1. This parameter is ignored if the volume name buffer is not supplied.</param>
		/// <param name="volumeSerialNumber">A pointer to a variable that receives the volume serial number. This parameter can be NULL if the serial number is not required. This function returns the volume serial number that the operating system assigns when a hard disk is formatted. To programmatically obtain the hard disk's serial number that the manufacturer assigns, use the Windows Management Instrumentation (WMI) Win32_PhysicalMedia property SerialNumber.</param>
		/// <param name="maximumComponentLength">A pointer to a variable that receives the maximum length, in TCHARs, of a file name component that a specified file system supports. A file name component is the portion of a file name between backslashes. The value that is stored in the variable that *lpMaximumComponentLength points to is used to indicate that a specified file system supports long names. For example, for a FAT file system that supports long names, the function stores the value 255, rather than the previous 8.3 indicator. Long names can also be supported on systems that use the NTFS file system.</param>
		/// <param name="fileSystemFlags">A pointer to a variable that receives flags associated with the specified file system. This parameter can be one or more of the following flags. However, FILE_FILE_COMPRESSION and FILE_VOL_IS_COMPRESSED are mutually exclusive.</param>
		/// <param name="fileSystemNameBuffer">A pointer to a buffer that receives the name of the file system, for example, the FAT file system or the NTFS file system. The buffer size is specified by the nFileSystemNameSize parameter.</param>
		/// <param name="nFileSystemNameSize">The length of the file system name buffer, in TCHARs. The maximum buffer size is MAX_PATH+1. This parameter is ignored if the file system name buffer is not supplied.</param>
		/// <returns>If all the requested information is retrieved, the return value is nonzero. If not all the requested information is retrieved, the return value is zero.To get extended error information, call GetLastError.</returns>
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetVolumeInformation(string rootPathName, StringBuilder volumeNameBuffer, int volumeNameSize, out uint volumeSerialNumber, out uint maximumComponentLength, out FileSystemFeature fileSystemFlags, StringBuilder fileSystemNameBuffer, int nFileSystemNameSize);



		/// <summary>Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe. The function returns a handle that can be used to access the file or device for various types of I/O depending on the file or device and the flags and attributes specified. To perform this operation as a transacted operation, which results in a handle that can be used for transacted I/O, use the CreateFileTransacted function.</summary>
		/// <param name="lpFileName">The name of the file or device to be created or opened. You may use either forward slashes (/) or backslashes (\) in this name. In the ANSI version of this function, the name is limited to MAX_PATH characters. To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path. For more information, see Naming Files, Paths, and Namespaces. For information on special device names, see Defining an MS-DOS Device Name. To create a file stream, specify the name of the file, a colon, and then the name of the stream. For more information, see File Streams.</param>
		/// <param name="dwDesiredAccess">The requested access to the file or device, which can be summarized as read, write, both or neither zero). The most commonly used values are GENERIC_READ, GENERIC_WRITE, or both (GENERIC_READ | GENERIC_WRITE). For more information, see Generic Access Rights, File Security and Access Rights, File Access Rights Constants, and ACCESS_MASK. If this parameter is zero, the application can query certain metadata such as file, directory, or device attributes without accessing that file or device, even if GENERIC_READ access would have been denied. You cannot request an access mode that conflicts with the sharing mode that is specified by the dwShareMode parameter in an open request that already has an open handle. For more information, see the Remarks section of this topic and Creating and Opening Files.</param>
		/// <param name="dwShareMode">The requested sharing mode of the file or device, which can be read, write, both, delete, all of these, or none (refer to the following table). Access requests to attributes or extended attributes are not affected by this flag. If this parameter is zero and CreateFile succeeds, the file or device cannot be shared and cannot be opened again until the handle to the file or device is closed. For more information, see the Remarks section. You cannot request a sharing mode that conflicts with the access mode that is specified in an existing request that has an open handle. CreateFile would fail and the GetLastError function would return ERROR_SHARING_VIOLATION. To enable a process to share a file or device while another process has the file or device open, use a compatible combination of one or more of the following values. For more information about valid combinations of this parameter with the dwDesiredAccess parameter, see Creating and Opening Files. Note: The sharing options for each open handle remain in effect until that handle is closed, regardless of process context.</param>
		/// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members: an optional security descriptor, and a Boolean value that determines whether the returned handle can be inherited by child processes. This parameter can be NULL. If this parameter is NULL, the handle returned by CreateFile cannot be inherited by any child processes the application may create and the file or device associated with the returned handle gets a default security descriptor. The lpSecurityDescriptor member of the structure specifies a SECURITY_DESCRIPTOR for a file or device. If this member is NULL, the file or device associated with the returned handle is assigned a default security descriptor. CreateFile ignores the lpSecurityDescriptor member when opening an existing file or device, but continues to use the bInheritHandle member. The bInheritHandlemember of the structure specifies whether the returned handle can be inherited. For more information, see the Remarks section.</param>
		/// <param name="dwCreationDisposition">An action to take on a file or device that exists or does not exist. For devices other than files, this parameter is usually set to OPEN_EXISTING. For more information, see the Remarks section. This parameter must be one of the following values, which cannot be combined.</param>
		/// <param name="dwFlagsAndAttributes">The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the most common default value for files. This parameter can include any combination of the available file attributes (FILE_ATTRIBUTE_*). All other file attributes override FILE_ATTRIBUTE_NORMAL. This parameter can also contain combinations of flags (FILE_FLAG_*) for control of file or device caching behavior, access modes, and other special-purpose flags. These combine with any FILE_ATTRIBUTE_* values. This parameter can also contain Security Quality of Service (SQOS) information by specifying the SECURITY_SQOS_PRESENT flag. Additional SQOS-related flags information is presented in the table following the attributes and flags tables. Note  When CreateFile opens an existing file, it generally combines the file flags with the file attributes of the existing file, and ignores any file attributes supplied as part of dwFlagsAndAttributes. Special cases are detailed in Creating and Opening Files. Some of the following file attributes and flags may only apply to files and not necessarily all other types of devices that CreateFile can open. For additional information, see the Remarks section of this topic and Creating and Opening Files. For more advanced access to file attributes, see SetFileAttributes. For a complete list of all file attributes with their values and descriptions, see File Attribute Constants. The dwFlagsAndAttributesparameter can also specify SQOS information. For more information, see Impersonation Levels. When the calling application specifies the SECURITY_SQOS_PRESENT flag as part of dwFlagsAndAttributes, it can also contain one or more of the following values.</param>
		/// <param name="hTemplateFile">A valid handle to a template file with the GENERIC_READ access right. The template file supplies file attributes and extended attributes for the file that is being created. This parameter can be NULL. When opening an existing file, CreateFile ignores this parameter. When opening a new encrypted file, the file inherits the discretionary access control list from its parent directory. For additional information, see File Encryption.</param>
		/// <returns>If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot. If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, int hTemplateFile);



		/// <summary>Registers the device or type of device for which a window will receive notifications.</summary>
		/// <param name="hRecipient">A handle to the window or service that will receive device events for the devices specified in the NotificationFilter parameter. The same window handle can be used in multiple calls to RegisterDeviceNotification. Services can specify either a window handle or service status handle.</param>
		/// <param name="notificationFilter">A pointer to a block of data that specifies the type of device for which notifications should be sent. This block always begins with the DEV_BROADCAST_HDR structure. The data following this header is dependent on the value of the dbch_devicetype member, which can be DBT_DEVTYP_DEVICEINTERFACE or DBT_DEVTYP_HANDLE. For more information, see Remarks.</param>
		/// <param name="flags">This parameter can be one of the values specified in <see cref="DeviceNotifyFlags"/>.</param>
		/// <returns>If the function succeeds, the return value is a device notification handle. If the function fails, the return value is NULL. To get extended error information, call GetLastError.</returns>
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr notificationFilter, uint flags);



		/// <summary>Closes the specified device notification handle.</summary>
		/// <param name="hHandle">Device notification handle returned by the RegisterDeviceNotification function.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern bool UnregisterDeviceNotification(IntPtr hHandle);



		/// <summary>Closes an open object handle.</summary>
		/// <param name="hObject">A valid handle to an open object.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError. If the application is running under a debugger, the function will throw an exception if it receives either a handle value that is not valid or a pseudo-handle value. This can happen if you close a handle twice, or if you call CloseHandle on a handle returned by the FindFirstFile function instead of calling the FindClose function.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr hObject);



	}



}
