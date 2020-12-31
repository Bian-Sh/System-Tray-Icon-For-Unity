using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using UnityEngine;
using System.Text;

public class SystemTray : IDisposable
{
    public Icon ico_run;
    public NotifyIcon trayIcon;
    public System.Windows.Forms.ContextMenu trayMenu;

    private List<Action> actions = new List<Action>();

    public SystemTray()
    {
        trayMenu = new System.Windows.Forms.ContextMenu();
        trayIcon = new NotifyIcon();
        //trayIcon.Text = UnityEngine.Application.productName;
        ushort uicon;
        StringBuilder strB = new StringBuilder(UnityEngine.Application.streamingAssetsPath + "\\Icons\\icon_run.ico");
        IntPtr handle = StaticPinvoke.ExtractAssociatedIcon(IntPtr.Zero, strB, out uicon);
        ico_run = Icon.FromHandle(handle);
        trayIcon.Icon = ico_run;
        strB.Clear();
        trayIcon.ContextMenu = trayMenu;
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
    public void ShowNotification(int duration, string title, string text)
    {
        trayIcon.Visible = true;
        trayIcon.BalloonTipTitle = title;
        trayIcon.BalloonTipText = text;
        trayIcon.BalloonTipIcon = ToolTipIcon.Info;
        trayIcon.ShowBalloonTip(5000);
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
