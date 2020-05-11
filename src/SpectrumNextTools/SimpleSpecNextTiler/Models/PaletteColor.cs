using SimpleSpecNextTiler.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace SpecNextTiler.Models
{
    public class PaletteColor : ViewModelBase
    {
        private Color realColor;
        private SpecColor specColor;

        public Color RealColor
        {
            get => this.realColor;
            set 
            {
                SetProperty(ref this.realColor, value);
                SpecColor = new SpecColor(value);
            }
        }

        public SpecColor SpecColor 
        { 
            get => specColor;
            private set => SetProperty(ref this.specColor, value);
        }
    }
}
