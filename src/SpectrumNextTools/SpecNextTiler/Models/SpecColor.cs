using SpectrumNextTools.Library.Palettes;
using System;
using Template10.Mvvm;
using Windows.UI;

namespace SpecNextTiler.Models
{
    public class SpecColor : ViewModelBase, IComparable
    {
        private readonly byte r;
        private readonly byte g;
        private readonly byte b;

        public byte EightBitColor { get; set; }


        public Color WinColor
        {
            get
            {
                return Color.FromArgb(255, R, G, B);
            }
        }

        public byte R => r;

        public byte G => g;

        public byte B => b;

        public SpecColor()
        {
            EightBitColor = 0;
            r = b = g = 0;
        }

        public SpecColor(byte specColor)
        {
            EightBitColor = specColor;
            //r = (byte)(specColor & 0b111_000_00);
            //g = (byte)((specColor & 0b000_111_00) << 3);
            //b = (byte)((specColor & 0b111_000_00)<< 6);
            SpectrumColorConverter.ConvertToBgra(specColor, out _, out r, out g, out b);
        }

        public SpecColor(byte r, byte g, byte b)
        {
            this.r = r;
            this.b = b;
            this.g = g;

            //EightBitColor = (byte)
            //    ((r & 0b111_000_00) +
            //    ((g >> 3) & 0b000_111_00) +
            //    ((b >> 6) & 0b000_000_11));
            EightBitColor = SpectrumColorConverter.ConvertFromBgr(b, g, r);
        }

        public SpecColor(Color c) : this(c.R, c.G, c.B)
        {
        }

        public override string ToString()
        {
            return $"RGB: #{R:X2}{G:X2}{B:X2} SPEC: #{EightBitColor:X2}";
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is SpecColor otherSpecColor)
            {
                var thisScore = b + (g << 3) + (r << 6);
                var otherScore = otherSpecColor.b + (otherSpecColor.g << 3) + (otherSpecColor.r << 6);

                if (thisScore < otherScore)
                {
                    return -1;
                }
                else if (thisScore == otherScore)
                {
                    return 0;
                }
                else // thisScore > otherScore
                {
                    return 1;
                }
            }
            else
                throw new ArgumentException("Object is not a SpecColor");
        }
    }
}
