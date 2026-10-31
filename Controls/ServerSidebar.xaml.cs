using System;
using System.Windows;
using System.Windows.Controls;
using MasterShare.Models;

namespace MasterShare.Controls;
public partial class ServerSidebar : UserControl
{
    public event EventHandler<RoomInfo>? RoomSelected;
    public event EventHandler? SettingsRequested;
    private readonly RoomInfo[] _rooms =
    {
        new("Geral", 1, "◖"), new("Jogos", 0, "◖"), new("Privado", 0, "▣")
    };
    public ServerSidebar()
    {
        InitializeComponent();
        foreach (var room in _rooms)
        {
            var button = new Button { HorizontalContentAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(0, 2, 0, 2), Padding = new Thickness(10, 9, 10, 9), Tag = room };
            var panel = new DockPanel();
            var count = new TextBlock { Text = room.MemberCount.ToString(), Foreground = (System.Windows.Media.Brush)FindResource("MutedBrush"), FontSize = 11 };
            DockPanel.SetDock(count, Dock.Right);
            panel.Children.Add(count);
            panel.Children.Add(new TextBlock { Text = $"{room.Icon}   {room.Name}", Foreground = (System.Windows.Media.Brush)FindResource("TextBrush"), FontSize = 12, VerticalAlignment = VerticalAlignment.Center });
            button.Content = panel;
            button.Click += Room_Click;
            RoomButtons.Children.Add(button);
        }
        SelectRoom(_rooms[0]);
    }
    private void Room_Click(object sender, RoutedEventArgs e) { if (((Button)sender).Tag is RoomInfo room) SelectRoom(room); }
    private void SelectRoom(RoomInfo room)
    {
        foreach (Button b in RoomButtons.Children)
            b.Background = Equals(b.Tag, room) ? (System.Windows.Media.Brush)FindResource("AccentBrush") : System.Windows.Media.Brushes.Transparent;
        RoomSelected?.Invoke(this, room);
    }
    public void SetLocalMicrophoneState(bool enabled) { }
    private void Settings_Click(object sender, RoutedEventArgs e) => SettingsRequested?.Invoke(this, EventArgs.Empty);
}
