using System;

namespace SpectrumNextTools.Library.Images.Bitmaps
{
    public class BitmapPaletteColor : IComparable, IComparable<BitmapPaletteColor>
    {
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }
        public byte A { get; private set; }

        public BitmapPaletteColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            B = b;
            G = g;
            A = a;
        }

        public BitmapPaletteColor(byte[] palette, int offset)
        {
            if (palette.Length % 4 != 0)
            {
                throw new ArgumentException($"{nameof(palette)} must be divisible by 4");
            }

            if (offset % 4 != 0)
            {
                throw new ArgumentException($"{nameof(offset)} must be divisible by 4");
            }

            R = palette[offset];
            G = palette[offset+1];
            B = palette[offset+2];
            A = palette[offset+3];
        }

        public int CompareTo(object other)
        {
            return GetHashCode() > other.GetHashCode() ? 1 : GetHashCode() < other.GetHashCode() ? -1 : 0;
        }

        public int CompareTo(BitmapPaletteColor other)
        {
            return GetHashCode() > other.GetHashCode() ? 1 : GetHashCode() < other.GetHashCode() ? -1 : 0;
        }

        public override int GetHashCode()
        {
            return (R << 16) | (G << 8) | (B << 0);
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public byte[] ToArray()
        {
            return new byte[] { R, G, B, A };
        }

        public override string ToString()
        {
            return $"R: {R:D3}, G: {G:D3}, B: {B:D3}, A: {A:D3}";
        }

        public static bool operator ==(BitmapPaletteColor left, BitmapPaletteColor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BitmapPaletteColor left, BitmapPaletteColor right)
        {
            return !left.Equals(right);
        }
    }
}
