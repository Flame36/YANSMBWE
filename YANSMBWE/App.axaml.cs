using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System.IO;
using System.Diagnostics;
using Avalonia.Controls;

namespace YANSMBWE
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                throw new UnreachableException();

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                throw new UnreachableException();

            Window mainWindow = new Startup();
            desktop.MainWindow = mainWindow;

            base.OnFrameworkInitializationCompleted();
        }
    }
}