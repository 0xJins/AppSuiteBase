﻿using CarinaStudio.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarinaStudio.AppSuite
{
    /// <summary>
    /// Interface of AppSuite application.
    /// </summary>
    public interface IAppSuiteApplication : IApplication
    {
        /// <summary>
        /// Check application update information asynchronously.
        /// </summary>
        /// <returns>Task to wait for checking.</returns>
        Task<ApplicationUpdateInfo?> CheckUpdateInfoAsync();


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
        /// Check whether restarting all main windows is needed or not.
        /// </summary>
        bool IsRestartingMainWindowsNeeded { get; }


        /// <summary>
        /// Check whether <see cref="ThemeMode.System"/> is supported or not.
        /// </summary>
        bool IsSystemThemeModeSupported { get; }


        /// <summary>
        /// Get options to launch application which is converted by arguments passed to application.
        /// </summary>
        IDictionary<string, object> LaunchOptions { get; }


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
        IList<Window> MainWindows { get; }


        /// <summary>
        /// Get name of application.
        /// </summary>
        string Name { get; }


        /// <summary>
        /// Get URI of application package manifest.
        /// </summary>
        Uri? PackageManifestUri { get; }


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
        /// <returns>True if restarting has been accepted.</returns>
        bool Restart(string? args = null);


        /// <summary>
        /// Request restarting given main window.
        /// </summary>
        /// <param name="mainWindow">Main window to restart.</param>
        /// <returns>True if restarting has been accepted.</returns>
        bool RestartMainWindow(Window mainWindow);


        /// <summary>
        /// Request restarting all main windows.
        /// </summary>
        /// <returns>True if restarting has been accepted.</returns>
        bool RestartMainWindows();


        /// <summary>
        /// Save <see cref="CarinaStudio.IApplication.PersistentState"/> to file.
        /// </summary>
        /// <returns>Task of saving.</returns>
        Task SavePersistentStateAsync();


        /// <summary>
        /// Save <see cref="CarinaStudio.IApplication.Settings"/> to file.
        /// </summary>
        /// <returns>Task of saving.</returns>
        Task SaveSettingsAsync();


        /// <summary>
        /// Create and show main window.
        /// </summary>
        /// <returns>True if main window created and shown successfully.</returns>
        bool ShowMainWindow();


        /// <summary>
        /// Close all main windows and shut down application.
        /// </summary>
        void Shutdown();


        /// <summary>
        /// Get latest checked application update information.
        /// </summary>
        ApplicationUpdateInfo? UpdateInfo { get; }
    }
}
