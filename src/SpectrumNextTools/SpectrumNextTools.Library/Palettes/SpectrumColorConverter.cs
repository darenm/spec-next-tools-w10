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

            r = r & 0b1110_0000;
            g = g & 0b1110_0000;
            b = b & 0b1110_0000;

            r = r >> 5;
            g = g >> 5;
            b = b >> 5;

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

        public static byte ConvertFromBgr(byte b, byte g, byte r)
        {
            return ConvertFromBgra(b, g, r, 255);
        }


        public static byte ConvertFromBgra(byte b, byte g, byte r, byte a)
        {
            // we ignore a

            r &= 0b1110_0000;
            g &= 0b1110_0000;
            b &= 0b1110_0000;

            g >>= 3;
            b >>= 6;

            return (byte)(r + g + b);
        }

        public static void ConvertToBgra(byte spectrumColor,out byte a, out byte r, out byte g, out byte b)
        {
            // we ignore a
            a = 255;
            r = (byte)(0b1110_0000 & spectrumColor);
            g = (byte)(0b0001_1100 & spectrumColor);
            b = (byte)(0b0000_0011 & spectrumColor);

            g <<= 3;
            b <<= 6;
        }
    }
}
