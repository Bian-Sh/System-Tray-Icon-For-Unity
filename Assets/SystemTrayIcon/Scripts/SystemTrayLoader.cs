using Microsoft.Win32;
using System;
using System.Windows.Forms;
using UnityEngine;
using static Win32Helper;

/// <summary>
/// Systemtray menu & actions
/// </summary>
public class SystemTrayLoader : MonoBehaviour
{
    public SystemTray tray;
    public string exe;
    public string icon = "Icons\\icon_run.ico";

    public bool useApplicationIcon;
    public static SystemTrayLoader instance = null;
    void Awake()
    {
       exe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        icon = System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath ,icon);
        //singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    //..menuitems variables
    MenuItem[] weathers = new MenuItem[2];
    MenuItem displaySetup;
    public MenuItem startup;
    public MenuItem video;
    public MenuItem update;
    MenuItem gear_clock, circle_clock, simple_clock;
    MenuItem auto_ui_color, manual_ui_color;


    /// <summary>
    /// initialize traymenu, called after menucontroller script initilization.  Running on Main Unity Thread x_x..
    /// </summary>
    public void Start()
    {
        tray = CreateSystemTrayIcon();
        if (tray != null)
        {
            tray.SetTitle("rePaper");
            MenuItem weather = new MenuItem("Weather");

            weathers[0] = tray.trayMenu.MenuItems.Add("Clear", new EventHandler(Weather_Btn));
            weathers[1] = tray.trayMenu.MenuItems.Add("Thunder & Heavy Rain", new EventHandler(Weather_Btn));


            weather.MenuItems.AddRange(weathers);
            tray.trayMenu.MenuItems.Add(weather);
            tray.trayMenu.MenuItems.Add("-");

            MenuItem clockType = new MenuItem("Clock Style");
            gear_clock = tray.trayMenu.MenuItems.Add("Gear", Clock_Btn);
            circle_clock = tray.trayMenu.MenuItems.Add("Circle", new EventHandler(Clock_Btn));
            simple_clock = tray.trayMenu.MenuItems.Add("Simple", new EventHandler(Clock_Btn));
            clockType.MenuItems.Add(gear_clock);
            clockType.MenuItems.Add(circle_clock);
            clockType.MenuItems.Add(simple_clock);
            tray.trayMenu.MenuItems.Add(clockType);

            MenuItem uiColor = new MenuItem("UI Color");
            auto_ui_color = tray.trayMenu.MenuItems.Add("Auto", new EventHandler(UI_Btn));
            manual_ui_color = tray.trayMenu.MenuItems.Add("Pick One", new EventHandler(UI_Btn));
            uiColor.MenuItems.Add(auto_ui_color);
            uiColor.MenuItems.Add(manual_ui_color);
            tray.trayMenu.MenuItems.Add(uiColor);



            startup = new MenuItem("Run at Startup", new EventHandler(System_Startup_Btn));
            tray.trayMenu.MenuItems.Add(startup);
            tray.trayMenu.MenuItems.Add("-");

            MenuItem website = new MenuItem("Project Website", new EventHandler(WebpageBtn));
            tray.trayMenu.MenuItems.Add(website);



            MenuItem settings = new MenuItem("Settings", new EventHandler(Settings_Launcher));
            tray.trayMenu.MenuItems.Add(settings);
            tray.trayMenu.MenuItems.Add("-");

            MenuItem close = new MenuItem("Exit", new EventHandler(Close_Action));
            tray.trayMenu.MenuItems.Add(close);

            tray.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Settings_Launcher_Mouse);

            tray.ShowNotification("Hello..", "I'll just stay in systemtray, right click for more option...", 1000);


            startup.Checked = false;

            WeatherBtnCheckMark();
            ClockCheckMark();
            ColorCheckMark();

        }
    }

    private void Settings_Launcher_Mouse(object sender, MouseEventArgs e)
    {
        Debug.Log($"{nameof(SystemTrayLoader)}: tray.trayIcon.MouseDoubleClick ");
    }

    private void Weather_Btn(object sender, EventArgs e)
    {
        Debug.Log($"{nameof(SystemTrayLoader)}: {((sender as MenuItem).Text)}");
    }

    #region multimoniotr_menu

    private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
    {
        UpdateTrayMenuDisplay();
        MoveToDisplay(0);
    }

    //future use.
    void UpdateTrayMenuDisplay()
    {
        displaySetup.MenuItems.Clear();
        System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
        int i = 0;
        //displaySetup.MenuItems.Add("Span", new EventHandler(UserDisplayMenu));
        foreach (var item in screens)
        {
            displaySetup.MenuItems.Add("Display " + i.ToString(), new EventHandler(UserDisplayMenu));
            i++;
        }

        if (screens.Length > 1)
            displaySetup.Enabled = true;
        else
            displaySetup.Enabled = false;
    }

    private void UserDisplayMenu(object sender, EventArgs e)
    {
        int i = 0;
        string s = (sender as MenuItem).Text;
        /*
        if (s == "Span")
        {
            MoveToDisplay(-1);
            return;
        }
        */

        foreach (var item in System.Windows.Forms.Screen.AllScreens)
        {
            if (s == "Display " + i.ToString())
            {
                MoveToDisplay(i);
                break;
            }
            i++;
        }
    }

    void MoveToDisplay(int i)
    {
        Debug.Log($"{nameof(SystemTrayLoader)}: {i}");
    }

    #endregion
    //unity might be intercepting the messages or windows fked up, todo:- have to find a solution
    #region power_suspend_resume_UNUSED
    void OnPowerChange(System.Object sender, PowerModeChangedEventArgs e)
    {
        Debug.Log($"POWER CHANGE {e.Mode}");
    }
    #endregion power_suspend_resume

    /// <summary>
    /// Update traymenu color submenu checkmark
    /// </summary>
    public void ColorCheckMark()
    {
        if (UnityEngine.Application.isEditor == false)
        {
            if (auto_ui_color.Enabled == true)
                auto_ui_color.Enabled = false;

            auto_ui_color.Checked = false;
            manual_ui_color.Checked = true;

        }

    }

    /// <summary>
    /// traymenu color picker submenu action
    /// </summary>
    private void UI_Btn(System.Object sender, System.EventArgs e)
    {
        ColorCheckMark();
    }

    /// <summary>
    /// Update traymenu clocks submenu checkmark
    /// </summary>
    public void ClockCheckMark()
    {
        gear_clock.Checked = false;
        circle_clock.Checked = false;
        simple_clock.Checked = false;
    }

    /// <summary>
    /// Update traymenu weather submenu checkmark
    /// </summary>
    public void WeatherBtnCheckMark()
    {
        try
        {
            foreach (var item in weathers) //button text
            {
                item.Checked = false;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error weathrbtn checkmark" + e.Message);
        }

    }



    private void WebpageBtn(System.Object sender, System.EventArgs e)
    {
        System.Diagnostics.Process.Start("https://rocksdanister.github.io/rePaper");
    }

    private void KofiBtn(System.Object sender, System.EventArgs e)
    {
        System.Diagnostics.Process.Start("https://ko-fi.com/rocksdanister");
    }

    /// <summary>
    /// Multimonitor display utility launch.
    /// </summary>
    private void DisplayBtn(System.Object sender, System.EventArgs e)
    {

        Debug.Log("Controller script not found");
    }

    /// <summary>
    /// Clock type change traymenu
    /// </summary>
    private void Clock_Btn(System.Object sender, System.EventArgs e)
    {
        string s = (sender as MenuItem).Text;
        Debug.Log($"{nameof(SystemTrayLoader)}: {s}");
        ClockCheckMark();
    }

    /// <summary>
    /// Update traymenu, launches github page in browser.
    /// </summary>
    private void Update_Check(System.Object sender, System.EventArgs e)
    {
        System.Diagnostics.Process.Start("https://github.com/rocksdanister/rePaper");
    }

    /// <summary>
    /// Enable/Disable weather selection traymenu.
    /// </summary>
    /// <param name="val">Enable/Disable traymenu.</param>
    public void WeatherMenuEnable(bool val)
    {
        if (val == false)
        {
            foreach (var item in weathers)
            {
                item.Enabled = false;
            }
        }
        else
        {
            foreach (var item in weathers)
            {
                item.Enabled = true;
            }
        }
    }


    /// <summary>
    /// traymenu, launch configuration utility.
    /// </summary>
    private void Settings_Launcher(System.Object sender, System.EventArgs e)
    {
        Debug.Log($"{nameof(SystemTrayLoader)}:{nameof(Settings_Launcher)} ");
    }


    /// <summary>
    /// traymenu - Exit Application.
    /// </summary>
    /// <remarks>
    /// Disposes traymenu, stops dxva playback instance, refreshes desktop by calling setwallpaper, closes all open windows.
    /// </remarks>
    public void Close_Action(System.Object sender, System.EventArgs e)
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit(); //quits unity.
#endif
    }


    private void OnApplicationQuit()
    {
        tray?.Dispose();
    }



    /// <summary>
    /// traymenu run at startup button
    /// </summary>
    private void System_Startup_Btn(System.Object sender, System.EventArgs e)
    {
        runAtStartup = !runAtStartup;
        if (runAtStartup == true) //btn checkmark
            startup.Checked = true;
        else
            startup.Checked = false;
        SetStartup(runAtStartup);
    }
    private bool runAtStartup = false;

    public  SystemTray CreateSystemTrayIcon()
    {
        return new SystemTray(useApplicationIcon?exe:icon);
    }
}
