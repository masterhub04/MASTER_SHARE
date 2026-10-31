using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MasterShare.Controls;
public partial class TopBar : UserControl
{
    private Window? _window;
    public TopBar() { InitializeComponent(); MouseLeftButtonDown += DragWindow; }
    public void Attach(Window window) => _window = window;
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 2)
            _window!.WindowState = _window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        else if (e.ButtonState == MouseButtonState.Pressed) _window?.DragMove();
    }
    private void Min_Click(object sender, RoutedEventArgs e) => _window!.WindowState = WindowState.Minimized;
    private void Max_Click(object sender, RoutedEventArgs e) => _window!.WindowState = _window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    private void Close_Click(object sender, RoutedEventArgs e) => _window?.Close();
}
