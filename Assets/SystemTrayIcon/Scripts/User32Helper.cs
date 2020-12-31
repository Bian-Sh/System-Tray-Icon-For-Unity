﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using static StaticPinvoke;
using Debug = UnityEngine.Debug;

public static class User32Helper
{
    private static Hashtable processWnd = null;
    private const int SW_HIDE = 0;  //hied task bar
 
    private const int SW_RESTORE = 9;//show task bar
    /// <summary>
    /// 托盘图标
    /// </summary>
    private static System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
 
    private static int _width = 100, _height = 100;
    private static System.Windows.Forms.ContextMenu contextMenu1;
    private static System.Windows.Forms.MenuItem menuItem1;
    private static System.ComponentModel.IContainer components;
    private static IntPtr window = IntPtr.Zero;
    
    
    public static IntPtr CurrentWindowHandle
    {
        get
        {
            if (window == IntPtr.Zero)
            {
                window = GetCurrentWindowHandle();
            }

            return window;
        }
    }

    #region 构造函数

    static User32Helper()
    {
        if (processWnd == null)
        {
            processWnd = new Hashtable();
        }

        MakeTrayIcon();
    }

    #endregion
    
    
    #region 按键
    public static void KeyDown(Key key)
    {
        keybd_event((byte) key, 0, 0, 0);
    }
    public static void KeyUp(Key key)
    {
        keybd_event((byte) key, 0, 2, 0);
    }

    public static async Task Click(Key key)
    {
        KeyDown(key);
        await Task.Delay(100);
        KeyUp(key);
    }
    #endregion
    
    /// <summary>
    /// 添加右键菜单
    /// </summary>
    public static void MakeTrayIcon()
    {
        Debug.Log($"{nameof(User32Helper)}: haha");
        components = new System.ComponentModel.Container();
        contextMenu1 = new System.Windows.Forms.ContextMenu();
        menuItem1 = new System.Windows.Forms.MenuItem();
 
        contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItem1});
 
        menuItem1.Index = 0;
        menuItem1.Text = "退出";
        menuItem1.Click += new System.EventHandler(QuitApplication);
        
        // _notifyIcon = new System.Windows.Forms.NotifyIcon(components);
 
        //Note This is a System.Drawing.Icon type, google if you get stuck
        // _notifyIcon.Icon = new Icon("appicon.ico");
 
        notifyIcon.ContextMenu = contextMenu1;
 
        // _notifyIcon.Text = "Unity Tray Icon Test";
        // _notifyIcon.Visible = true;
    }

    

    /// <summary>
    /// 退出程序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void QuitApplication(object sender, EventArgs e)
    {
        Application.Quit();
    }

    /// <summary>
    /// 获取当前窗口句柄
    /// </summary>
    /// <returns></returns>
    public static IntPtr GetCurrentWindowHandle()
    {
        IntPtr ptrWnd = IntPtr.Zero;
        uint uiPid = (uint)Process.GetCurrentProcess().Id;  // 当前进程 ID
        object objWnd = processWnd[uiPid];

        if (objWnd != null)
        {
            ptrWnd = (IntPtr)objWnd;
            if (ptrWnd != IntPtr.Zero && IsWindow(ptrWnd))  // 从缓存中获取句柄
            {
                return ptrWnd;
            }
            else
            {
                ptrWnd = IntPtr.Zero;
            }
        }

        bool bResult = EnumWindows(new WNDENUMPROC(EnumWindowsProc), uiPid);
        // 枚举窗口返回 false 并且没有错误号时表明获取成功
        if (!bResult && Marshal.GetLastWin32Error() == 0)
        {
            objWnd = processWnd[uiPid];
            if (objWnd != null)
            {
                ptrWnd = (IntPtr)objWnd;
            }
        }

        return ptrWnd;
    }

    /// <summary>
    /// 缓存遍历过的进程
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    private static bool EnumWindowsProc(IntPtr hwnd, uint lParam)
    {
        uint uiPid = 0;

        if (GetParent(hwnd) == IntPtr.Zero)
        {
            GetWindowThreadProcessId(hwnd, ref uiPid);
            if (uiPid == lParam)    // 找到进程对应的主窗口句柄
            {
                processWnd[uiPid] = hwnd;   // 把句柄缓存起来
                SetLastError(0);    // 设置无错误
                return false;   // 返回 false 以终止枚举窗口
            }
        }

        return true;
    }
    /// <summary>
    /// 最小化到托盘
    /// </summary>
    public static void HideTaskBar()
    {
        try
        {
            //window = GetForegroundWindow();
 
            ShowWindow(CurrentWindowHandle, SW_HIDE);
 
            notifyIcon.BalloonTipText = $"{Application.productName} {Application.version}";//托盘气泡显示内容
 
            notifyIcon.Text = Application.productName;//鼠标悬浮时显示的内容
 
            notifyIcon.Visible = true;//托盘按钮是否可见
 
            notifyIcon.Icon = CustomTrayIcon(Application.streamingAssetsPath + "/icon.png", _width, _height);//托盘图标
 
            notifyIcon.ShowBalloonTip(2000);//托盘气泡显示时间
 
            notifyIcon.MouseClick += notifyIcon_MouseClick;//双击托盘图标响应事件
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    /// <summary>
    /// 点击托盘图标
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)//
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            notifyIcon.MouseDoubleClick -= notifyIcon_MouseClick;
 
            notifyIcon.Visible = false;
 
            ShowWindow(CurrentWindowHandle, SW_RESTORE);
            SetForegroundWindow(CurrentWindowHandle);
        }
    }

    /// <summary>
    /// 托盘图标
    /// </summary>
    /// <param name="iconPath"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    private static System.Drawing.Icon CustomTrayIcon(string iconPath, int width, int height)
    {
        System.Drawing.Bitmap bt = new System.Drawing.Bitmap(iconPath);
 
        System.Drawing.Bitmap fitSizeBt = new System.Drawing.Bitmap(bt, width, height);
 
        return System.Drawing.Icon.FromHandle(fitSizeBt.GetHicon());
    }

}
/// <summary>
/// 窗口状态
/// </summary>
public struct WINDOWPLACEMENT
{
    public int length;
    public int flags;
    public int showCmd;
    public System.Drawing.Point ptMinPosition;
    public System.Drawing.Point ptMaxPosition;
    public System.Drawing.Rectangle rcNormalPosition;
}


