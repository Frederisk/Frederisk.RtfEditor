#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frederisk.RtfEditor.Windows.Pages {

    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class StartPage : Page {

        public StartPage() {
            this.InitializeComponent();
            this.ViewModule = new StartPageViewModule();
        }

        public void ComeTo(Type type) {
            this.Frame.Navigate(type);
        }

        public StartPageViewModule ViewModule { get; set; }
    }

    public sealed class RtfTemplate {
        public Symbol ViewSymbol { get; set; }
        public String? Name { get; set; }
        public String? Introduction { get; set; }
        public Boolean IsAvailable { get; set; }
    }

    public sealed class StartPageViewModule : BindableBase {

        public StartPageViewModule() {
            _templates = new ObservableCollection<RtfTemplate> {
                // TODO: Add more template and try to use local file to create template.
                new RtfTemplate {
                    ViewSymbol = Symbol.Home,
                    Name = "Empty",
                    Introduction = "An empty text file.",
                    IsAvailable = true
                }
            };
            Enumerable.Range(0, 10).ToList().ForEach(delegate {
                _templates.Add(new RtfTemplate {
                    ViewSymbol = Symbol.Preview,
                    Name = "Unavailable",
                    IsAvailable = false
                });
            });
        }

        private readonly ObservableCollection<RtfTemplate> _templates;

        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public ObservableCollection<RtfTemplate> Templates {
            // ReSharper disable once ArrangeAccessorOwnerBody
            get => _templates;
           // set => SetProperty(ref _templates, value);
        }
    }
}