using System.Threading.Tasks;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using CarinaStudio.AppSuite.Controls;
using CarinaStudio.AppSuite.Converters;
using CarinaStudio.AppSuite.ViewModels;
using CarinaStudio.Collections;
using CarinaStudio.Configuration;
using CarinaStudio.Controls;
using CarinaStudio.Data.Converters;
using CarinaStudio.Input;
using CarinaStudio.Threading;
using CarinaStudio.Windows.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using TabControl = Avalonia.Controls.TabControl;

namespace CarinaStudio.AppSuite.Tests
{
    partial class MainWindow : Controls.MainWindow<App, Workspace>
    {
        const string TabItemKey = "TabItem";


        static readonly AvaloniaProperty<int> Int32Property = AvaloniaProperty.Register<MainWindow, int>("Int32", 1);


        readonly MutableObservableBoolean canShowAppInfo = new MutableObservableBoolean(true);
        readonly IntegerTextBox integerTextBox;
        readonly IntegerTextBox integerTextBox2;
        readonly IPAddressTextBox ipAddressTextBox;
        readonly ScheduledAction logAction;
        private readonly ObservableList<TabItem> tabItems = new();


        public MainWindow()
        {
            this.ShowAppInfoDialogCommand = new Command(() => this.ShowAppInfoDialog(), this.canShowAppInfo);

            this.GetObservable(Int32Property).Subscribe(value =>
            {
                ;
            });

            InitializeComponent();

            this.logAction = new ScheduledAction(() =>
            {
                this.Logger.LogDebug($"Time: {DateTime.Now}");
                this.logAction?.Schedule(500);
            });

            this.integerTextBox = this.FindControl<IntegerTextBox>(nameof(integerTextBox)).AsNonNull();
            this.integerTextBox2 = this.FindControl<IntegerTextBox>(nameof(integerTextBox2)).AsNonNull();
            this.ipAddressTextBox = this.FindControl<IPAddressTextBox>(nameof(ipAddressTextBox)).AsNonNull();

            var tabControl = this.FindControl<TabControl>("tabControl").AsNonNull();
            this.tabItems.AddRange(tabControl.Items.Cast<TabItem>());
            tabControl.Items = this.tabItems;
            (this.tabItems[0].Header as Control)?.Let(it => this.Application.EnsureClosingToolTipIfWindowIsInactive(it));
        }


        ViewModels.ApplicationOptions ApplicationOptions { get; } = new ViewModels.ApplicationOptions();


        void EditConfiguration()
        {
            _ = new SettingsEditorDialog()
            {
                SettingKeys = SettingKey.GetDefinedKeys<ConfigurationKeys>(),
                Settings = this.Configuration,
            }.ShowDialog(this);
        }


        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);


        //public override bool IsExtendingClientAreaAllowed => false;


