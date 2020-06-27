using Frederisk_RtfEditor_DirectXWrapper;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frederisk.RtfEditor.Windows.Pages {

    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class EditorPage : Page {

        public EditorPage() {
            this.InitializeComponent();
        }
    }

    public sealed class EditorPageViewModule : BindableBase {

        public EditorPageViewModule() {
            // 初始化字型
            var writeFactory = new WriteFactory();
            WriteFontCollection writeFontCollection = writeFactory.GetSystemFontCollection();
            var count = writeFontCollection.GetFontFamilyCount();
            var fonts = new String[count];
            for (var i = 0; i < count; i++) {
                WriteFontFamily writeFontFamily = writeFontCollection.GetFontFamily(i);
                WriteLocalizedStrings writeLocalizedStrings = writeFontFamily.GetFamilyNames();
                if (writeLocalizedStrings.FindLocaleName("en-us", out var index)) {
                    fonts[i] = writeLocalizedStrings.GetString(index);
                }
                else {
                    fonts[i] = writeLocalizedStrings.GetString(0);
                }
            }
            Array.Sort(fonts);
            _fontCollection = new ObservableCollection<String>(fonts);
        }

        private ObservableCollection<String> _fontCollection;
    }
}