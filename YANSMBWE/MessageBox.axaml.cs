using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YANSMBWE;

public partial class MessageBox : Window
{
    public string Message { get; set; }
    public List<string> Buttons { get; set; }

    public event EventHandler? ButtonPressed;
    public string? Choice { get; set; } = null;

    public MessageBox(string Title, string Message, List<string> Buttons)
    {
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

        Show();
    }

    public MessageBox() : this("", "", new()) { }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        Focus();
    }

    private void OnButtonPressed(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button button)
            if (button.Tag is MessageBoxButton messageBoxButton)
            {
                Choice = messageBoxButton.ButtonText;
                ButtonPressed?.Invoke(this, EventArgs.Empty);
                Trace.WriteLine($"invoked {Choice}");
                Close();
            }
    }
}