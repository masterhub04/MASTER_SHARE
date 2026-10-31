using System;
using System.IO;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace MasterShare;

public partial class MainWindow : Window
{
    private bool _isMaximized;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await Browser.EnsureCoreWebView2Async();
            Browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            Browser.CoreWebView2.Settings.IsStatusBarEnabled = false;
            Browser.CoreWebView2.WebMessageReceived += Browser_WebMessageReceived;

            var htmlPath = Path.Combine(AppContext.BaseDirectory, "HTML", "index.html");
            if (!File.Exists(htmlPath))
            {
                MessageBox.Show($"Não encontrei a interface do MASTER SHARE em:\n{htmlPath}",
                    "MASTER SHARE", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            Browser.CoreWebView2.Navigate(new Uri(htmlPath).AbsoluteUri);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Não foi possível iniciar o WebView2.\n\n" + ex.Message,
                "MASTER SHARE", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
    }

    private void Browser_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            using var json = System.Text.Json.JsonDocument.Parse(e.WebMessageAsJson);
            var root = json.RootElement;
            if (!root.TryGetProperty("action", out var action) || action.GetString() != "window")
                return;
            if (!root.TryGetProperty("command", out var command))
                return;

            switch (command.GetString())
            {
                case "minimize":
                    WindowState = WindowState.Minimized;
                    break;
                case "maximize":
                    _isMaximized = !_isMaximized;
                    WindowState = _isMaximized ? WindowState.Maximized : WindowState.Normal;
                    break;
                case "close":
                    Close();
                    break;
            }
        }
        catch
        {
            // Mensagens inválidas da interface são ignoradas.
        }
    }
}