namespace MoveMenu;

/// <summary>
/// 設定
/// </summary>
public class Settings
{
    /// <summary>
    /// X位置指定の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowXType XType { get; set; }
    /// <summary>
    /// X
    /// </summary>
    public double X { get; set; }
    /// <summary>
    /// Y位置指定の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowYType YType { get; set; }
    /// <summary>
    /// Y
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Settings()
    {
        XType = WindowXType.Left;
        X = 0;
        YType = WindowYType.Top;
        Y = 0;
    }
}
