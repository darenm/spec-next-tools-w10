using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            //using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
            //{
            //    // Create the decoder from the stream
            //    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

            //    // Get the SoftwareBitmap representation of the file
            //    var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            //}
            using (var fs = await storageFile.OpenStreamForReadAsync())
            {
                await OpenFromStream(fs);
            }
        }

        private async Task OpenFromStream(Stream s)
        {
            SourceImage = await BitmapFactory.FromStream(s, Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8);
        }

        // need to get palette from image

        // need to palettize the image
    }
}
