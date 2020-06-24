using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 使用者控制項項目範本記載於 https://go.microsoft.com/fwlink/?LinkId=234236

namespace Frederisk.RtfEditor.Windows.Controllers {
    public sealed partial class MainPageActionButton : UserControl {
        public MainPageActionButton() {
            this.InitializeComponent();
        }

        public String Text {
            get => GetValue(TextProperty) as String;
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(String),
            typeof(MainPageActionButton),
            new PropertyMetadata("A Sample")
            );

    }
}
