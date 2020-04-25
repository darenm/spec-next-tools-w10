using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using SpecNextTiler.Models;

namespace SpecNextTiler.ViewModel.Design
{
    public class MainPageViewModel : BindableBase, IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Title = "Hello design-time world.";
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
