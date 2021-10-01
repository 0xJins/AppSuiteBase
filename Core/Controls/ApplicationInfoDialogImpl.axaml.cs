using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CarinaStudio.AppSuite.ViewModels;
using System;

namespace CarinaStudio.AppSuite.Controls
{
	/// <summary>
	/// Application info dialog.
	/// </summary>
	partial class ApplicationInfoDialogImpl : Dialog
	{
		// Static fields.
		static readonly AvaloniaProperty<bool> HasGitHubProjectProperty = AvaloniaProperty.Register<ApplicationInfoDialogImpl, bool>(nameof(HasGitHubProject));
		static readonly AvaloniaProperty<bool> HasPrivacyPolicyProperty = AvaloniaProperty.Register<ApplicationInfoDialogImpl, bool>(nameof(HasPrivacyPolicy));
		static readonly AvaloniaProperty<bool> HasUserAgreementProperty = AvaloniaProperty.Register<ApplicationInfoDialogImpl, bool>(nameof(HasUserAgreement));
		static readonly AvaloniaProperty<string?> VersionStringProperty = AvaloniaProperty.Register<ApplicationInfoDialogImpl, string?>(nameof(VersionString));


		// Constructor.
		public ApplicationInfoDialogImpl()
		{
			InitializeComponent();
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
					filter.Extensions.Add("txt");
					filter.Name = this.Application.GetString("FileFormat.Text");
				}));
				it.InitialFileName = $"Logs-{dateTime.ToString("yyyyMMdd-HHmmss")}.txt";
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
					Icon = MessageDialogIcon.Information,
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


		// Check whether GitHub exists or not.
		public bool HasGitHubProject { get => this.GetValue<bool>(HasGitHubProjectProperty); }


		// Check whether Privacy Policy exists or not.
		public bool HasPrivacyPolicy { get => this.GetValue<bool>(HasPrivacyPolicyProperty); }


		// Check whether User Agreement exists or not.
		public bool HasUserAgreement { get => this.GetValue<bool>(HasUserAgreementProperty); }


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
					this.Title = this.Application.GetFormattedString("ApplicationInfoDialog.Title", appInfo.Name);
					this.SetValue<bool>(HasGitHubProjectProperty, appInfo.GitHubProjectUri != null);
					this.SetValue<bool>(HasPrivacyPolicyProperty, appInfo.PrivacyPolicyUri != null);
					this.SetValue<bool>(HasUserAgreementProperty, appInfo.UserAgreementUri != null);
					this.SetValue<string?>(VersionStringProperty, this.Application.GetFormattedString("ApplicationInfoDialog.Version", appInfo.Version));
				}
			}
        }


        // String represent version.
        string? VersionString { get => this.GetValue<string?>(VersionStringProperty); }
	}
}