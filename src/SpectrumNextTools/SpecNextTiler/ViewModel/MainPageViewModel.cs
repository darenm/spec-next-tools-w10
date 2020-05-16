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


        private ObservableCollection<SpecColor> colors = new ObservableCollection<SpecColor>(SpecBase256Palette.Colors);

        public MainPageViewModel()
        {
            TileSourceImage = new TileSourceImage();
        }

        public ObservableCollection<SpecColor> Colors
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
    }
}
