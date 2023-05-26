using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reflection.Metadata;

using ReactiveUI;
using System.Runtime.CompilerServices;
using YANSMBWE.U8;

namespace YANSMBWE
{
    public partial class ArcExplorer : Window
    {
        ArcExplorerViewModel ViewModel { get; set; } = new ArcExplorerViewModel();

        public ArcExplorer(string? filePath = null)
        {
            if (filePath is not null)
                OpenFile(filePath);

            InitializeComponent();

            Show();
        }
        public ArcExplorer() : this(null) { } //Avalonia

        public void OpenFile(string filePath)
        {
            ViewModel = new ArcExplorerViewModel(filePath);
            DataContext = ViewModel;
        }
    }
}