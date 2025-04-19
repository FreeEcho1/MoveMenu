namespace MoveMenu;

public class PluginProcessing : IPlugin
{
    /// <summary>
    /// Disposed
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// プラグインの名前
    /// </summary>
    public string PluginName { get; } = "MoveMenu";
    /// <summary>
    /// ウィンドウが存在するかの値
    /// </summary>
    public bool IsWindowExist { get; } = true;
    /// <summary>
    /// ウィンドウハンドルがウィンドウの場合のみイベント処理 (処理しない「false」/処理する「true」)
    /// </summary>
    public bool IsWindowOnlyEventProcessing { get; } = true;
    /// <summary>
    /// 取得するウィンドウイベントの種類 (なし「0」)
    /// </summary>
    public GetWindowEventType GetWindowEventType
    {
        get
        {
            return 0;
        }
    }
    /// <summary>
    /// 取得するウィンドウイベントの種類の変更イベントのデータ
    /// </summary>
    public ChangeGetWindowEventTypeData ChangeGetWindowEventTypeData
    {
        get;
    } = new();
    /// <summary>
    /// イベント処理のデータ
    /// </summary>
    public EventProcessingData EventProcessingData
    {
        get;
    } = new();

    /// <summary>
    /// 設定ウィンドウ
    /// </summary>
    private SettingsWindow? SettingsWindow;

    /// <summary>
    /// イベントフックインスタンス
    /// </summary>
    private static IntPtr Hook = IntPtr.Zero;
    /// <summary>
    /// フック用のスレッド
    /// </summary>
    private readonly Thread HookThread = new(new ThreadStart(() =>
    {
    }))
    {
        IsBackground = true
    };

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PluginProcessing()
    {
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~PluginProcessing()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing">disposing</param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed)
        {
            return;
        }
        if (disposing)
        {
            Destruction();
        }
        Disposed = true;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="settingDirectory">設定のディレクトリ</param>
    /// <param name="language">言語</param>
    public void Initialize(
        string settingDirectory,
        string language
        )
    {
        PluginData.SettingDirectoryPath = settingDirectory;
        SettingFileProcessing.ReadSettings();
        PluginData.MonitorInformation = MonitorInformation.GetMonitorInformation();

        HookThread.Start();
        if (Hook == IntPtr.Zero)
        {
            Hook = NativeMethods.SetWinEventHook((int)EVENT.EVENT_SYSTEM_MENUPOPUPSTART, (int)EVENT.EVENT_SYSTEM_MENUPOPUPSTART, IntPtr.Zero, ContextMenuEventCallback, 0, 0, (int)WINEVENT.WINEVENT_OUTOFCONTEXT);
        }
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destruction()
    {
        if (Hook != IntPtr.Zero)
        {
            NativeMethods.UnhookWinEvent(Hook);
            Hook = IntPtr.Zero;
        }

        if (SettingsWindow != null)
        {
            SettingsWindow.Close();
            SettingsWindow = null;
        }
    }

    /// <summary>
    /// コンテキストメニューのイベントのコールバック
    /// </summary>
    /// <param name="hWinEventHook"></param>
    /// <param name="eventType"></param>
    /// <param name="hwnd"></param>
    /// <param name="idObject"></param>
    /// <param name="idChild"></param>
    /// <param name="dwEventThread"></param>
    /// <param name="dwmsEventTime"></param>
    private async static void ContextMenuEventCallback(
        IntPtr hWinEventHook,
        uint eventType,
        IntPtr hwnd,
        int idObject,
        int idChild,
        uint dwEventThread,
        uint dwmsEventTime
        )
    {
        try
        {
            // 処理を遅らせる。
            // 遅らせないと一部のメニューが移動されない。
            await Task.Delay(100);

            NativeMethods.GetWindowPlacement(hwnd, out WINDOWPLACEMENT windowPlacement);
            RectangleInt menuRectangle = new()
            {
                Left = windowPlacement.rcNormalPosition.Left,
                Top = windowPlacement.rcNormalPosition.Top,
                Right = windowPlacement.rcNormalPosition.Right,
                Bottom = windowPlacement.rcNormalPosition.Bottom
            };      // メニューの上下左右の位置
             MonitorInformation.GetMonitorInformationForSpecifiedArea(menuRectangle, out MonitorInfoEx monitorInfo);

            RectangleInt rectangle = new();      // 移動後の位置
            switch (PluginData.Settings.XType)
            {
                case WindowXType.DoNotChange:
                    rectangle.Left = menuRectangle.Left;
                    break;
                case WindowXType.Left:
                    rectangle.Left = monitorInfo.WorkArea.Left;
                    break;
                case WindowXType.Middle:
                    rectangle.Left = monitorInfo.WorkArea.Left + ((monitorInfo.WorkArea.Right - monitorInfo.WorkArea.Left) / 2) - (menuRectangle.Width / 2);
                    break;
                case WindowXType.Right:
                    rectangle.Left = monitorInfo.WorkArea.Right - menuRectangle.Width;
                    break;
                case WindowXType.Value:
                    rectangle.Left = monitorInfo.WorkArea.Left + (int)PluginData.Settings.X;
                    break;
            }
            switch (PluginData.Settings.YType)
            {
                case WindowYType.DoNotChange:
                    rectangle.Top = menuRectangle.Top;
                    break;
                case WindowYType.Top:
                    rectangle.Top = monitorInfo.WorkArea.Top;
                    break;
                case WindowYType.Middle:
                    rectangle.Top = monitorInfo.WorkArea.Top + ((monitorInfo.WorkArea.Bottom - monitorInfo.WorkArea.Top) / 2) - (menuRectangle.Height / 2);
                    break;
                case WindowYType.Bottom:
                    rectangle.Top = monitorInfo.WorkArea.Bottom - menuRectangle.Height;
                    break;
                case WindowYType.Value:
                    rectangle.Top = monitorInfo.WorkArea.Top + (int)PluginData.Settings.Y;
                    break;
            }

            NativeMethods.SetWindowPos(hwnd, (int)HwndInsertAfter.HWND_TOPMOST, rectangle.Left, rectangle.Top, 0, 0, (int)SWP.SWP_NOACTIVATE | (int)SWP.SWP_NOZORDER | (int)SWP.SWP_NOSIZE);
        }
        catch
        {
        }
    }

    /// <summary>
    /// ウィンドウを表示
    /// </summary>
    public void ShowWindow()
    {
        if (SettingsWindow == null)
        {
            SettingsWindow = new();
            SettingsWindow.Closed += Window_Closed;
            SettingsWindow.Show();
        }
    }

    /// <summary>
    /// 「設定」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            SettingsWindow = null;
        }
        catch
        {
        }
    }
}
