/// <summary>
/// https://www.pinvoke.net/default.aspx/Enums/SHOWWINDOW_FLAGS.html
/// </summary>
public enum ShowWindowCommands : uint
{
    /// <summary>
    ///        Hides the window and activates another window.
    /// </summary>
    SW_HIDE = 0,

    /// <summary>
    ///        Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
    /// </summary>
    SW_SHOWNORMAL = 1,

    /// <summary>
    ///        Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
    /// </summary>
    SW_NORMAL = 1,

    /// <summary>
    ///        Activates the window and displays it as a minimized window.
    /// </summary>
    SW_SHOWMINIMIZED = 2,

    /// <summary>
    ///        Activates the window and displays it as a maximized window.
    /// </summary>
    SW_SHOWMAXIMIZED = 3,

    /// <summary>
    ///        Maximizes the specified window.
    /// </summary>
    SW_MAXIMIZE = 3,

    /// <summary>
    ///        Displays a window in its most recent size and position. This value is similar to <see cref="ShowWindowCommands.SW_SHOWNORMAL"/>, except the window is not activated.
    /// </summary>
    SW_SHOWNOACTIVATE = 4,

    /// <summary>
    ///        Activates the window and displays it in its current size and position.
    /// </summary>
    SW_SHOW = 5,

    /// <summary>
    ///        Minimizes the specified window and activates the next top-level window in the z-order.
    /// </summary>
    SW_MINIMIZE = 6,

    /// <summary>
    ///        Displays the window as a minimized window. This value is similar to <see cref="ShowWindowCommands.SW_SHOWMINIMIZED"/>, except the window is not activated.
    /// </summary>
    SW_SHOWMINNOACTIVE = 7,

    /// <summary>
    ///        Displays the window in its current size and position. This value is similar to <see cref="ShowWindowCommands.SW_SHOW"/>, except the window is not activated.
    /// </summary>
    SW_SHOWNA = 8,

    /// <summary>
    ///        Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
    /// </summary>
    SW_RESTORE = 9,

    /// <summary>
    ///        Items 10, 11 and 11 existed in the VB definition but not the c# definition - so I am assuming this was a mistake and have added them here.
    ///         Please forgive me if this is wrong!  I don't think it should have any negative impact.
    ///         According to what I have read elsewhere: The SW_SHOWDEFAULT makes sure the window is restored prior to showing, then activating.
    ///         And the 11's try to coerce a window to minimized or maximized.
    /// </summary>
    SW_SHOWDEFAULT = 10,
    SW_FORCEMINIMIZE = 11,
    SW_MAX = 11

}