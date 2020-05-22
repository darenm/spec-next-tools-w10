using SpectrumNextTools.Library.Models;
using SpectrumNextTools.Library.Palettes;
using System;
using Template10.Mvvm;
using Windows.UI;

namespace SpecNextTiler.Models
{
    public class WinSpecColor : SpecColor
    {
        public Color WinColor
        {
            get
            {
                return Color.FromArgb(255, R, G, B);
            }
        }


        public WinSpecColor(Color c) : base(c.R, c.G, c.B)
        {
        }

        public WinSpecColor(byte r, byte g, byte b) : base(r, g, b)
        {
        }

        public WinSpecColor(byte specColor) : base(specColor)
        {
        }

        public WinSpecColor() : base()
        {
        }

        public override string ToString()
        {
            return $"RGB: #{R:X2}{G:X2}{B:X2} SPEC: #{EightBitColor:X2} #{NineBitColor:X2}";
        }

        public static WinSpecColor ConvertFrom(SpecColor s)
        {
            return new WinSpecColor(s.EightBitColor);
        }
    }
}
