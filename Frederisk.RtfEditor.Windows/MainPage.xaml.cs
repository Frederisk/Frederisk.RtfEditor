﻿#nullable enable

using System;
using Frederisk.RtfEditor.Windows.Controllers;
using Frederisk.RtfEditor.Windows.Pages;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x404

namespace Frederisk.RtfEditor.Windows {

    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page {

        public MainPage() {
            this.InitializeComponent();
            this.ViewModule = new MainPageViewModule();
            this.VisualViewModule = App.VisualViewModule ?? throw new Exception();
        }

        /// <summary>
        /// 視圖模型。
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public MainPageViewModule ViewModule { get; set; }

        public VisualEffectViewModule VisualViewModule { get; set; }
    }

    public sealed class MainPageViewModule : BindableBase {

        public MainPageViewModule() {
            _mainFrame = new Frame();
            _mainFrame.Navigate(typeof(StartPage));
            var tButtons = new ObservableCollection<MainPageActionButton> {
#pragma warning disable CA1303 // 不要將常值當做已當地語系化的參數傳遞
                new MainPageActionButton{Width = 120, Height = 120, Text = "Start", Symbol = Symbol.Home, IsEnabled = false, IsChecked = true, PageType = typeof(StartPage)},
                new MainPageActionButton{Width = 120, Height = 120, Text = "New", Symbol = Symbol.NewFolder, PageType = typeof(NewPage)},
                new MainPageActionButton{Width = 120, Height = 120, Text = "Open", Symbol = Symbol.OpenFile, PageType = typeof(OpenPage)},
                new MainPageActionButton{Width = 120, Height = 120, Text = "Setting", Symbol = Symbol.Setting, PageType = typeof(SettingPage)}
#pragma warning restore CA1303 // 不要將常值當做已當地語系化的參數傳遞
            };
            foreach (var item in tButtons) {
                item.Tapped += (sender, args) => {
                    if (!(sender is MainPageActionButton b))
                        return;
                    this._mainFrame.Navigate(b.PageType);
                    tButtons.ToList().ForEach(i => {
                        if (i == b)
                            return;
                        i.IsEnabled = true;
                    });
                    b.IsEnabled = false;
                };
            }

            ActionButtons = tButtons;
        }

        private Frame _mainFrame;

        public Frame MainFrame {
            get => _mainFrame;
            set => SetProperty(ref _mainFrame, value);
        }

        public ObservableCollection<MainPageActionButton> ActionButtons { get; }
    }

}