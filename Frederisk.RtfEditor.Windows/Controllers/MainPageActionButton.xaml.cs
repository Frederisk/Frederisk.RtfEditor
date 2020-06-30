#nullable enable

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 使用者控制項項目範本記載於 https://go.microsoft.com/fwlink/?LinkId=234236

namespace Frederisk.RtfEditor.Windows.Controllers {

    public sealed partial class MainPageActionButton : UserControl {

        public MainPageActionButton() {
            this.InitializeComponent();
        }

        #region DependencyProperty

        public String Text {
            get => (String)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(String),
                typeof(MainPageActionButton),
                new PropertyMetadata("A Sample")
            );

        public Symbol Symbol {
            get => (Symbol)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(
                nameof(Symbol),
                typeof(Symbol),
                typeof(MainPageActionButton),
                new PropertyMetadata(Symbol.Home)
            );

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
                "IsChecked",
                typeof(Boolean?),
                typeof(MainPageActionButton),
                new PropertyMetadata(false)
            );

        public Boolean? IsChecked {
            get => (Boolean?)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        #endregion DependencyProperty

        public Type? PageType { get; set; }
    }
}