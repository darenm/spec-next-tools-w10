using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using SpecNextTiler.Models;
using Template10.Mvvm;
using Template10.Navigation;

namespace SpecNextTiler.ViewModel
{
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
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

        private ObservableCollection<SpecColor> colors = new ObservableCollection<SpecColor>(SpecBase256Palette.Colors);
        public ObservableCollection<SpecColor> Colors
        {
            get => this.colors;
            set => SetProperty(ref this.colors, value);
        }
    }
}
