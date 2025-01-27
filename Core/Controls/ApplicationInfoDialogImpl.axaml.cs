using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using CarinaStudio.AppSuite.ViewModels;
using CarinaStudio.Collections;
using CarinaStudio.Configuration;
using CarinaStudio.Data.Converters;
using CarinaStudio.Threading;
using System;

namespace CarinaStudio.AppSuite.Controls
{
	/// <summary>
	/// Application info dialog.
	/// </summary>
	partial class ApplicationInfoDialogImpl : Dialog
	{
		// Static fields.
		static readonly IValueConverter AppReleasingTypeConverter = new Converters.EnumConverter(AppSuiteApplication.Current, typeof(ApplicationReleasingType));
		static readonly AvaloniaProperty<bool> HasApplicationChangeListProperty = AvaloniaProperty.Register<ApplicationInfoDialogImpl, bool>(nameof(HasApplicationChangeList));
		static readonly AvaloniaProperty<bool> HasTotalPhysicalMemoryProperty = AvaloniaProperty.Register<ApplicationInfoDialogImpl, bool>(nameof(HasTotalPhysicalMemory));
		static readonly SettingKey<bool> IsRestartingInDebugModeConfirmationShownKey = new("ApplicationInfoDialog.IsRestartingInDebugModeConfirmationShown");
		static readonly AvaloniaProperty<string?> VersionStringProperty = AvaloniaProperty.Register<ApplicationInfoDialogImpl, string?>(nameof(VersionString));


		// Constructor.
		public ApplicationInfoDialogImpl()
		{
			InitializeComponent();
			this.SetValue(HasTotalPhysicalMemoryProperty, this.Application.HardwareInfo.TotalPhysicalMemory.HasValue);
		}


		// Export application logs to file.
		async void ExportLogs()
		{
			// check state
			if (this.DataContext is not ApplicationInfo appInfo)
				return;

			// select file
			var fileName = await new SaveFileDialog().Also(it =>
			{
				var dateTime = DateTime.Now;
				it.Filters.Add(new FileDialogFilter().Also(filter =>
				{
					filter.Extensions.Add("zip");
					filter.Name = this.Application.GetString("FileFormat.Zip");
				}));
				it.InitialFileName = $"Logs-{dateTime.ToString("yyyyMMdd-HHmmss")}.zip";
			}).ShowAsync(this);
			if (fileName == null)
				return;

			// export
			var success = await appInfo.ExportLogs(fileName);

			// show result
			if (!this.IsOpened)
				return;
			if (success)
			{
				_ = new MessageDialog()
				{
					Icon = MessageDialogIcon.Success,
					Message = this.Application.GetString("ApplicationInfoDialog.SucceededToExportAppLogs"),
				}.ShowDialog(this);
			}
			else
			{
				_ = new MessageDialog()
				{
					Icon = MessageDialogIcon.Error,
					Message = this.Application.GetString("ApplicationInfoDialog.FailedToExportAppLogs"),
				}.ShowDialog(this);
			}
		}


		// Check whether application change list is available or not.
		public bool HasApplicationChangeList { get => this.GetValue<bool>(HasApplicationChangeListProperty); }


		// Check whether total physical memory info is valid or not.
		public bool HasTotalPhysicalMemory { get => this.GetValue<bool>(HasTotalPhysicalMemoryProperty); }


		// Initialize.
		private void InitializeComponent() => AvaloniaXamlLoader.Load(this);


		// Property changed.
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);
			if (change.Property == DataContextProperty)
			{
				if (change.NewValue.Value is ApplicationInfo appInfo)
				{
					// sync state
					this.Title = this.Application.GetFormattedString("ApplicationInfoDialog.Title", appInfo.Name);
					this.SetValue<string?>(VersionStringProperty, Global.Run(() =>
					{
						var str = this.Application.GetFormattedString("ApplicationInfoDialog.Version", appInfo.Version);
						if (appInfo.ReleasingType == ApplicationReleasingType.Stable)
							return str;
						return str + $" ({AppReleasingTypeConverter.Convert<string?>(appInfo.ReleasingType)})";
					}));

					// check change list
					this.SynchronizationContext.Post(async () =>
					{
						if (this.DataContext is not ApplicationInfo appInfo)
							return;
						await appInfo.ApplicationChangeList.WaitForChangeListReadyAsync();
						if (this.DataContext != appInfo)
							return;
						this.SetValue(HasApplicationChangeListProperty, appInfo.ApplicationChangeList.ChangeList.IsNotEmpty());
					});

					// show assemblies
					this.FindControl<Panel>("assembliesPanel")?.Let(panel =>
					{
						panel.Children.Clear();
						foreach (var assembly in appInfo.Assemblies)
						{
							if (panel.Children.Count > 0)
								panel.Children.Add(new Separator().Also(it => it.Classes.Add("Dialog_Separator_Small")));
							panel.Children.Add(new TextBlock() { Text = $"{assembly.GetName().Name} {assembly.GetName().Version}" });
						}
					});
				}
			}
        }


		// Restart in debug mode.
		async void RestartInDebugMode()
		{
			// check state
			if (this.Application.IsDebugMode)
				return;
			
			// show message
			if (!this.PersistentState.GetValueOrDefault(IsRestartingInDebugModeConfirmationShownKey))
			{
				await new MessageDialog()
				{
					Icon = MessageDialogIcon.Information,
					Message = this.Application.GetString("ApplicationInfoDialog.ConfirmRestartingInDebugMode"),
				}.ShowDialog(this);
				this.PersistentState.SetValue<bool>(IsRestartingInDebugModeConfirmationShownKey, true);
			}

			// restart
			this.Application.Restart($"{AppSuiteApplication.RestoreMainWindowsArgument} {AppSuiteApplication.DebugArgument}", this.Application.IsRunningAsAdministrator);
		}


		// Show change list of application.
		void ShowApplicationChangeList()
        {
			// check state
			if (this.DataContext is not ApplicationInfo appInfo)
				return;
			if (appInfo.ApplicationChangeList.ChangeList.IsEmpty())
				return;

			// show dialog
			_ = new ApplicationChangeListDialog(appInfo.ApplicationChangeList).ShowDialog(this);
		}


        // String represent version.
        string? VersionString { get => this.GetValue<string?>(VersionStringProperty); }
	}
}