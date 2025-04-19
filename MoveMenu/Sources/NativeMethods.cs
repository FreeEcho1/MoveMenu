namespace MoveMenu;

/// <summary>
/// NativeMethods
/// </summary>
internal static partial class NativeMethods
{
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
        IntPtr hWnd,
        int hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        int uFlags
        );

    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
        WinEventDelegate lpfnWinEventProc, uint idProcess,
        uint idThread, uint dwFlags);
    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    [DllImport("user32.dll")]
    public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll", EntryPoint = "EnumDisplayMonitors")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumDisplayMonitors(
        IntPtr hdc,
        IntPtr lprcClip,
        EnumMonitorsDelegate lpfnEnum,
        MonitorInformation dwData
        );
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetMonitorInfo(
        IntPtr hMonitor,
        ref MonitorInfoEx lpmi
        );
    public delegate int EnumMonitorsDelegate(
        IntPtr hMonitor,
        IntPtr hdcMonitor,
        ref RECT lprcMonitor,
        MonitorInformation dwData);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement(
        IntPtr hWnd,
        out WINDOWPLACEMENT lpwndpl
        );
}
