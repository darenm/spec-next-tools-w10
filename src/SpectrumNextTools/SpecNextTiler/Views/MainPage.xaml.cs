using Microsoft.Toolkit.Uwp.UI.Controls;
using SpecNextTiler.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SpecNextTiler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel => DataContext as MainPageViewModel;
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += (s,e) => ViewModel.Dispatcher = this.Dispatcher;
        }

        private async void ShowTiles(object sender, RoutedEventArgs e)
        {
            ViewModel.LogView.AddLogEntry("Show Tiles");

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                WrapPanel.Children.Clear();

                WrapPanel.Width = 16 * 8;
                ViewModel.LogView.AddLogEntry($"Tiles: {ViewModel.TileSourceImage.Tiles.Count}");
                foreach (var tile in ViewModel.TileSourceImage.Tiles)
                {
                    var tc = new Controls.TileControl
                    {
                        Tile = tile
                    };
                    WrapPanel.Children.Add(tc);
                }
            });

        }

        private async void ShowTileMap(object sender, RoutedEventArgs e)
        {
            ViewModel.LogView.AddLogEntry("Show Tile Map");
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                WrapPanel.Children.Clear();

                var tileMap = ViewModel.TileSourceImage.TileMap;
                ViewModel.LogView.AddLogEntry($"Width: {tileMap.Width}, Height: {tileMap.Height}");
                WrapPanel.Width = tileMap.Width * 8;

                for (int row = 0; row < tileMap.Height; row++)
                {
                    for (int column = 0; column < tileMap.Width; column++)
                    {
                        var tc = new Controls.TileControl();
                        var tileMapEntry = tileMap[row, column];
                        tc.Tile = ViewModel.TileSourceImage.Tiles[tileMapEntry.Index];
                        tc.Orientation = tileMapEntry.Orientation;
                        WrapPanel.Children.Add(tc);
                    }
                }
            });
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                foreach (var child in WrapPanel.Children)
                {
                    if (child is Controls.TileControl tc)
                    {
                        tc.ApplyOrientation = tb.IsChecked ?? false;
                    }
                }
            }
        }
    }
}
