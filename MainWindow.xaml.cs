using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace MasterShare;

public partial class MainWindow : Window
{
    private bool _isMaximized;
    private static readonly HttpClient Http = CreateHttpClient();

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("MASTER-SHARE-Updater");
        client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
        return client;
    }

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

    private async void Browser_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            using var json = JsonDocument.Parse(e.WebMessageAsJson);
            var root = json.RootElement;
            if (!root.TryGetProperty("action", out var action))
                return;

            if (action.GetString() == "updates")
            {
                if (root.TryGetProperty("command", out var updateCommand) && updateCommand.GetString() == "check")
                    await CheckForUpdatesAsync();
                return;
            }

            if (action.GetString() != "window" || !root.TryGetProperty("command", out var command))
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
        catch (Exception ex)
        {
            MessageBox.Show("Não foi possível verificar atualizações.\n\n" + ex.Message,
                "MASTER SHARE", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async Task CheckForUpdatesAsync()
    {
        var currentVersion = typeof(MainWindow).Assembly.GetName().Version ?? new Version(0, 0, 0);
        using var response = await Http.GetAsync("https://api.github.com/repos/masterhub04/MASTER_SHARE/releases/latest");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            MessageBox.Show("Ainda não existe uma versão publicada para atualização.\n\nQuando uma nova versão for lançada, ela aparecerá aqui.",
                "MASTER SHARE", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var release = document.RootElement;
        var tag = release.GetProperty("tag_name").GetString() ?? "";
        var versionText = tag.TrimStart('v', 'V');
        if (!Version.TryParse(versionText, out var latestVersion))
            throw new InvalidOperationException("A versão publicada no GitHub está com uma identificação inválida.");

        if (latestVersion <= currentVersion)
        {
            MessageBox.Show($"Você já está usando a versão mais recente do MASTER SHARE.\n\nVersão instalada: {currentVersion.ToString(3)}",
                "MASTER SHARE", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var asset = release.GetProperty("assets").EnumerateArray()
            .FirstOrDefault(item =>
            {
                var name = item.GetProperty("name").GetString() ?? "";
                return name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                    && name.StartsWith("MASTER_SHARE_Setup_", StringComparison.OrdinalIgnoreCase);
            });

        if (asset.ValueKind == JsonValueKind.Undefined)
            throw new InvalidOperationException("A nova versão foi encontrada, mas o instalador ainda não está anexado à publicação.");

        var downloadUrl = asset.GetProperty("browser_download_url").GetString();
        if (string.IsNullOrWhiteSpace(downloadUrl))
            throw new InvalidOperationException("O GitHub não informou o endereço do instalador.");

        var result = MessageBox.Show(
            $"Uma nova versão do MASTER SHARE está disponível!\n\nInstalada: {currentVersion.ToString(3)}\nNova versão: {latestVersion.ToString(3)}\n\nDeseja baixar e instalar agora?",
            "Atualização disponível", MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (result != MessageBoxResult.Yes)
            return;

        var installerPath = Path.Combine(Path.GetTempPath(), $"MASTER_SHARE_Update_{latestVersion}.exe");
        using (var download = await Http.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
        {
            download.EnsureSuccessStatusCode();
            await using var file = File.Create(installerPath);
            await download.Content.CopyToAsync(file);
        }

        Process.Start(new ProcessStartInfo(installerPath) { UseShellExecute = true });
        Application.Current.Shutdown();
    }
}
