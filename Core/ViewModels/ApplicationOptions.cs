﻿using Avalonia.Data.Converters;
using CarinaStudio.Configuration;
using CarinaStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CarinaStudio.AppSuite.ViewModels
{
    /// <summary>
    /// View-model for application options UI.
    /// </summary>
    public class ApplicationOptions : ViewModel<IAppSuiteApplication>
    {
        /// <summary>
        /// <see cref="IValueConverter"/> to convert from <see cref="ApplicationCulture"/> to <see cref="string"/>.
        /// </summary>
        public static readonly IValueConverter ApplicationCultureConverter = new Converters.EnumConverter(AppSuiteApplication.CurrentOrNull, typeof(ApplicationCulture));
        /// <summary>
        /// <see cref="IValueConverter"/> to convert from <see cref="ThemeMode"/> to <see cref="string"/>.
        /// </summary>
        public static readonly IValueConverter ThemeModeConverter = new Converters.EnumConverter(AppSuiteApplication.CurrentOrNull, typeof(ThemeMode));


        /// <summary>
        /// Initialize new <see cref="ApplicationOptions"/> instance.
        /// </summary>
        public ApplicationOptions() : base(AppSuiteApplication.Current)
        {
            this.IsCustomScreenScaleFactorSupported = double.IsFinite(this.Application.CustomScreenScaleFactor);
            this.IsCustomScreenScaleFactorAdjusted = this.IsCustomScreenScaleFactorSupported
                && Math.Abs(this.Application.CustomScreenScaleFactor - this.Application.EffectiveCustomScreenScaleFactor) >= 0.01;
            this.ThemeModes = new List<ThemeMode>(Enum.GetValues<ThemeMode>()).Also(it =>
            {
                if (!this.Application.IsSystemThemeModeSupported)
                    it.Remove(ThemeMode.System);
            }).AsReadOnly();
        }


        /// <summary>
        /// Get or set whether to accept application update with non-stable version or not.
        /// </summary>
        public bool AcceptNonStableApplicationUpdate
        {
            get => this.Settings.GetValueOrDefault(SettingKeys.AcceptNonStableApplicationUpdate);
            set => this.Settings.SetValue<bool>(SettingKeys.AcceptNonStableApplicationUpdate, value);
        }


        /// <summary>
        /// Get or set application culture.
        /// </summary>
        public ApplicationCulture Culture
        {
            get => this.Settings.GetValueOrDefault(SettingKeys.Culture);
            set => this.Settings.SetValue<ApplicationCulture>(SettingKeys.Culture, value);
        }


        /// <summary>
        /// Get available values of <see cref="Culture"/>.
        /// </summary>
        public IList<ApplicationCulture> Cultures { get; } = new List<ApplicationCulture>(Enum.GetValues<ApplicationCulture>()).AsReadOnly();


        /// <summary>
        /// Get or set custom screen scale factor.
        /// </summary>
        public double CustomScreenScaleFactor
        {
            get => this.Application.CustomScreenScaleFactor;
            set => this.Application.CustomScreenScaleFactor = value;
        }


        /// <summary>
        /// Get effective custom screen scale factor.
        /// </summary>
        public double EffectiveCustomScreenScaleFactor { get => this.Application.EffectiveCustomScreenScaleFactor; }


        /// <summary>
        /// Get or set whether to enable blurry window background if available or not.
        /// </summary>
        public bool EnableBlurryBackground
        {
            get => this.Settings.GetValueOrDefault(SettingKeys.EnableBlurryBackground);
            set => this.Settings.SetValue<bool>(SettingKeys.EnableBlurryBackground, value);
        }


        /// <summary>
        /// Check whether custom screen scale factor is different from effective scale factor or not.
        /// </summary>
        public bool IsCustomScreenScaleFactorAdjusted { get; private set; }


        /// <summary>
        /// Check whether custom screen scale factor is supported or not.
        /// </summary>
        public bool IsCustomScreenScaleFactorSupported { get; }


        /// <summary>
        /// Check whether <see cref="LogOutputTargetPort"/> is supported or not.
        /// </summary>
        public bool IsLogOutputTargetPortSupported { get => (this.Application as AppSuiteApplication)?.DefaultLogOutputTargetPort != 0; }


        /// <summary>
        /// Check whether restarting all main windows is needed or not.
        /// </summary>
        public bool IsRestartingMainWindowsNeeded
        {
            get => this.Application.IsRestartingMainWindowsNeeded;
        }


        /// <summary>
        /// Get or set port of localhost to receive log output.
        /// </summary>
        public int LogOutputTargetPort
        {
            get => (this.Application as AppSuiteApplication)?.LogOutputTargetPort ?? 0;
            set => (this.Application as AppSuiteApplication)?.Let(app => app.LogOutputTargetPort = value);
        }


        /// <summary>
        /// Get or set whether to notify user when application update found or not.
        /// </summary>
        public bool NotifyApplicationUpdate
        {
            get => this.Settings.GetValueOrDefault(SettingKeys.NotifyApplicationUpdate);
            set => this.Settings.SetValue<bool>(SettingKeys.NotifyApplicationUpdate, value);
        }


        /// <summary>
        /// Called when property of application changed.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnApplicationPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnApplicationPropertyChanged(e);
            if (e.PropertyName == nameof(IAppSuiteApplication.CustomScreenScaleFactor))
            {
                this.OnPropertyChanged(nameof(CustomScreenScaleFactor));
                var adjusted = Math.Abs(this.Application.CustomScreenScaleFactor - this.Application.EffectiveCustomScreenScaleFactor) >= 0.01;
                if (this.IsCustomScreenScaleFactorAdjusted != adjusted)
                {
                    this.IsCustomScreenScaleFactorAdjusted = adjusted;
                    this.OnPropertyChanged(nameof(IsCustomScreenScaleFactorAdjusted));
                }
            }
            else if (e.PropertyName == nameof(IAppSuiteApplication.IsRestartingMainWindowsNeeded))
                this.OnPropertyChanged(nameof(IsRestartingMainWindowsNeeded));
            else if (e.PropertyName == nameof(AppSuiteApplication.LogOutputTargetPort))
                this.OnPropertyChanged(nameof(LogOutputTargetPort));
        }


        /// <summary>
        /// Called when setting changed.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnSettingChanged(SettingChangedEventArgs e)
        {
            base.OnSettingChanged(e);
            var key = e.Key;
            if (key == SettingKeys.AcceptNonStableApplicationUpdate)
                this.OnPropertyChanged(nameof(AcceptNonStableApplicationUpdate));
            else if (key == SettingKeys.Culture)
                this.OnPropertyChanged(nameof(Culture));
            else if (key == SettingKeys.EnableBlurryBackground)
                this.OnPropertyChanged(nameof(EnableBlurryBackground));
            else if (key == SettingKeys.NotifyApplicationUpdate)
                this.OnPropertyChanged(nameof(NotifyApplicationUpdate));
            else if (key == SettingKeys.ThemeMode)
                this.OnPropertyChanged(nameof(ThemeMode));
        }


        /// <summary>
        /// Get or set theme mode.
        /// </summary>
        public ThemeMode ThemeMode
        {
            get => this.Settings.GetValueOrDefault(SettingKeys.ThemeMode);
            set
            {
                if (value == ThemeMode.System && !this.Application.IsSystemThemeModeSupported)
                    return;
                this.Settings.SetValue<ThemeMode>(SettingKeys.ThemeMode, value);
            }
        }


        /// <summary>
        /// Get available values of <see cref="ThemeMode"/>.
        /// </summary>
        public IList<ThemeMode> ThemeModes { get; } 
    }
}
