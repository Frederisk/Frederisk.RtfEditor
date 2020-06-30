#nullable enable

using Frederisk_RtfEditor_DirectXWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frederisk.RtfEditor.Windows.Pages {

    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class EditorPage : Page {
        private StorageFile? _file;
        private String? _lastFormattedText;
        private Int32 _lastRawTextLength;


        public EditorPage() {
            this.InitializeComponent();
            ViewModule = new EditorPageViewModule();
            VisualViewModule = App.VisualViewModule;
        }

        /// <summary>
        /// 視圖模型。
        /// </summary>
        public EditorPageViewModule ViewModule { get; set; }

        public VisualEffectViewModule VisualViewModule { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">事件發送者。</param>
        /// <param name="e">路由事件資料。</param>
        private void ToggleButton_OnClick(Object sender, RoutedEventArgs e) {
            var senderButton = sender as ToggleButton;
            var value = senderButton?.IsChecked ?? false ? FormatEffect.On : FormatEffect.Off;
            var selection = main_RichEditBox.Document.Selection;
            switch (senderButton?.Tag.ToString()) {
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">事件發送者。</param>
        /// <param name="e">路由事件資料。</param>
        private void Font_ComBox_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
            var sendTag = ((sender as ComboBox)?.SelectedItem as StackPanel)?.Tag.ToString();
            main_RichEditBox.Document.Selection.CharacterFormat.Name = sendTag ?? FontFamily.XamlAutoFontFamily.Source;
        }



        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">事件發送者。</param>
        /// <param name="e">路由事件資料。</param>
        private void TextBox_OnTextChanged(Object sender, TextChangedEventArgs e) {
            if (!Single.TryParse((sender as TextBox)?.Text, out Single i) || !(i > 0) || !(i < 1638))
                return;
            main_RichEditBox.Document.Selection.CharacterFormat.Size = i;
        }

        private void ButtonBase_OnClick(Object sender, RoutedEventArgs e) {
            var bu = sender as Button;
            var color = (bu?.Content as Rectangle)?.Fill;
            if (color is null)
                throw new Exception();
            if (bu != null)
                bu.Background = color;
        }

        private void Color_Button_OnClick(Object sender, RoutedEventArgs e) {
            var button = sender as Button;
            switch (button?.Name) {
                //case nameof(fontColor_Button):
                //    var color = ViewModule.FontColorBrush?.Color ?? Colors.Black;
                //    main_RichEditBox.Document.Selection.CharacterFormat.ForegroundColor = color;
                //    main_RichEditBox.Focus(FocusState.Keyboard);
                //    currentColor = color;
                //    break;
                case nameof(backgroundColor_Button):
                    main_RichEditBox.Document.Selection.CharacterFormat.BackgroundColor = ViewModule.BackgroundColorBrush?.Color ?? Colors.White;
                    break;

                default:
                    break;
            }
        }

        private void Main_RichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args) {
            if (FocusManager.GetFocusedElement() == main_RichEditBox) {
                main_RichEditBox.Document.Selection.CharacterFormat.ForegroundColor = ViewModule.FontColorBrush?.Color ?? Colors.Black;
            }
        }

        private async void Save_Button_OnClick(Object sender, RoutedEventArgs e) {
            if (_file is null) {
                FileSavePicker savePicker = new FileSavePicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };
                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });
                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "New Document";
                _file = await savePicker.PickSaveFileAsync();
            }

            if (_file is null)
                return;
            // Prevent updates to the remote version of the file until we 
            // finish making changes and call CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(_file);
            // write to file
            try {
                using IRandomAccessStream randAccStream = await _file.OpenAsync(FileAccessMode.ReadWrite);
                main_RichEditBox.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
                // Let Windows know that we're finished changing the file so the 
                // other app can update the remote version of the file.
                var status = await CachedFileManager.CompleteUpdatesAsync(_file);
                if (status is FileUpdateStatus.Complete)
                    return;
                throw new IOException(FileUpdateStatus.Failed.ToString());
            }
            catch (IOException ex) {
                MessageDialog errorBox =
                    new MessageDialog("File " + _file.Name + " couldn't be saved. " + ex.Message);
                await errorBox.ShowAsync();
            }
        }

        private async void Open_Button_Click(object sender, RoutedEventArgs e) {
            // Open a text file.
            FileOpenPicker open = new FileOpenPicker {SuggestedStartLocation = PickerLocationId.DocumentsLibrary};
            open.FileTypeFilter.Add(".rtf");
            _file = await open.PickSingleFileAsync();
            if (_file == null) return;
            using IRandomAccessStream randAccStream =
                await _file.OpenAsync(FileAccessMode.Read);
            // Load the file into the Document property of the RichEditBox.
            main_RichEditBox.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);
        }


        private void Editor_LosingFocus(object sender, RoutedEventArgs e) {
            // Save text length to determine text length change
            main_RichEditBox.Document.GetText(TextGetOptions.UseCrlf, out string lastRawText);
            _lastRawTextLength = lastRawText.Length;
            // Save formatted to restore formatting upon regaining focus
            main_RichEditBox.Document.GetText(TextGetOptions.FormatRtf, out _lastFormattedText);
        }

        private void Editor_GotFocus(object sender, RoutedEventArgs e) {
            main_RichEditBox.Document.GetText(TextGetOptions.UseCrlf, out string currentRawText);
            if (currentRawText.Length != _lastRawTextLength) {
                // User used cut or paste from action command, skip the event
                return;
            }
            // reset colors to correct defaults for Focused state
            ITextRange documentRange = main_RichEditBox.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["TextControlBackgroundFocused"];
            SolidColorBrush foreground = (SolidColorBrush)App.Current.Resources["TextControlForegroundFocused"];
            main_RichEditBox.Document.ApplyDisplayUpdates();
            if (background != null && foreground != null) {
                documentRange.CharacterFormat.BackgroundColor = background.Color;
            }
            // saving selection span
            var caretPosition = main_RichEditBox.Document.Selection.GetIndex(TextRangeUnit.Character) - 1;
            if (caretPosition <= 0) {
                caretPosition = 1;
            }
            var selectionLength = main_RichEditBox.Document.Selection.Length;
            // restoring text styling, unintentionally sets caret position at beginning of text
            main_RichEditBox.Document.SetText(TextSetOptions.FormatRtf, _lastFormattedText ?? String.Empty);
            // restoring selection position
            main_RichEditBox.Document.Selection.SetIndex(TextRangeUnit.Character, caretPosition, false);
            main_RichEditBox.Document.Selection.SetRange(caretPosition, caretPosition + selectionLength);
            // Editor might have gained focus because user changed color.
            // Change selection color
            // Note that only way to regain with selection containing text is using the change color button
            main_RichEditBox.Document.Selection.CharacterFormat.ForegroundColor = ViewModule.FontColorBrush?.Color ?? Colors.Black;
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
        }

        private Boolean _isBold;
        private Boolean _isItalic;
        private Boolean _isUnderline;
        private Boolean _isAlignRight;
        private Boolean _isAlignLeft;
        private Boolean _isAlignCenter;
        private StackPanel? _fontSelected;
        private SolidColorBrush? _fontColorBrush;
        private SolidColorBrush? _backgroundColorBrush;

        public ObservableCollection<StackPanel> FontCollection { get; }

        // public List<Int32> NumberList { get; }

        // public List<Brush> BaseColorBrushes { get; }

        public Boolean IsBold {
            get => _isBold;
            set => SetProperty(ref _isBold, value);
        }

        public Boolean IsItalic {
            get => _isItalic;
            set => SetProperty(ref _isItalic, value);
        }

        public Boolean IsUnderline {
            get => _isUnderline;
            set => SetProperty(ref _isUnderline, value);
        }

        public Boolean IsAlignCenter {
            get => _isAlignCenter;
            set => SetProperty(ref _isAlignCenter, value);
        }

        public Boolean IsAlignLeft {
            get => _isAlignLeft;
            set => SetProperty(ref _isAlignLeft, value);
        }

        public Boolean IsAlignRight {
            get => _isAlignRight;
            set => SetProperty(ref _isAlignRight, value);
        }

        public StackPanel? FontSelected {
            get => _fontSelected;
            set => SetProperty(ref _fontSelected, value);
        }

        public SolidColorBrush? FontColorBrush {
            get => _fontColorBrush;
            set => SetProperty(ref _fontColorBrush, value);
        }

        public SolidColorBrush? BackgroundColorBrush {
            get => _backgroundColorBrush;
            set => SetProperty(ref _backgroundColorBrush, value);
        }

        private Single _fontSize;

        public String FontSize {
            get => _fontSize.ToString(CultureInfo.CurrentCulture);
            set {
                if (Single.TryParse(value, out Single i) && i > 0 && i < 1638) {
                    SetProperty(ref _fontSize, i);
                }
                else {
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 內容變更時動作。
        /// </summary>
        /// <param name="sender">事件發送者。</param>
        /// <param name="e">路由事件資料。</param>
        /// <exception cref="ArgumentNullException"></exception>
        // ReSharper disable once UnusedParameter.Global
        public void OnContentChanged(Object sender, RoutedEventArgs e) {
            var editor = (RichEditBox)sender;
            if (editor is null)
                throw new ArgumentNullException(nameof(sender));
            var selection = editor.Document.Selection;
            IsBold = selection.CharacterFormat.Bold is FormatEffect.On;
            IsItalic = selection.CharacterFormat.Italic is FormatEffect.On;
            IsUnderline = !(selection.CharacterFormat.Underline is UnderlineType.None);
            IsAlignLeft = selection.ParagraphFormat.Alignment is ParagraphAlignment.Left;
            IsAlignCenter = selection.ParagraphFormat.Alignment is ParagraphAlignment.Center;
            IsAlignRight = selection.ParagraphFormat.Alignment is ParagraphAlignment.Right;
            FontSize = selection.CharacterFormat.Size.ToString(CultureInfo.CurrentCulture);
            FontSelected = FontCollection.First(r => String.CompareOrdinal(r.Tag.ToString(), selection.CharacterFormat.Name) is 0);
        }

#pragma warning disable CA1707 // 識別項不應包含底線

        /// <summary>
        /// 字體大小變更按鈕點選時動作。
        /// </summary>
        /// <param name="sender">事件發送者。</param>
        /// <param name="e">路由事件資料。</param>
        public void ButtonBase_OnClick(Object sender, RoutedEventArgs e) {
#pragma warning restore CA1707 // 識別項不應包含底線
            var size = Single.Parse(FontSize, CultureInfo.CurrentCulture);

            FontSize = (sender as Button)?.Tag.ToString() switch
            {
                "FontDecrease" =>
                (size - 1).ToString(CultureInfo.CurrentCulture),
                "FontIncrease" =>
                (size + 1).ToString(CultureInfo.CurrentCulture),
                _ => throw new NotImplementedException()
            };
        }
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