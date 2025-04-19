namespace MoveMenu;

/// <summary>
/// プラグインデータ
/// </summary>
public static class PluginData
{
    /// <summary>
    /// 設定ディレクトリのパス
    /// </summary>
    public static string SettingDirectoryPath { get; set; } = "";
    /// <summary>
    /// 設定
    /// </summary>
    public static Settings Settings { get; set; } = new();

    /// <summary>
    /// モニター情報
    /// </summary>
    public static MonitorInformation MonitorInformation { get; set; } = MonitorInformation.GetMonitorInformation();
}
