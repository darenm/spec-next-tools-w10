using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI;

namespace SpecNextTiler.Models
{
    public class PaletteColor : ViewModelBase
    {
        private Color realColor;
        private WinSpecColor specColor;

        public Color RealColor
        {
            get => this.realColor;
            set 
            {
                SetProperty(ref this.realColor, value);
                SpecColor = new WinSpecColor(value);
            }
        }

        public WinSpecColor SpecColor 
        { 
            get => specColor;
            private set => SetProperty(ref this.specColor, value);
        }
    }
}
