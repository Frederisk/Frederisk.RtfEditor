#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// 使用者控制項項目範本記載於 https://go.microsoft.com/fwlink/?LinkId=234236

namespace Frederisk.RtfEditor.Windows.Controllers {

    public sealed partial class EditorPageColorSelectorControl : UserControl {

        public EditorPageColorSelectorControl() {
            this.InitializeComponent();
            ViewModule = new EditorPageColorSelectorControlViewModule();
        }

        public EditorPageColorSelectorControlViewModule ViewModule { get; set; }

        private void ButtonBase_OnClick(Object sender, RoutedEventArgs e) {
            ViewModule.BackgroundColorBrush = new SolidColorBrush(colorPicker.Color);
        }
    }

    public sealed class EditorPageColorSelectorControlViewModule : BindableBase {

        public EditorPageColorSelectorControlViewModule() {
            var colors = new Color[] { Colors.Black, Colors.Red, Colors.Yellow, Colors.Blue, Colors.Green, Colors.Gray, Colors.Aqua, Colors.Gold, Colors.White };

            ColorButtons = colors.Select(c => {
                var b = new Button {
                    Padding = new Thickness(0),
                    Content = new Rectangle { Height = 24, Width = 24, Fill = new SolidColorBrush(c) }
                };
                b.Click += (sender, args) => {
                    BackgroundColorBrush = ((Rectangle)((Button)sender).Content).Fill;
                };
                return b;
            }).ToList();

            BackgroundColorBrush = new SolidColorBrush(Colors.Black);
        }

        public List<Button> ColorButtons { get; }

        private Brush? _backgroundColorBrush;

        public Brush? BackgroundColorBrush {
            get => _backgroundColorBrush;
            set => SetProperty(ref _backgroundColorBrush, value);
        }
    }

    public class SolidColorBrushToColorConverter : IValueConverter {
        public Object Convert(Object value, Type targetType, Object parameter, String language) {
            if (value is SolidColorBrush colorBrush) {
                return colorBrush.Color;
            }
#pragma warning disable CA1303 // 不要將常值當做已當地語系化的參數傳遞
            throw new ArgumentException("ERROR TYPE");
#pragma warning restore CA1303 // 不要將常值當做已當地語系化的參數傳遞
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language) {
            if (value is Color color) {
                return new SolidColorBrush(color);
            }
#pragma warning disable CA1303 // 不要將常值當做已當地語系化的參數傳遞
            throw new ArgumentException("ERROR TYPE");
#pragma warning restore CA1303 // 不要將常值當做已當地語系化的參數傳遞
        }
    }



}