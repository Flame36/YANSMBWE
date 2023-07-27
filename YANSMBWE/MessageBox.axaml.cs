using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YANSMBWE;

public partial class MessageBox : Window
{
    public string Message { get; set; }
    public List<string> Buttons { get; set; }

    public MessageBox(string Title, string Message, List<string> Buttons)
    {
        CanResize = false;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.Message = Message;
        this.Buttons = Buttons;

        double width = 50;

        DataContext = this;
        SizeToContent = SizeToContent.WidthAndHeight;
        
        InitializeComponent();

        this.Title = Title;

        StackPanel buttonsPanel = this.FindControl<StackPanel>("ButtonsPanel");
        foreach (string buttonText in Buttons)
        {
            MessageBoxButton messageBoxButton = new(buttonText);
            ContentControl contentControl = new()
            {
                Content = messageBoxButton
            };
            width += messageBoxButton.Width;
            buttonsPanel.Children.Add(contentControl);
        }
    }
    public MessageBox() : this("", "", new()) { }

    private void OnButtonPressed(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
            if (button.Tag is MessageBoxButton messageBoxButton)
                Close(messageBoxButton.ButtonText);
    }
}