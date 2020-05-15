using SpectrumNextTools.Library.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace SpecNextTiler.ViewModel
{
    public class TileSourceImage : ViewModelBase
    {
        private WriteableBitmap sourceImage;
        private List<Tile> tiles;

        public TileSourceImage()
        {
            // Test image for binding
            SourceImage = BitmapFactory.New(512, 512);
            // Closed green polyline with P1(10, 5), P2(20, 40), P3(30, 30) and P4(7, 8)
            int[] p = new int[] { 10, 5, 20, 40, 30, 30, 7, 8, 10, 5 };
            SourceImage.DrawPolyline(p, Colors.Green);
        }

        public WriteableBitmap SourceImage
        {
            get => sourceImage;
            set => SetProperty(ref sourceImage, value);
        }

        // need to load image from file

        public async Task LoadImageFromFileAsync(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"The supplied file does not exist: {path}", nameof(path));
            }

            using (var fs = File.OpenRead(path))
            {
                await OpenFromStream(fs);
            }
        }

        public async Task LoadImageFromFileAsync(StorageFile storageFile)
        {
            // https://docs.microsoft.com/en-us/windows/uwp/audio-video-camera/imaging
            using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                {
                    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }

                WriteableBitmap bitmap = new WriteableBitmap(softwareBitmap.PixelWidth, softwareBitmap.PixelHeight);
                try
                {
                    softwareBitmap.CopyToBuffer(bitmap.PixelBuffer);
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }

                // Set the source of the Image control
                SourceImage = bitmap;
                Tileate();
            }


            //using (var fs = await storageFile.OpenStreamForReadAsync())
            //{
            //    await OpenFromStream(fs);
            //}
        }

        private void Tileate()
        {
            byte[] pixels = SourceImage.PixelBuffer.ToArray();
            // pixels are in BGRA
            this.tiles = new List<Tile>();

            // there are 4 bytes per pixel...
            var tileWidth = SourceImage.PixelWidth / 8;
            var tileHeight = SourceImage.PixelHeight / 8;
            for (var column = 0; column < tileWidth; column++)
            {
                for (var row = 0; row < tileHeight; column++)
                {
                    var startX = column * 8 * 4;
                    var startY = row * 8;
                    var tileBytes = new byte[8, 8];
                    for (int y = 0; y < 8; y++)
                        for (int x = 0; x < 8; x++)
                        {
                            tileBytes[y, x] = pixels[(startY * SourceImage.PixelWidth) + y + startX + x];
                        }
                    tiles.Add(new Tile(tileBytes));
                }
            }
        }

        private async Task OpenFromStream(Stream s)
        {
            SourceImage = await BitmapFactory.FromStream(s, BitmapPixelFormat.Rgba8);
        }

        // need to get palette from image

        // need to palettize the image
    }
}
