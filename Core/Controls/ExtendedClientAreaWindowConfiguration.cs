﻿using Avalonia;
using Avalonia.Controls;
using System;

namespace CarinaStudio.AppSuite.Controls
{
    /// <summary>
    /// Configuration of <see cref="Window"/> with <see cref="Avalonia.Controls.Window.ExtendClientAreaToDecorationsHint"/> set to true.
    /// </summary>
    public static class ExtendedClientAreaWindowConfiguration
    {
        /// <summary>
        /// Get padding of window content when maximized.
        /// </summary>
        public static Thickness ContentPaddingWhenMaximized { get; } = Global.Run(() =>
        {
            if (!Platform.IsWindows)
                return new Thickness();
            return new Thickness(6); // Windows 10
        });


        /// <summary>
        /// Check whether extending client area is supported on current platform or not.
        /// </summary>
        public static bool IsExtendedClientAreaSupported { get; } = Global.Run(() =>
        {
            if (Platform.IsWindows || Platform.IsMacOS)
                return true;
            return false;
        });


        /// <summary>
        /// Check whether <see cref="SystemChromePlacement"/> is <see cref="PlacementMode.Left"/> or not.
        /// </summary>
        public static bool IsSystemChromePlacedAtLeft { get => SystemChromePlacement == PlacementMode.Left; }


        /// <summary>
        /// Check whether <see cref="SystemChromePlacement"/> is <see cref="PlacementMode.Right"/> or not.
        /// </summary>
        public static bool IsSystemChromePlacedAtRight { get => SystemChromePlacement == PlacementMode.Right; }


        /// <summary>
        /// Get placement of system chrome.
        /// </summary>
        public static PlacementMode SystemChromePlacement { get; } = Global.Run(() =>
        {
            if (Platform.IsMacOS)
                return PlacementMode.Left;
            return PlacementMode.Right;
        });


        /// <summary>
        /// Get width of system chrome.
        /// </summary>
        public static double SystemChromeWidth { get; } = Global.Run(() =>
        {
            if (Platform.IsWindows)
                return 140; // Windows 10
            if (Platform.IsMacOS)
                return 70;
            return 0;
        });
    }
}