using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleSpecNextTiler.ViewModels;
using SpecNextTiler.Models;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SpecNextTiler.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
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
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                TileSourceImage = new TileSourceImage();
                await TileSourceImage.LoadImageFromFileAsync(file);
            }

        }
    }
}
