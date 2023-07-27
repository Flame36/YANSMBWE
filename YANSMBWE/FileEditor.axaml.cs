using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using YANSMBWE.Utils;

namespace YANSMBWE;

public partial class FileEditor : Window
{
    FileEditorDataContext dataContext;

    public FileEditor()
    {
        dataContext = new FileEditorDataContext();
        DataContext = dataContext;

        this.AttachDevTools();

        InitializeComponent();

    }

    public void CreateFile(FileType fileType)
    {
        throw new NotImplementedException();
    }

    public void OpenFile(string path)
    {
        FileEditorTab newTab = FileHandler.CreateNewFileTab(path);
        dataContext.Tabs.Add(newTab);
        dataContext.SelectedTab = newTab;
    }

    private void SelectTab(object sender, RoutedEventArgs e)
    {
        FileEditorTab? tab = sender.GetButtonTag<FileEditorTab>();
        if (tab is null)
            return;
        dataContext.SelectedTab = tab;
    }

    private void CloseTab(object sender, RoutedEventArgs e)
    {
        FileEditorTab? tab = sender.GetButtonTag<FileEditorTab>();
        if (tab is null)
            return;
        dataContext.CloseTab(tab);
    }
}