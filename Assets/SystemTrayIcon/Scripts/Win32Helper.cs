using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using static StaticPinvoke;
using Debug = UnityEngine.Debug;

public static class Win32Helper
{
    static Win32Helper()
    {
        //软件启动就获取主窗口句柄
        CurrentWindowHandle = GetForegroundWindow();
    }
    public static IntPtr CurrentWindowHandle { get; }

    /// <summary>
    /// 隐藏任务栏图标
    /// </summary>
    public static void HideTaskBar()
    {
        try
        {
            ShowWindow(CurrentWindowHandle, (uint)ShowWindowCommands.SW_HIDE);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    /// <summary>
    /// 显示任务栏图标
    /// </summary>
    public static void ShowTaskBar()
    {
        try
        {
            ShowWindow(CurrentWindowHandle, (uint)ShowWindowCommands.SW_RESTORE);
            SetForegroundWindow(CurrentWindowHandle);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 添加系统启动项
    /// </summary>
    /// <remarks>
    /// If disabled in taskmanager, entry gets added but key value will remain disabled.
    /// </remarks>
    /// <param name="val">true: set entry, false: delete entry.</param>
    public static void SetStartup(bool val)
    {
        //create shortcut first, overwrite if exist with new path.
        CreateShortcut();
        RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (val)
        {
            rk.SetValue(UnityEngine.Application.productName, System.AppDomain.CurrentDomain.BaseDirectory + "\\rePaperStartup.lnk");
        }
        else
        {
            rk.DeleteValue(UnityEngine.Application.productName, false);
        }
        rk.Close();
    }

    /// <summary>
    /// Creates application shortcut to link to windows startup in registry.
    /// </summary>
    private static void CreateShortcut()
    {
        try
        {
            WshShell shell = new WshShell();
            var shortCutLinkFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\rePaperStartup.lnk";
            var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(shortCutLinkFilePath);
            windowsApplicationShortcut.Description = "shortcut of rePaper Live Wallpaper";
            windowsApplicationShortcut.WorkingDirectory = System.IO.Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).ToString();
            windowsApplicationShortcut.TargetPath = System.IO.Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).ToString() + "\\Start.exe";
            windowsApplicationShortcut.Save();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}
