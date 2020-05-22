using SpectrumNextTools.Library.Palettes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumNextTools.Library.Models
{
    public class SpecColor : IComparable
    {
        private readonly byte r;
        private readonly byte g;
        private readonly byte b;

        public byte EightBitColor { get; private set; }
        public byte NineBitColor { get; private set; }

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

        public override bool Equals(object obj)
        {
            if (obj is SpecColor other)
            {
                if (R == other.R
                    && G == other.G
                    && B == other.B
                    && EightBitColor == other.EightBitColor
                    && NineBitColor == other.NineBitColor)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return EightBitColor ^ NineBitColor;
        }

        public static bool operator ==(SpecColor a, SpecColor b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((a is null) || (b is null)) 
            {
                return false;
            }

            // Return true if the fields match:
            return a.R == b.R
                   && a.G == b.G
                   && a.B == b.B
                   && a.EightBitColor == b.EightBitColor
                   && a.NineBitColor == b.NineBitColor;
        }

        public static bool operator !=(SpecColor a, SpecColor b)
        {
            return !(a == b);
        }
    }
}
