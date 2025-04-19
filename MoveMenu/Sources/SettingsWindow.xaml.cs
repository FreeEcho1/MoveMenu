using ModernWpf.Controls;
using System.Windows.Controls;

namespace MoveMenu;

/// <summary>
/// 設定ウィンドウ
/// </summary>
public partial class SettingsWindow : Window
{
    /// <summary>
    /// 設定ファイルに書き込むかの確認値
    /// </summary>
    private bool CheckWriteSettingFile = false;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SettingsWindow()
    {
        InitializeComponent();

        switch (PluginData.Settings.XType)
        {
            case WindowXType.DoNotChange:
                XComboBox.SelectedIndex = 0;
                break;
            case WindowXType.Left:
                XComboBox.SelectedIndex = 1;
                break;
            case WindowXType.Middle:
                XComboBox.SelectedIndex = 2;
                break;
            case WindowXType.Right:
                XComboBox.SelectedIndex = 3;
                break;
            case WindowXType.Value:
                XComboBox.SelectedIndex = 4;
                break;
        }
        XNumberBox.Value = PluginData.Settings.X;
        switch (PluginData.Settings.YType)
        {
            case WindowYType.DoNotChange:
                YComboBox.SelectedIndex = 0;
                break;
            case WindowYType.Top:
                YComboBox.SelectedIndex = 1;
                break;
            case WindowYType.Middle:
                YComboBox.SelectedIndex = 2;
                break;
            case WindowYType.Bottom:
                YComboBox.SelectedIndex = 3;
                break;
            case WindowYType.Value:
                YComboBox.SelectedIndex = 4;
                break;
        }
        YNumberBox.Value = PluginData.Settings.Y;

        XComboBox.SelectionChanged += XComboBox_SelectionChanged;
        XNumberBox.ValueChanged += XNumberBox_ValueChanged;
        YComboBox.SelectionChanged += YComboBox_SelectionChanged;
        YNumberBox.ValueChanged += YNumberBox_ValueChanged;
    }

    /// <summary>
    /// 「ContentRendered」イベント
    /// </summary>
    /// <param name="e"></param>
    protected override void OnContentRendered(
        EventArgs e
        )
    {
        base.OnContentRendered(e);
        // WPFの「SizeToContent」「WidthAndHeight」のバグ対策
        InvalidateMeasure();
    }

    /// <summary>
    /// 「Close」イベント
    /// </summary>
    /// <param name="e"></param>
    protected override void OnClosed(
        EventArgs e
        )
    {
        try
        {
            base.OnClosed(e);

            if (CheckWriteSettingFile)
            {
                SettingFileProcessing.WriteSettings();
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void XComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string selectedItem = (string)((ComboBoxItem)XComboBox.SelectedItem).Content;
            switch (selectedItem)
            {
                case "変更しない":
                    PluginData.Settings.XType = WindowXType.DoNotChange;
                    break;
                case "左端":
                    PluginData.Settings.XType = WindowXType.Left;
                    break;
                case "中央":
                    PluginData.Settings.XType = WindowXType.Middle;
                    break;
                case "右端":
                    PluginData.Settings.XType = WindowXType.Right;
                    break;
                case "座標指定":
                    PluginData.Settings.XType = WindowXType.Value;
                    break;
            }
            CheckWriteSettingFile = true;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」NumberBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void XNumberBox_ValueChanged(
        NumberBox sender,
        NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            PluginData.Settings.X = XNumberBox.Value;
            CheckWriteSettingFile = true;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Y」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void YComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string selectedItem = (string)((ComboBoxItem)YComboBox.SelectedItem).Content;
            switch (selectedItem)
            {
                case "変更しない":
                    PluginData.Settings.YType = WindowYType.DoNotChange;
                    break;
                case "上端":
                    PluginData.Settings.YType = WindowYType.Top;
                    break;
                case "中央":
                    PluginData.Settings.YType = WindowYType.Middle;
                    break;
                case "下端":
                    PluginData.Settings.YType = WindowYType.Bottom;
                    break;
                case "座標指定":
                    PluginData.Settings.YType = WindowYType.Value;
                    break;
            }
            CheckWriteSettingFile = true;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Y」NumberBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void YNumberBox_ValueChanged(
        NumberBox sender,
        NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            PluginData.Settings.Y = YNumberBox.Value;
            CheckWriteSettingFile = true;
        }
        catch
        {
        }
    }
}
