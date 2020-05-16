using SpectrumNextTools.Library.Models;
using SpectrumNextTools.Library.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpecNextTiler.Controls
{
    public sealed partial class TileControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TileProperty = DependencyProperty.Register(
            "Tile",
            typeof(Tile),
            typeof(TileControl),
            new PropertyMetadata(0.0, OnTileChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        private WriteableBitmap sourceImage;
        public WriteableBitmap SourceImage
        {
            get => sourceImage;
            set => SetProperty(ref sourceImage, value);
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (property == null || !property.Equals(value))
            {
                property = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Tile Tile
        {
            get { return (Tile)GetValue(TileProperty); }
            set { SetValue(TileProperty, value); }
        }

        private static void OnTileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Redraw trail, rotate needle, and update value text.
            // ...

            if (d is TileControl tc && e.NewValue is Tile)
            {
                tc.RenderTile();
            }
        }

        public TileOrientation Orientation
        {
            get { return (TileOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                "Orientation",
                typeof(TileOrientation),
                typeof(TileControl),
                new PropertyMetadata(TileOrientation.Default(), OnOrientationChanged));

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TileControl tc && e.NewValue is TileOrientation orientation)
            {
                tc.RenderTile();
            }
        }



        public bool ApplyOrientation
        {
            get { return (bool)GetValue(ApplyOrientationProperty); }
            set { SetValue(ApplyOrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ApplyOrientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ApplyOrientationProperty = DependencyProperty.Register("ApplyOrientation",
            typeof(bool),
            typeof(TileControl),
            new PropertyMetadata(true, OnApplyOrientationChanged));

        private static void OnApplyOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TileControl tc)
            {
                tc.RenderTile();
            }
        }

        public TileControl()
        {
            this.InitializeComponent();
        }

        private void RenderTile()
        {
            if (Tile == null)
            {
                return;
            }

            WriteableBitmap bitmap = new WriteableBitmap(8, 8);
            using (var context = bitmap.GetBitmapContext())
                for (int column = 0; column < 8; column++)
                {
                    for (int row = 0; row < 8; row++)
                    {
                        SpectrumColorConverter.ConvertToBgra(Tile.Pixels[row, column],
                                                             out byte a,
                                                             out byte r,
                                                             out byte g,
                                                             out byte b);
                        bitmap.SetPixel(column, row, Color.FromArgb(a, r, g, b));
                    }
                }

            if (ApplyOrientation)
            {
                if (Orientation.Rotate)
                {
                    bitmap = bitmap.Rotate(90);
                }
                if (Orientation.MirrorX)
                {
                    bitmap = bitmap.Flip(WriteableBitmapExtensions.FlipMode.Vertical);
                }
                if (Orientation.MirrorY)
                {
                    bitmap = bitmap.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);
                }
            }

            SourceImage = bitmap;
        }
    }
}
