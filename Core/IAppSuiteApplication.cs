﻿using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace CarinaStudio.AppSuite
{
    /// <summary>
    /// Interface of AppSuite application.
    /// </summary>
    public interface IAppSuiteApplication : IAvaloniaApplication
    {
        /// <summary>
        /// Add custom resource to aplication resource dictionary.
        /// </summary>
        /// <param name="resource">Resource.</param>
        /// <returns><see cref="IDisposable"/> which represents token of added resource.</returns>
        IDisposable AddCustomResource(IResourceProvider resource);


        /// <summary>
        /// Called when user agree the Privacy Policy.
        /// </summary>
        void AgreePrivacyPolicy();


        /// <summary>
        /// Called when user agree the User Agreement.
        /// </summary>
        void AgreeUserAgreement();


        /// <summary>
        /// Check application update information asynchronously.
        /// </summary>
        /// <returns>Task to wait for checking.</returns>
        Task<ApplicationUpdateInfo?> CheckUpdateInfoAsync();


        /// <summary>
        /// Get application configuration.
        /// </summary>
        Configuration.ISettings Configuration { get; }


        /// <summary>
        /// Get or set custom screen scale factor for Linux.
        /// </summary>
        double CustomScreenScaleFactor { get; set; }


        /// <summary>
        /// Get effective custom screen scale factor for Linux.
        /// </summary>
        double EffectiveCustomScreenScaleFactor { get; }


        /// <summary>
        /// Get theme mode which is currently applied to application.
        /// </summary>
        ThemeMode EffectiveThemeMode { get; }


        /// <summary>
        /// Get information of hardware.iapsu
        /// </summary>
        HardwareInfo HardwareInfo { get; }


        /// <summary>
        /// Check whether application is running in debug mode or not.
        /// </summary>
        bool IsDebugMode { get; }


        /// <summary>
        /// Check whether this is the first time application launch or not.
        /// </summary>
        bool IsFirstLaunch { get; }


        /// <summary>
        /// Check whether the current Privacy Policy has been agreed by user or not.
        /// </summary>
        bool IsPrivacyPolicyAgreed { get; }


        /// <summary>
        /// Check whether the Privacy Policy has been agreed by user before or not.
        /// </summary>
        bool IsPrivacyPolicyAgreedBefore { get; }


        /// <summary>
        /// Check whether restarting all main windows is needed or not.
        /// </summary>
        bool IsRestartingMainWindowsNeeded { get; }


        /// <summary>
        /// Check whether application is running as Administrator/Superuser or not.
        /// </summary>
        bool IsRunningAsAdministrator { get; }


        /// <summary>
        /// Check whether <see cref="ThemeMode.System"/> is supported or not.
        /// </summary>
        bool IsSystemThemeModeSupported { get; }


        /// <summary>
        /// Check whether the current User Agreement has been agreed by user or not.
        /// </summary>
        bool IsUserAgreementAgreed { get; }


        /// <summary>
        /// Check whether the User Agreement has been agreed by user before or not.
        /// </summary>
        bool IsUserAgreementAgreedBefore { get; }


        /// <summary>
        /// Get latest active main window.
        /// </summary>
        CarinaStudio.Controls.Window? LatestActiveMainWindow { get; }


        /// <summary>
        /// Get options to launch application which is converted by arguments passed to application.
        /// </summary>
        IDictionary<string, object> LaunchOptions { get; }


        /// <summary>
        /// Layout existing main windows.
        /// </summary>
        /// <param name="screen"><see cref="Avalonia.Platform.Screen"/> to layout main windows.</param>
        /// <param name="layout">Layout.</param>
        /// <param name="activeMainWindow">Main window which should be active one after layout.</param>
        void LayoutMainWindows(Avalonia.Platform.Screen screen, Controls.MultiWindowLayout layout, CarinaStudio.Controls.Window? activeMainWindow);


        /// <summary>
        /// Raised when start loading string resources for given culture.
        /// </summary>
        event EventHandler<IAppSuiteApplication, CultureInfo> LoadingStrings;


        /// <summary>
        /// Load <see cref="IApplication.PersistentState"/> from file.
        /// </summary>
        /// <returns>Task of loading.</returns>
        Task LoadPersistentStateAsync();


        /// <summary>
        /// Load <see cref="IApplication.Settings"/> from file.
        /// </summary>
        /// <returns>Task of loading.</returns>
        Task LoadSettingsAsync();


        /// <summary>
        /// Get list of main windows.
        /// </summary>
        IList<CarinaStudio.Controls.Window> MainWindows { get; }


        /// <summary>
        /// Get name of application.
        /// </summary>
        string Name { get; }


        /// <summary>
        /// Get URI of application package manifest.
        /// </summary>
        Uri? PackageManifestUri { get; }


        /// <summary>
        /// Get version of the Privacy Policy. Null means there is no Privacy Policy.
        /// </summary>
        Version? PrivacyPolicyVersion { get; }


        /// <summary>
        /// Get information of current process.
        /// </summary>
        ProcessInfo ProcessInfo { get; }


        /// <summary>
        /// Get type of application releasing.
        /// </summary>
        ApplicationReleasingType ReleasingType { get; }


        /// <summary>
        /// Restart application.
        /// </summary>
        /// <param name="args">Arguments to restart.</param>
        /// <param name="asAdministrator">True to restart application as Administrator/Superuser.</param>
        /// <returns>True if restarting has been accepted.</returns>
        bool Restart(string? args = null, bool asAdministrator = false);


        /// <summary>
        /// Request restarting given main window.
        /// </summary>
        /// <param name="mainWindow">Main window to restart.</param>
        /// <returns>True if restarting has been accepted.</returns>
        bool RestartMainWindow(CarinaStudio.Controls.Window mainWindow);


        /// <summary>
        /// Request restarting all main windows.
        /// </summary>
        /// <returns>True if restarting has been accepted.</returns>
        bool RestartMainWindows();


        /// <summary>
        /// Save <see cref="IApplication.PersistentState"/> to file.
        /// </summary>
        /// <returns>Task of saving.</returns>
        Task SavePersistentStateAsync();


        /// <summary>
        /// Save <see cref="IApplication.Settings"/> to file.
        /// </summary>
        /// <returns>Task of saving.</returns>
        Task SaveSettingsAsync();


        /// <summary>
        /// Create and show main window.
        /// </summary>
        /// <param name="windowCreatedAction">Action to perform when window created.</param>
        /// <returns>True if main window created and shown successfully.</returns>
        bool ShowMainWindow(Action<CarinaStudio.Controls.Window>? windowCreatedAction = null);


        /// <summary>
        /// Close all main windows and shut down application.
        /// </summary>
        void Shutdown();


        /// <summary>
        /// Get latest checked application update information.
        /// </summary>
        ApplicationUpdateInfo? UpdateInfo { get; }


        /// <summary>
        /// Get version of the User Agreement. Null means there is no User Agreement.
        /// </summary>
        Version? UserAgreementVersion { get; }
    }
}