public enum Key
{
    Backspace = 8,
    Tab = 9,
    Clear = 12,
    Enter = 13,
    Shift = 16,
    Control = 17,
    Alt = 18,
    CapsLock = 20,
    Esc = 27,
    Space = 32,
    PageUp = 33,
    PageDown = 34,
    End = 35,
    Home = 36,
    LeftArrow = 37,
    UpArrow = 38,
    RightArrow = 39,
    DownArrow = 40,
    Insert = 45,
    Delete = 46,
    Help = 47,
    A = 65,
    B = 66,
    C = 67,
    D = 68,
    E = 69,
    F = 70,
    G = 71,
    H = 72,
    I = 73,
    J = 74,
    K = 75,
    L = 76,
    M = 77,
    N = 78,
    O = 79,
    P = 80,
    Q = 81,
    R = 82,
    S = 83,
    T = 84,
    U = 85,
    V = 86,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,
    Alpha0 = 48,
    Alpha1 = 49,
    Alpha2 = 50,
    Alpha3 = 51,
    Alpha4 = 52,
    Alpha5 = 53,
    Alpha6 = 54,
    Alpha7 = 55,
    Alpha8 = 56,
    Alpha9 = 57,
    Keypad0 = 96,
    Keypad1 = 97,
    Keypad2 = 98,
    Keypad3 = 99,
    Keypad4 = 100,
    Keypad5 = 101,
    Keypad6 = 102,
    Keypad7 = 103,
    Keypad8 = 104,
    Keypad9 = 105,
    Multiply = 106,
    Addition = 107,
    NumPadEnter = 108,
    Subtraction = 109,
    Dot = 110,
    Division = 111,
    F1 = 112,
    F2 = 113,
    F3 = 114,
    F4 = 115,
    F5 = 116,
    F6 = 117,
    F7 = 118,
    F8 = 119,
    F9 = 120,
    F10 = 121,
    F11 = 122,
    F12 = 123,
    NumLock = 144
}