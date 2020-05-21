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

namespace SpecNextTiler.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private int _tileIndex = 0;

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Title = "Hello run-time world.";
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // empty
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isImageLoaded;
        public bool IsImageLoaded
        {
            get => _isImageLoaded;
            set => SetProperty(ref _isImageLoaded, value);
        }

        private TileSourceImage _tileSourceImage;
        public TileSourceImage TileSourceImage
        {
            get => _tileSourceImage;
            set => SetProperty(ref _tileSourceImage, value);
        }

        private Tile _tile;
        public Tile Tile
        {
            get => _tile;
            set => SetProperty(ref _tile, value);
        }


        private ObservableCollection<WinSpecColor> colors = new ObservableCollection<WinSpecColor>(SpecBase256Palette.Colors);

        public MainPageViewModel()
        {
            TileSourceImage = new TileSourceImage();
        }

        public ObservableCollection<WinSpecColor> Colors
        {
            get => this.colors;
            set => SetProperty(ref this.colors, value);
        }

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
                // Application now has read/write access to the picked file
                TileSourceImage = new TileSourceImage();
                await TileSourceImage.LoadImageFromFileAsync(file);
                var index = 0;
                foreach (var color in TileSourceImage.SortedColors)
                {
                    Colors[index++] = color;
                }
                while(index < 256)
                {
                    Colors[index++] = new WinSpecColor();
                }

                IsImageLoaded = true;
            }

        }

        public void ShowTileOne()
        {
            if (TileSourceImage.Tiles != null && TileSourceImage.Tiles.Count > 0)
            {
                if (_tileIndex >= TileSourceImage.Tiles.Count)
                {
                    _tileIndex = 0;
                }
                Tile = TileSourceImage.Tiles[_tileIndex];
                _tileIndex++;
            }
        }

        public void CopyTiles()
        {
            DataPackage dataPackage = new DataPackage();
            var sb = new StringBuilder();
            for (int i = 0; i < colors.Count; i++)
            {
                sb.AppendLine($"    db ${colors[i].EightBitColor:X2}, ${colors[i].NineBitColor:X2}    ; ({i:D2})  ({i:X2})");
            }
            dataPackage.SetText(sb.ToString());
            Clipboard.SetContent(dataPackage);
        }
    }
}
