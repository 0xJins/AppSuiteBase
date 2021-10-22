using Avalonia;
using Avalonia.Markup.Xaml;
using CarinaStudio.Controls;
using CarinaStudio.Threading;
using CarinaStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarinaStudio.AppSuite.Tests
{
    public class App : AppSuiteApplication
    {
        protected override bool AllowMultipleMainWindows => true;


        public override void Initialize()
        {
            this.Name = "AppSuite";
            AvaloniaXamlLoader.Load(this);
        }


        static void Main(string[] args)
        {
            BuildApplication<App>().StartWithClassicDesktopLifetime(args);
        }


        protected override Window OnCreateMainWindow(object? param) => new MainWindow();


        protected override ViewModel OnCreateMainWindowViewModel(object? param) => new Workspace();


        protected override void OnNewInstanceLaunched(IDictionary<string, object> launchOptions)
        {
            base.OnNewInstanceLaunched(launchOptions);
            this.ShowMainWindow();
        }


        protected override async Task OnPrepareStartingAsync()
        {
            await base.OnPrepareStartingAsync();

            this.ShowMainWindow();

            this.SynchronizationContext.PostDelayed(() =>
            {
                //for (var i = 0; i < 1; ++i)
                    //this.ShowMainWindow();
                //splashWindow.Close();
            }, 5000);
        }


        protected override bool OnSelectEnteringDebugMode()
        {
#if DEBUG
            return true;
#else
            return base.OnSelectEnteringDebugMode();
#endif
        }


        public override System.Uri? PackageManifestUri => new Uri("https://raw.githubusercontent.com/carina-studio/ULogViewer/master/PackageManifest.json");
    }
}
