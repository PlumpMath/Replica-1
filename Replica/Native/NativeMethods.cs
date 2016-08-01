namespace Replica.Native {

	using System;
	using System.Runtime.InteropServices;



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



	}



}
