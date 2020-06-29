using Frederisk_RtfEditor_DirectXWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frederisk.RtfEditor.Windows.Pages {

    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class EditorPage : Page {

        public EditorPage() {
            this.InitializeComponent();
            ViewModule = new EditorPageViewModule();
        }

        public EditorPageViewModule ViewModule { get; private set; }

        private void ToggleButton_OnClick(Object sender, RoutedEventArgs e) {
            var senderButton = sender as ToggleButton;
            var value = senderButton?.IsChecked ?? false ? FormatEffect.On : FormatEffect.Off;
            var selection = main_RichEditBox.Document.Selection;
            switch (senderButton?.Tag) {
                case "Bold":
                    selection.CharacterFormat.Bold = value;
                    break;

                case "Italic":
                    selection.CharacterFormat.Italic = value;
                    break;

                case "Underline":
                    selection.CharacterFormat.Underline =
                        value is FormatEffect.On ? UnderlineType.Single : UnderlineType.None;
                    break;

                case "AlignCenter":
                    selection.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    break;

                case "AlignLeft":
                    selection.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    break;

                case "AlignRight":
                    selection.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                    break;

                default:
                    break;
            }
        }
    }

    public sealed class EditorPageViewModule : BindableBase {

        public EditorPageViewModule() {
            // 初始化字型
            var writeFactory = new WriteFactory();
            WriteFontCollection writeFontCollection = writeFactory.GetSystemFontCollection();
            var count = writeFontCollection.GetFontFamilyCount();
            var fontFamilies = new FontFamily[count];
            for (var i = 0; i < count; i++) {
                WriteFontFamily writeFontFamily = writeFontCollection.GetFontFamily(i);
                WriteLocalizedStrings writeLocalizedStrings = writeFontFamily.GetFamilyNames();
                if (writeLocalizedStrings.FindLocaleName("en-us", out var index)) {
                    fontFamilies[i] = new FontFamily(writeLocalizedStrings.GetString(index));
                }
                else {
                    fontFamilies[i] = new FontFamily(writeLocalizedStrings.GetString(0));
                }
            }
            var fontItems = fontFamilies.
                OrderBy(fontFamily => fontFamily.Source).
                Select(fontFamily =>
                new StackPanel {
                    Tag = fontFamily.Source,
                    Children = {
                        new TextBlock {
                            Text = fontFamily.Source,
                            FontFamily = fontFamily
                        },
                        new TextBlock {
                            Text = fontFamily.Source
                        }
                    }
                });

            FontCollection = new ObservableCollection<StackPanel>(fontItems);
            // NumberList = new List<Int32> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            BaseColorBrushes = new List<Brush>{
                new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.Red),new SolidColorBrush(Colors.Yellow),
                new SolidColorBrush(Colors.Blue),new SolidColorBrush(Colors.Green),new SolidColorBrush(Colors.Gray),
                new SolidColorBrush(Colors.Aqua),new SolidColorBrush(Colors.Gold),new SolidColorBrush(Colors.OrangeRed),
            };
        }

        private Boolean? _isBold;
        private Boolean? _isItalic;
        private Boolean? _isUnderline;
        private Boolean? _isAlignRight;
        private Boolean? _isAlignLeft;
        private Boolean? _isAlignCenter;
        private StackPanel _fontSelected;

        public ObservableCollection<StackPanel> FontCollection { get; }

        // public List<Int32> NumberList { get; }

        public List<Brush> BaseColorBrushes { get; }

        public Boolean? IsBold {
            get => _isBold;
            set => SetProperty(ref _isBold, value, nameof(IsBold));
        }

        public Boolean? IsItalic {
            get => _isItalic;
            set => SetProperty(ref _isItalic, value);
        }

        public Boolean? IsUnderline {
            get => _isUnderline;
            set => SetProperty(ref _isUnderline, value);
        }

        public Boolean? IsAlignCenter {
            get => _isAlignCenter;
            set => SetProperty(ref _isAlignCenter, value);
        }

        public Boolean? IsAlignLeft {
            get => _isAlignLeft;
            set => SetProperty(ref _isAlignLeft, value);
        }

        public Boolean? IsAlignRight {
            get => _isAlignRight;
            set => SetProperty(ref _isAlignRight, value);
        }

        public StackPanel FontSelected {
            get => _fontSelected;
            set => SetProperty(ref _fontSelected, value);
        }

        private Double _fontSize;

        public String FontSize {
            get => _fontSize.ToString(CultureInfo.CurrentCulture);
            set {
                if (Double.TryParse(value, out Double i)) {
                    SetProperty(ref _fontSize, i);
                }
                else {
                    OnPropertyChanged();
                }
            }
        }

        // ReSharper disable once UnusedParameter.Global
        public void OnContentChanged(Object sender, RoutedEventArgs args) {
            var senderBox = sender as RichEditBox;
            var selection = senderBox?.Document.Selection;
            IsBold = selection?.CharacterFormat.Bold is FormatEffect.On;
            IsItalic = selection?.CharacterFormat.Italic is FormatEffect.On;
            IsUnderline = !(selection?.CharacterFormat.Underline is UnderlineType.None);
            IsAlignLeft = selection?.ParagraphFormat.Alignment is ParagraphAlignment.Left;
            IsAlignCenter = selection?.ParagraphFormat.Alignment is ParagraphAlignment.Center;
            IsAlignRight = selection?.ParagraphFormat.Alignment is ParagraphAlignment.Right;
            FontSize = selection?.CharacterFormat.Size.ToString(CultureInfo.CurrentCulture);
        }

        //public sealed class FormatConverter : IValueConverter {
        //    public Object Convert(Object value, Type targetType, Object parameter, String language) =>
        //        (FormatEffect)value switch
        //        {
        //            FormatEffect.On => true,
        //            _ => false
        //        };

        //    public Object ConvertBack(Object value, Type targetType, Object parameter, String language) =>
        //        (Boolean?)value switch
        //        {
        //            true => FormatEffect.On,
        //            false => FormatEffect.Off,
        //            null => FormatEffect.Undefined
        //        };
        //}

    }
}