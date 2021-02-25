using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using UnityEngine;
using System.Text;
using System.IO;
using System.Drawing.Imaging;

public class SystemTray : IDisposable
{
    public Icon ico_run;
    public NotifyIcon trayIcon;
    public System.Windows.Forms.ContextMenu trayMenu;

    private List<Action> actions = new List<Action>();

    public SystemTray(string iconPath)
    {
        trayMenu = new System.Windows.Forms.ContextMenu();
        trayIcon = new NotifyIcon();
        trayIcon.ContextMenu = trayMenu;
        SetTrayIcon(iconPath);
    }

    public void SetTrayIcon(string resPath)
    {
        if (resPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            var icon =Icon.ExtractAssociatedIcon(resPath);//有几率获取不到？
            
            Debug.Log($"{nameof(SystemTray)}: use exe icon {icon is null}");
            var bmp = icon.ToBitmap();
            bmp.Save(@"E:\Unity\Project\System Tray Icon For Unity\Build\11.bmp", ImageFormat.Bmp );
            trayIcon.Icon = icon;
        }
        else
        {
            Debug.Log($"{nameof(SystemTray)}: use custom icon");
            var strB = new StringBuilder(resPath);
            var handle = StaticPinvoke.ExtractAssociatedIcon(IntPtr.Zero, strB, out _);
            trayIcon.Icon = Icon.FromHandle(handle);
            strB.Clear();
        }

    }
    public void AddItem(string label, Action function)
    {
        actions.Add(function);
        trayMenu.MenuItems.Add(label, OnAdd);
    }

    /// <summary>
    /// The ToolTip text displayed when the mouse pointer rests on a notification area icon.
    /// </summary>
    /// <param name="title">title text.</param>
    public void SetTitle(string title)
    {
        trayIcon.Text = title;
    }

    public void AddSeparator()
    {
        trayMenu.MenuItems.Add("-");
    }
    /// <summary>
    /// Displays native windows notification.
    /// </summary>
    public void ShowNotification(string title, string text, int duration = 5000)
    {
        trayIcon.Visible = true;
        trayIcon.BalloonTipTitle = title;
        trayIcon.BalloonTipText = text;
        trayIcon.BalloonTipIcon = ToolTipIcon.Info;
        trayIcon.ShowBalloonTip(duration);
    }

    private void OnAdd(object sender, EventArgs e)
    {

        Action ac = actions[((MenuItem)sender).Index];
        if (ac != null)
            ac();
        else
            Debug.Log("Error adding traymenu item");

    }

    /// <summary>
    /// Removes icon from tray, memory cleanup.
    /// </summary>
    public void Dispose()
    {
        trayIcon.Visible = false;
        trayIcon.Icon.Dispose();
        trayMenu.Dispose();
        trayIcon.Dispose();
    }
}