        protected override void OnApplicationPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnApplicationPropertyChanged(e);
        }


        protected override void OnClosed(EventArgs e)
        {
            this.logAction.Cancel();
            base.OnClosed(e);
        }


        protected override ApplicationInfo OnCreateApplicationInfo() => new AppInfo();


        void OnDragEnterTabItem(object? sender, DragOnTabItemEventArgs e)
        {
            (sender as Controls.TabControl)?.ScrollHeaderIntoView(e.ItemIndex);
        }


        void OnDragLeaveTabItem(object? sender, TabItemEventArgs e)
        {
            if (e.Item is not TabItem tabItem)
                return;
            ItemInsertionIndicator.SetInsertingItemAfter(tabItem, false);
            ItemInsertionIndicator.SetInsertingItemBefore(tabItem, false);
        }


        void OnDragOverTabItem(object? sender, DragOnTabItemEventArgs e)
        {
            e.DragEffects = DragDropEffects.None;

            //if (tabItems != null && e.ItemIndex < tabItems.Count - 1)
            //(sender as Controls.TabControl)?.Let(it => it.SelectedIndex = e.ItemIndex);

            if (!e.Data.TryGetData<TabItem>(TabItemKey, out var tabItem) || tabItem == null)
                return;

            if (tabItem == e.Item)
            {
                e.DragEffects = DragDropEffects.Move;
                e.Handled = true;
            }
            else if (e.ItemIndex < tabItems.Count - 1)
            {
                var srcIndex = tabItems.IndexOf(tabItem);
                if (srcIndex < 0)
                    return;

                e.DragEffects = DragDropEffects.Move;

                var targetIndex = e.PointerPosition.X <= e.HeaderVisual.Bounds.Width / 2
                    ? e.ItemIndex
                    : e.ItemIndex + 1;

                //System.Diagnostics.Debug.WriteLine($"srcIndex: {srcIndex}, targetIndex: {targetIndex}");

                if (targetIndex == srcIndex || targetIndex == srcIndex + 1)
                {
                    ItemInsertionIndicator.SetInsertingItemAfter((Control)e.Item, false);
                    ItemInsertionIndicator.SetInsertingItemBefore((Control)e.Item, false);
                    return;
                }

                if (targetIndex > e.ItemIndex)
                {
                    ItemInsertionIndicator.SetInsertingItemAfter((Control)e.Item, true);
                    ItemInsertionIndicator.SetInsertingItemBefore((Control)e.Item, false);
                }
                else
                {
                    ItemInsertionIndicator.SetInsertingItemAfter((Control)e.Item, false);
                    ItemInsertionIndicator.SetInsertingItemBefore((Control)e.Item, true);
                }

                /*
                var srcIndex = tabItems.IndexOf(tabItem);
                if (srcIndex < 0)
                    return;
                (sender as Controls.TabControl)?.Let(it => it.SelectedIndex = e.ItemIndex);
                tabItems.RemoveAt(srcIndex);
                tabItems.Insert(e.ItemIndex, tabItem);
                (sender as Controls.TabControl)?.Let(it => it.SelectedIndex = e.ItemIndex);
                */
                e.Handled = true;
            }
        }


        void OnDropOnTabItem(object? sender, DragOnTabItemEventArgs e)
        {
            if (e.Item is not TabItem tabItem)
                return;

            ItemInsertionIndicator.SetInsertingItemAfter(tabItem, false);
            ItemInsertionIndicator.SetInsertingItemBefore(tabItem, false);

            if (!e.Data.TryGetData<TabItem>(TabItemKey, out var item) || item == null || item == tabItem)
                return;

            var srcIndex = tabItems.IndexOf(item);
            if (srcIndex < 0)
                return;

            var targetIndex = e.PointerPosition.X <= e.HeaderVisual.Bounds.Width / 2
                    ? e.ItemIndex
                    : e.ItemIndex + 1;

            if (targetIndex != srcIndex && targetIndex != srcIndex + 1)
            {
                if (srcIndex < targetIndex)
                    this.tabItems.Move(srcIndex, targetIndex - 1);
                else
                    this.tabItems.Move(srcIndex, targetIndex);
            }
        }


        void OnListBoxDoubleClickOnItem(object? sender, ListBoxItemEventArgs e)
        {
            _ = new MessageDialog()
            {
                Message = $"Double-clicked on {e.Item} at position {e.ItemIndex}",
            }.ShowDialog(this);
        }


        void OnTabItemDragged(object? sender, TabItemDraggedEventArgs e)
        {
            var data = new DataObject();
            data.Set(TabItemKey, e.Item);
            DragDrop.DoDragDrop(e.PointerEventArgs, data, DragDropEffects.Move);
            e.Handled = true;
        }


        void RestartApp()
        {
            this.Application.Restart(AppSuiteApplication.RestoreMainWindowsArgument);
        }


        async void ShowAppInfoDialog()
        {
            this.canShowAppInfo.Update(false);
            using var appInfo = new AppInfo();
            await new ApplicationInfoDialog(appInfo).ShowDialog(this);
            this.canShowAppInfo.Update(true);
        }

        ICommand ShowAppInfoDialogCommand { get; }

        async void ShowTestDialog()
        {
            var result = await new Dialog().ShowDialog<ApplicationOptionsDialogResult>(this);
            if (result == ApplicationOptionsDialogResult.RestartMainWindowsNeeded)
                this.Application.RestartMainWindows();
        }


        void SwitchTheme()
        {
            this.Settings.SetValue<ThemeMode>(SettingKeys.ThemeMode, this.Settings.GetValueOrDefault(SettingKeys.ThemeMode) switch
            {
                ThemeMode.System => ThemeMode.Dark,
                ThemeMode.Dark => ThemeMode.Light,
                _ => this.Application.IsSystemThemeModeSupported ? ThemeMode.System : ThemeMode.Dark,
            });
        }


        void Test()
        {
            this.integerTextBox2.IsNullValueAllowed = !this.integerTextBox2.IsNullValueAllowed;

            //this.integerTextBox.Value = 0;
            //this.integerTextBox.Text = "0";
            //this.integerTextBox.Value = 1234;
            //this.integerTextBox.Text = "1234";

            //this.ipAddressTextBox.Text = "127.0.0.1";
            //this.ipAddressTextBox.IPAddress = System.Net.IPAddress.Parse("127.0.0.1");
            //this.ipAddressTextBox.Text = "192.168.0.1";
            //this.ipAddressTextBox.IPAddress = System.Net.IPAddress.Parse("192.168.0.1");

            //this.Settings.SetValue<bool>(SettingKeys.ShowProcessInfo, !this.Settings.GetValueOrDefault(SettingKeys.ShowProcessInfo));
        }


        void Test2()
        {
            this.Settings.SetValue<ApplicationCulture>(SettingKeys.Culture, this.Settings.GetValueOrDefault(SettingKeys.Culture) switch
            {
                ApplicationCulture.System => ApplicationCulture.EN_US,
                ApplicationCulture.EN_US => ApplicationCulture.ZH_TW,
                _ => ApplicationCulture.System,
            });
            /*
            this.Application.ShowMainWindow(window =>
            {
                System.Diagnostics.Debug.WriteLine("Window created");
            });
            */
        }
    }
}
