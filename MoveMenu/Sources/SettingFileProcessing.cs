using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MoveMenu;

/// <summary>
/// 設定ファイルの処理
/// </summary>
public static class SettingFileProcessing
{
    /// <summary>
    /// 設定ファイルのファイル名
    /// </summary>
    private static string SettingFileName { get; } = "MoveMenuSetting.ini";

    /// <summary>
    /// 設定ファイルを読み込む
    /// </summary>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    public static bool ReadSettings()
    {
        bool result = false;        // 結果

        try
        {
            string path = GetSettingFilePath();

            if (File.Exists(path))
            {
                string readString = "";

                using (StreamReader reader = new(path))
                {
                    readString = reader.ReadToEnd();
                }
                JsonSerializerOptions? options = new()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true,
                    IgnoreReadOnlyProperties = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                Settings? settings = JsonSerializer.Deserialize<Settings>(readString, options);
                if (settings != null)
                {
                    PluginData.Settings = settings;
                    result = true;
                }
            }
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 設定ファイルに書き込む
    /// </summary>
    public static bool WriteSettings()
    {
        bool result = false;        // 結果

        try
        {
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
                IgnoreReadOnlyProperties = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string writeString = JsonSerializer.Serialize(PluginData.Settings, options);
            string path = GetSettingFilePath();

            using (StreamWriter writer = new(path))
            {
                writer.Write(writeString);
            }

            result = true;
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 設定ファイルのパスを取得
    /// </summary>
    /// <returns>   設定ファイルのパス</returns>
    private static string GetSettingFilePath()
    {
        return Path.Combine(PluginData.SettingDirectoryPath, SettingFileName);
    }
}
