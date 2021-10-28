﻿using Avalonia;
using Avalonia.Controls;
using System;
using System.Net;

namespace CarinaStudio.AppSuite.Controls
{
    /// <summary>
	/// <see cref="TextBox"/> which treat input text as <see cref="IPAddress"/>.
	/// </summary>
    public class IPAddressTextBox : ObjectTextBox<IPAddress>
    {
        /// <summary>
        /// Property of <see cref="IPAddress"/>.
        /// </summary>
        public static readonly AvaloniaProperty<IPAddress?> IPAddressProperty = AvaloniaProperty.Register<IPAddressTextBox, IPAddress?>(nameof(IPAddress));


        /// <summary>
        /// Get or set <see cref="IPAddress"/>.
        /// </summary>
        public IPAddress? IPAddress
        {
            get => this.GetValue<IPAddress?>(IPAddressProperty);
            set => this.SetValue<IPAddress?>(IPAddressProperty, value);
        }


        /// <inheritdoc/>.
        protected override void OnPropertyChanged<TProperty>(AvaloniaPropertyChangedEventArgs<TProperty> change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == IPAddressProperty)
                this.SetValue<IPAddress?>(ObjectProperty, change.NewValue.Value as IPAddress);
            else if (change.Property == ObjectProperty)
                this.SetValue<IPAddress?>(IPAddressProperty, change.NewValue.Value as IPAddress);
        }


        /// <inheritdoc/>
        protected override bool TryConvertToObject(string text, out IPAddress? obj) => IPAddress.TryParse(text, out obj);
    }
}