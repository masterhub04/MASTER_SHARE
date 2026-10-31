using System;
using System.Windows;
using System.Windows.Controls;

namespace MasterShare.Controls;
public partial class CallControls : UserControl
{
    public event EventHandler<bool>? MicrophoneChanged;
    public event EventHandler? ShareRequested;
    public event EventHandler? LeaveRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? FullscreenRequested;
    private bool _micEnabled = true;
    public CallControls() => InitializeComponent();
    private void Mic_Click(object sender, RoutedEventArgs e)
    {
        _micEnabled = !_micEnabled;
        MicButton.Opacity = _micEnabled ? 1 : 0.45;
        MicrophoneChanged?.Invoke(this, _micEnabled);
    }
    private void Share_Click(object sender, RoutedEventArgs e) => ShareRequested?.Invoke(this, EventArgs.Empty);
    private void Leave_Click(object sender, RoutedEventArgs e) => LeaveRequested?.Invoke(this, EventArgs.Empty);
    private void Settings_Click(object sender, RoutedEventArgs e) => SettingsRequested?.Invoke(this, EventArgs.Empty);
    private void Fullscreen_Click(object sender, RoutedEventArgs e) => FullscreenRequested?.Invoke(this, EventArgs.Empty);
}
