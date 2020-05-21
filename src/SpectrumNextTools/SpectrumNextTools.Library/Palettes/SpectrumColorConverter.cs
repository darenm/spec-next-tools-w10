using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumNextTools.Library.Palettes
{
    public static class SpectrumColorConverter
    {
        public static string ConvertRGBHex(string text)
        {
            var rHex = text.Substring(0, 2);
            var gHex = text.Substring(2, 2);
            var bHex = text.Substring(4, 2);
            var r = ConvertHexByte(rHex);
            var g = ConvertHexByte(gHex);
            var b = ConvertHexByte(bHex);

            r &= 0b1110_0000;
            g &= 0b1110_0000;
            b &= 0b1110_0000;

            r >>= 5;
            g >>= 5;
            b >>= 5;

            return $"{r}, {g}, {b}";
        }

        public static int ConvertHexByte(string hex)
        {
            if (hex.Length != 2)
            {
                throw new ArgumentException("hex should be 2 characters in length");
            }

            return int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        public static byte EightBitFromBgr(byte b, byte g, byte r)
        {
            r &= 0b1110_0000;
            g &= 0b1110_0000;
            b &= 0b1110_0000;

            g >>= 3;
            b >>= 6;

            return (byte)(r + g + b);
        }

        public static void NineBitFromBgr(byte b, byte g, byte r, out byte eightBitColor, out byte nineBitColor)
        {
            eightBitColor = EightBitFromBgr(b, g, r);
            nineBitColor = (byte)((b & 0b0010_0000) >> 5);
        }

        public static void EightBitToBgr(byte eightBitColor, out byte r, out byte g, out byte b)
        {
            r = (byte)(0b1110_0000 & eightBitColor);
            g = (byte)(0b0001_1100 & eightBitColor);
            b = (byte)(0b0000_0011 & eightBitColor);

            g <<= 3;
            b <<= 6;
            // third bit of blue color is logical or of bits 1 and 0
            // therefore if b > 0, then add 64
            //b += 64;

        }

        public static void NineBitToBgra(byte eightBitColor, byte nineBitColor, out byte a, out byte r, out byte g, out byte b)
        {
            // we ignore a
            a = 255;
            r = (byte)(0b1110_0000 & eightBitColor);
            g = (byte)(0b0001_1100 & eightBitColor);
            b = (byte)(0b0000_0011 & eightBitColor);

            g <<= 3;
            b <<= 6;
            b += (byte)(nineBitColor & 0b0000_0001);
        }
    }
}
