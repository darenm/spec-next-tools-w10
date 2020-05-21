using SpectrumNextTools.Library.Palettes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumNextTools.Library.Models
{
    public class SpecColor
    {
        private readonly byte r;
        private readonly byte g;
        private readonly byte b;

        public byte EightBitColor { get; set; }
        public byte NineBitColor { get; set; }

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
            SpectrumColorConverter.EightBitToBgr(specColor, out r, out g, out b);
        }

        public SpecColor(byte r, byte g, byte b)
        {
            this.r = r;
            this.b = b;
            this.g = g;

            //EightBitColor = SpectrumColorConverter.EightBitFromBgr(b, g, r);
            SpectrumColorConverter.NineBitFromBgr(b, g, r, out byte eightBitColor, out byte nineBitColor);
            EightBitColor = eightBitColor;
            NineBitColor = nineBitColor;
        }

        public override string ToString()
        {
            return $"RGB: #{R:X2}{G:X2}{B:X2} SPEC: #{EightBitColor:X2} #{NineBitColor:X2}";
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
