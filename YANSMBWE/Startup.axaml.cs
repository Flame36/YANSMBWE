using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Styling;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace YANSMBWE
{
    public partial class Startup : Window
    {

        public Startup()
        {
            InitializeComponent();

            UpdateRecentFiles();

            Show();
        }

        //TODO: Update this to ListBox
        private void UpdateRecentFiles()
        {
            var recentFilesMenu = this.FindControl<StackPanel>("RecentFilesMenu");
            recentFilesMenu.Children.Clear();

            foreach (RecentFile recentFile in RecentFilesManager.RecentFiles)
            {
                //Trace.WriteLine($"{recentFile.Name} {recentFile.Path} {recentFile.LastAccessed}");
                ContentControl contentControl = new()
                {
                    Content = recentFile
                };
                recentFilesMenu.Children.Add(contentControl);
            }
        }

        private void OpenArcExplorer(object sender, RoutedEventArgs e)
        {
            if (Application.Current is null ||
                Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktopLifetime)
                throw new UnreachableException("Wrong application lifetime");

            var arcExplorer = new ArcExplorer();
            desktopLifetime.MainWindow = arcExplorer;
            Close();
        }

        private void OpenRecentFile(object sender, RoutedEventArgs e)
        {
            RecentFile? recentFile = null;
            if (sender is Button button)
                if (button.Tag is RecentFile file)
                    recentFile = file;

            if (recentFile is null)
                return;


            if (!File.Exists(recentFile.Path))
            {
                MessageBox messageBox = new("File not found", "Can't find the file you're trying to open\nRemove it from the recent files list?", new() { "Yes", "No" });
                messageBox.ButtonPressed +=
                    (object? sender, EventArgs e) =>
                    {
                        if (sender is MessageBox msgBox)
                            if (msgBox.Choice == "No")
                                return;
                        RecentFilesManager.RecentFiles.Remove(recentFile);
                        RecentFilesManager.SaveRecentFiles();
                        UpdateRecentFiles();
                    };
                return;
            }

            List<RecentFile> recentFiles = RecentFilesManager.RecentFiles;
            recentFiles.Remove(recentFile);
            recentFile.LastAccessed = DateTime.Now;
            recentFiles.Add(recentFile);

            RecentFilesManager.SaveRecentFiles();
            UpdateRecentFiles();

            try
            {
                if (Application.Current is null ||
                    Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktopLifetime)
                    throw new UnreachableException("Wrong application lifetime");

                desktopLifetime.MainWindow = FileHandler.OpenFileInNewWindow(recentFile.Path);
                Close();
            }
            catch (ArgumentException)
            {
                _ = new MessageBox("Unknown file type", "Unable to open the requested file type.", new() { "Ok" });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                _ = new MessageBox("Error opening file", "An error occurred while trying to open the file.", new() { "Ok" });
            }
        }
    }
}