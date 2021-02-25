using System.Runtime.InteropServices;
using System;
using System.Text;

/// <summary>
/// Native C++ windows calls.
/// reference: https://www.pinvoke.net
/// </summary>
public class StaticPinvoke
{
    /// <summary>
    ///  显示窗口 
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="nCmdShow"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);


    [DllImport("shell32.dll")]
    public static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, StringBuilder lpIconPath, out ushort lpiIcon);

    /// <summary>
    /// This function retrieves the visibility state of the specified window.
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns></returns>
    [DllImport("USER32.DLL")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    #region DllImport

    /// <summary>
    /// This function returns the handle to the foreground window — the window with which the user is currently working.
    /// </summary>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    /// <summary>
    /// This function puts the thread that created the specified window into the foreground and activates the window.
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns></returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);



    #endregion
}
