﻿#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Data;

namespace Frederisk.RtfEditor.Windows {

    /// <summary>
    /// 實現<see cref="INotifyPropertyChanged"/>的簡化模型。
    /// </summary>
    [WebHostHidden]
    public abstract class BindableBase : INotifyPropertyChanged {

        /// <summary>
        /// 屬性更改通知的多播事件。
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 檢查屬性是否已與所需值匹配。設置屬性，並在必要時通知攔截器。
        /// </summary>
        /// <typeparam name="T">屬性的類型。</typeparam>
        /// <param name="storage">對具有getter和setter的屬性的引用。</param>
        /// <param name="value">屬性所需的值。</param>
        /// <param name="propertyName">用於通知攔截器的屬性的名稱。
        /// 此值是可選的，可以在支援CallerMemberName的編譯器調用時自動提供。</param>
        /// <returns>如果值更改為true，則如果現有值與所需值匹配，則為false。</returns>
        protected Boolean SetProperty<T>(ref T storage, T value, [CallerMemberName] String? propertyName = null) {
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 通知攔截器屬性值已更改。
        /// </summary>
        /// <param name="propertyName">用於通知攔截器的屬性的名稱。
        /// 此值是可選的，可以在支援<see cref="CallerMemberNameAttribute"/>
        /// 的編譯器中調用時自動提供。</param>
        // ReSharper disable once MemberCanBePrivate.Global
        protected void OnPropertyChanged([CallerMemberName] String? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}