using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frederisk.RtfEditor.Windows.Pages {

    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class SettingPage : Page {

        public SettingPage() {
            this.InitializeComponent();
            this.VisualViewModule = App.VisualViewModule ?? throw new Exception();
        }

        public VisualEffectViewModule VisualViewModule { get; set; }

    }

    public class ElementThemeToBoolConverter : IValueConverter {

        public Object Convert(Object value, Type targetType, Object parameter, String language) {
            if (!(value is ElementTheme theme)) {
                throw new ArgumentException("");
            }
            return theme is ElementTheme.Light;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language) {
            if (!(value is Boolean isLight)) {
                throw new ArgumentException("");
            }
            return isLight ? ElementTheme.Light : ElementTheme.Dark;
        }
    }







}