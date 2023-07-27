using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Dialogs;
using System.Threading.Tasks;
using Avalonia.Controls.Platform;
using Avalonia;
using System;
using System.Diagnostics;

namespace YANSMBWE
{
    public partial class ArcExplorer : Window
    {
        ArcExplorerViewModel ViewModel { get; set; }

        public ArcExplorer(string? filePath = null)
        {
            InitializeComponent();

            ViewModel = new ArcExplorerViewModel(filePath);
            DataContext = ViewModel;
        }
        public ArcExplorer() : this(null) { } //Avalonia

        private async void OpenFile(object sender, RoutedEventArgs e)
        {
            var dialogImpl = AvaloniaLocator.Current.GetService<ISystemDialogImpl>() ?? throw new Exception();
            Trace.WriteLine(dialogImpl.GetType());
            OpenFileDialog dialog = new();
            ManagedFileDialogOptions options = new()
            {
                AllowDirectorySelection = false,
            };

            string[] files = await dialog.ShowManagedAsync(this, options);
        }
    }
}