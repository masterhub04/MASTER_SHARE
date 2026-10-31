using System;
using System.Windows;
using System.Windows.Controls;
using MasterShare.Models;

namespace MasterShare.Controls;
public partial class StreamingArea : UserControl
{
    public event EventHandler? FullscreenRequested;
    public StreamingArea() => InitializeComponent();
    public void SetRoom(RoomInfo room) { }
}
