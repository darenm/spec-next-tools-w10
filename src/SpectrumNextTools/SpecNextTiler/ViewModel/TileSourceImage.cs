using SpecNextTiler.Models;
using SpectrumNextTools.Library.Models;
using SpectrumNextTools.Library.Palettes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public TileSourceImage()
        {
            // Test image for binding
            SourceImage = BitmapFactory.New(512, 512);
        }

        public WriteableBitmap SourceImage
        {
            get => sourceImage;
            set => SetProperty(ref sourceImage, value);
        }
        public List<Tile> Tiles { get; private set; }
        public TileMap TileMap { get; private set; }
        public List<WinSpecColor> SortedColors { get; private set; }

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
                softwareBitmap.CopyToBuffer(bitmap.PixelBuffer);

                // Set the source of the Image control
                SourceImage = bitmap;
                Tileate();
                DeDupTiles();
                ExtractPalette();
            }
        }

        private void ExtractPalette()
        {
            var uniqueColors = new List<byte>();

            foreach (var tile in Tiles)
            {
                foreach (var pixel in tile.Pixels)
                {
                    if (!uniqueColors.Contains(pixel))
                    {
                        uniqueColors.Add(pixel);
                    }
                }
            }

            //var sortedColors = uniqueColors.OrderBy(b => b).ToArray();
            SortedColors = new List<WinSpecColor>(uniqueColors.Count);
            foreach (var color in uniqueColors)
            {
                SortedColors.Add(new WinSpecColor(color));
            }
        }

        private void DeDupTiles()
        {
            // move index to the tile to check for duplicates
            var currentTileIndex = 0;

            // if there are more tiles
            while(currentTileIndex < Tiles.Count -1)
            {
                var currentTile = Tiles[currentTileIndex];

                // iterate through the remaining tiles to find matches
                for (int i = currentTileIndex + 1; i < Tiles.Count; i++)
                {
                    var compareTile = Tiles[i];
                    if (currentTile.Match(compareTile, out TileOrientation orientation))
                    {
                        TileMap.ReplaceTilesByTileIndex(i, currentTileIndex, orientation);
                    }
                }
                currentTileIndex++;
            }

            // rebuild Tiles from the updated TileMap
            // Tile order is based upon order of incidence in TileMap
            // also have to  update the TileMap as we go to reference new
            // indexes

            var newTiles = new List<Tile>();

            for (int row = 0; row < TileMap.Height; row++)
            {
                for (int column = 0; column < TileMap.Width; column++)
                {
                    var tileMapEntry = TileMap[row, column];
                    if (tileMapEntry.Index >= newTiles.Count)
                    {
                        // we haven't added the tile to the new list
                        var newIndex = newTiles.Count;
                        var oldIndex = tileMapEntry.Index;
                        newTiles.Add(Tiles[tileMapEntry.Index]);
                        TileMap.UpdateTileIndex(oldIndex, newIndex);
                    }
                }
            }

            Tiles = newTiles;
        }

        private void Tileate()
        {
            byte[] pixels = SourceImage.PixelBuffer.ToArray();
            // pixels are in BGRA
            this.Tiles = new List<Tile>();

            // there are 4 bytes per pixel...
            var tileWidth = SourceImage.PixelWidth / 8;
            var tileHeight = SourceImage.PixelHeight / 8;
            TileMap = new TileMap(tileWidth, tileHeight);

            for (var row = 0; row < tileHeight; row++)
            {
                for (var column = 0; column < tileWidth; column++)
                {
                    var startX = column * 8 * 4;
                    var startY = row * 8 * SourceImage.PixelWidth * 4;
                    var offset = startX + startY;
                    var tileBytes = new byte[8, 8];
                    for (int y = 0; y < 8; y++)
                        for (int x = 0; x < 8 * 4; x += 4)
                        {
                            var localOffset = x + (y * SourceImage.PixelWidth * 4);
                            byte b = pixels[offset + localOffset];
                            byte g = pixels[offset + localOffset + 1];
                            byte r = pixels[offset + localOffset + 2];
                            // we ignore the a
                            // byte a = pixels[offset + localOffset + 3];
                            tileBytes[y, x / 4] = SpectrumColorConverter.EightBitFromBgr(b, g, r);
                        }

                    var tile = new Tile(tileBytes);
                    int index;
                    if (Tiles.Contains(tile))
                    {
                        index = Tiles.IndexOf(tile);
                    }
                    else
                    {
                        Tiles.Add(new Tile(tileBytes));
                        index = Tiles.Count - 1;
                    }
                    TileMap.PlaceTile(row, column, index);
                }
            }
        }

        private async Task OpenFromStream(Stream s)
        {
            SourceImage = await BitmapFactory.FromStream(s, BitmapPixelFormat.Rgba8);
        }
    }
}
