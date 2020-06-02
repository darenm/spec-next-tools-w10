using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using SpecNextTiler.Models;
using SpectrumNextTools.Library.Models;
using Template10.Mvvm;
using Template10.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;

namespace SpecNextTiler.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        private bool _isImageLoaded;
        public bool IsImageLoaded
        {
            get => _isImageLoaded;
            set => SetProperty(ref _isImageLoaded, value);
        }

        private bool _isOneScreenOnly;
        public bool IsOneScreenOnly
        {
            get => _isOneScreenOnly;
            set => SetProperty(ref _isOneScreenOnly, value);
        }

        private TileSourceImage _tileSourceImage;
        public TileSourceImage TileSourceImage
        {
            get => _tileSourceImage;
            set => SetProperty(ref _tileSourceImage, value);
        }

        private LogViewModel _logView;
        public LogViewModel LogView
        {
            get => _logView;
            set => SetProperty(ref _logView, value);
        }

        private ObservableCollection<WinSpecColor> colors = new ObservableCollection<WinSpecColor>(SpecBase256Palette.Colors);

        public MainPageViewModel()
        {
            TileSourceImage = new TileSourceImage();
            LogView = new LogViewModel();
        }

        public ObservableCollection<WinSpecColor> Colors
        {
            get => this.colors;
            set => SetProperty(ref this.colors, value);
        }

        private bool _isPaletteMapped;
        public bool IsPaletteMapped
        {
            get => _isPaletteMapped;
            set => SetProperty(ref _isPaletteMapped, value);
        }

        public CoreDispatcher Dispatcher { get; internal set; }

        public async void OpenTileImage()
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    LogView.AddLogEntry($"Opening {file.DisplayName}");
                    // Application now has read/write access to the picked file
                    TileSourceImage = new TileSourceImage();
                    await TileSourceImage.LoadImageFromFileAsync(file);

                    LogView.AddLogEntry($"{TileSourceImage.Tiles.Count} tile{(TileSourceImage.Tiles.Count == 1 ? string.Empty : "s")}");
                    LogView.AddLogEntry($"Tile Map width: {TileSourceImage.TileMap.Width}, height: {TileSourceImage.TileMap.Height}");
                    LogView.AddLogEntry($"Tile Map size: {TileSourceImage.TileMap.Width * TileSourceImage.TileMap.Height * 2}Kb");
                    var index = 0;
                    foreach (var color in TileSourceImage.SortedColors)
                    {
                        Colors[index++] = color;
                    }
                    while (index < 256)
                    {
                        Colors[index++] = new WinSpecColor();
                    }

                    IsImageLoaded = true;

                    GeneratePalette();
                });
            }

        }

        public void CopyTiles()
        {
            DataPackage dataPackage = new DataPackage();
            var sb = new StringBuilder();
            for (var tileIndex = 0; tileIndex < TileSourceImage.Tiles.Count; tileIndex++)
            {
                sb.AppendLine($"; {tileIndex:X2}");

                for (int row = 0; row < 8; row++)
                {
                    var tile = TileSourceImage.Tiles[tileIndex];
                    sb.AppendLine($"    db ${tile.ExportBytes[row, 0]:X2}, ${tile.ExportBytes[row, 1]:X2}, ${tile.ExportBytes[row, 2]:X2}, ${tile.ExportBytes[row, 3]:X2}");
                }

                sb.AppendLine();
            }

            dataPackage.SetText(sb.ToString());
            Clipboard.SetContent(dataPackage);
        }

        public void CopyMap()
        {
            // 1 screen is 32 rows of 40 tiles
            DataPackage dataPackage = new DataPackage();
            var sb = new StringBuilder();
            var rowOffset = 0;

            var maxRows = IsOneScreenOnly ? 32 : TileSourceImage.TileMap.Height;
            var maxColumns = IsOneScreenOnly ? 42 : TileSourceImage.TileMap.Width;

            for (int row = 0 + rowOffset; row < maxRows + rowOffset; row++)
            {
                var firstItem = true;
                sb.Append("    db ");
                for (int column = 0; column < maxColumns; column++)
                {
                    if (firstItem)
                    {
                        firstItem = false;
                    }
                    else
                    {
                        sb.Append("  ,");
                    }
                    var tileMapEntry = TileSourceImage.TileMap[row, column];
                    var tile = TileSourceImage.Tiles[tileMapEntry.Index];
                    var extendedAttribute = FormatExtendedByte(tile.MatchedPaletteId, tileMapEntry.Orientation, tileMapEntry.Index);
                    var eightBitTileIndex = tileMapEntry.Index & 0xFF;
                    sb.Append($"${eightBitTileIndex:X2},${extendedAttribute:X2}");
                }
                sb.AppendLine();
            }


            dataPackage.SetText(sb.ToString());
            Clipboard.SetContent(dataPackage);

            byte FormatExtendedByte(int paletteId, TileOrientation orientation, int tileIndex)
            {
                byte extendedByte = 0;

                extendedByte += (byte)((paletteId & 0b0000_1111) << 4);
                if (orientation.MirrorX)
                {
                    extendedByte += 0b0000_1000;
                }

                if (orientation.MirrorY)
                {
                    extendedByte += 0b0000_0100;
                }

                if (orientation.Rotate)
                {
                    extendedByte += 0b0000_0010;
                }

                if (tileIndex > 255)
                {
                    extendedByte += 1;
                }

                return extendedByte;
            }
        }

        public void GeneratePalette()
        {
            // get the palette for each tile
            var palettes = new List<List<SpecColor>>();

            List<Tile> tiles = TileSourceImage.Tiles;
            foreach (var (tile, tilePalette) in
            // create the minimum number of 16 color palettes that support the tiles
            from tile in tiles
            let tilePalette = tile.GetPaletteFromTile()
            select (tile, tilePalette))
            {
                tile.IsPaletteMatched = false;
                for (int paletteIndex = 0; paletteIndex < palettes.Count; paletteIndex++)
                {
                    var palette = palettes[paletteIndex];
                    var missingColors = tilePalette.Except(palette).ToList();
                    if (missingColors.Any())
                    {
                        if (ListFits(palette, missingColors))
                        {
                            palette.AddRange(missingColors);
                            tile.MatchedPaletteId = paletteIndex;
                            tile.IsPaletteMatched = true;
                            break; // no need to look at another palette
                        }

                        // otherwise we check the next palette
                    }
                    else
                    {
                        tile.IsPaletteMatched = true;
                        tile.MatchedPaletteId = paletteIndex;
                    }
                }

                if (!tile.IsPaletteMatched)
                {
                    // we must create a new palette and add the **entire** tile palette
                    var newPalette = new List<SpecColor>
                    {
                        // every new palette has to have a transparent color at zero
                        // so we will add BLACK
                        new SpecColor(0, 0, 0)
                    };
                    newPalette.AddRange(tilePalette);
                    palettes.Add(newPalette);
                    tile.IsPaletteMatched = true;
                    tile.MatchedPaletteId = palettes.Count - 1;
                }
            }

            palettes.ForEach(p => p.Sort());

            LogView.AddLogEntry($"{palettes.Count} palette{(palettes.Count == 1 ? string.Empty : "s")}");

            // create palette color mapping for each tile
            tiles.ForEach(t => t.AssignPalette(palettes[t.MatchedPaletteId]));

            // Update displayed palette
            var index = 0;
            palettes.ForEach(p => p.ForEach(c => { if (index < 256) Colors[index++] = WinSpecColor.ConvertFrom(c); }));
            while (index < 256)
            {
                Colors[index++] = new WinSpecColor();
            }

            bool ListFits(List<SpecColor> palette, List<SpecColor> missingColors)
            {
                return palette.Count + missingColors.Count <= 16;
            }

            IsPaletteMapped = true;
        }
    }
}
