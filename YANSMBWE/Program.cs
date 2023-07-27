using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using YANSMBWE.Utils;

namespace YANSMBWE
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter(File.OpenWrite("logs.txt"))));
            Trace.AutoFlush = true;

            try
            {
                var a = BuildAvaloniaApp();
                a.StartWithClassicDesktopLifetime(args);//, ShutdownMode.OnMainWindowClose);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e, "Something very bad happened");
            }
            finally
            {
                Trace.Close();
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}