using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumNextTools.Library.Images.Bitmaps
{
    public class BitmapPalette
    {
        public BitmapPalette(byte[] palette)
        {
            // each palette entry must be 4 bytes, therefore modulus of palette length
            // divided by 4 should be 0
            if (palette.Length % 4 != 0)
            {
                throw new ArgumentException($"{nameof(palette)} is an invalid BMP palette - total length must be divisible by 4");
            }

            Colors = new BitmapPaletteColor[palette.Length / 4];
            for (int i = 0; i < palette.Length/4; i++)
            {
                Colors[i] = new BitmapPaletteColor(palette, i * 4);
            }
        }

        public BitmapPaletteColor[] Colors { get; private set; }

        public int NumberOfUniqueColors { get; private set; }

        public BitmapPalette Sort()
        {
            Array.Sort(Colors);
            return this;
        }

        public BitmapPalette RemoveDuplicates()
        {
            NumberOfUniqueColors = 0;

            for (int i = 0; i < Colors.Length; i++)
            {
                if (Colors[i] != Colors[NumberOfUniqueColors])
                {
                    Colors[++NumberOfUniqueColors] = Colors[i];
                }
            }

            // as NumberOfUniqueColors is currently set to the index, increment it for the count
            NumberOfUniqueColors++;

            SetUnusedColorsToBlack();
            return this;
        }

        private BitmapPalette SetUnusedColorsToBlack()
        {
            for (int i = NumberOfUniqueColors; i < Colors.Length; i++)
            {
                Colors[i] = new BitmapPaletteColor(0, 0, 0, 0);
            }
            return this;
        }

        public byte[] ToArray()
        {
            var palette = new byte[Colors.Length * 4];

            for (var i = 0; i < Colors.Length; i++)
            {
                Colors[i].ToArray().CopyTo(palette, i * 4);
            }

            return palette;
        }
    }
}
