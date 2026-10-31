using System.Windows;
using MasterShare.Models;

namespace MasterShare;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        TopBar.Attach(this);
        ServerSidebar.RoomSelected += OnRoomSelected;
        CallControls.MicrophoneChanged += (_, enabled) =>
            ServerSidebar.SetLocalMicrophoneState(enabled);
        CallControls.ShareRequested += (_, _) =>
            MessageBox.Show("O módulo de captura e transmissão será conectado na próxima etapa.", "MASTER SHARE",
                MessageBoxButton.OK, MessageBoxImage.Information);
        CallControls.LeaveRequested += (_, _) =>
            MessageBox.Show("Você ainda não está conectado a uma sala remota.", "MASTER SHARE",
                MessageBoxButton.OK, MessageBoxImage.Information);
        CallControls.SettingsRequested += (_, _) =>
            MessageBox.Show("As configurações de transmissão serão adicionadas na próxima etapa.", "MASTER SHARE",
                MessageBoxButton.OK, MessageBoxImage.Information);
        StreamingArea.FullscreenRequested += (_, _) => ToggleFullscreen();
    }

    private void OnRoomSelected(object? sender, RoomInfo room)
    {
        RoomTitle.Text = room.Name;
        ViewerCount.Text = $"  •  {room.MemberCount} participantes";
        StreamingArea.SetRoom(room);
    }

    private void ToggleFullscreen()
    {
        if (WindowStyle == WindowStyle.None && ResizeMode == ResizeMode.NoResize && WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            ResizeMode = ResizeMode.CanResize;
            TopBar.Visibility = Visibility.Visible;
        }
        else
        {
            TopBar.Visibility = Visibility.Collapsed;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
        }
    }
}
