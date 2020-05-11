using SimpleSpecNextTiler.ViewModels;
using Windows.UI;

namespace SpecNextTiler.Models
{
    public class SpecColor : ViewModelBase
    {
        private readonly byte r;
        private readonly byte g;
        private readonly byte b;

        public byte EightBitColor { get; set; }


        public Color WinColor
        {
            get
            {
                return Color.FromArgb(255, r, g, b);
            }
        }

        public SpecColor()
        {
            EightBitColor = 0;
            r = b = g = 0;
        }

        public SpecColor(byte r, byte g, byte b)
        {
            this.r = r;
            this.b = b;
            this.g = g;

            EightBitColor = (byte)
                ((r & 0b111_000_00) +
                ((g >> 3) & 0b000_111_00) +
                ((b >> 6) & 0b000_000_11));
        }

        public SpecColor(Color c) : this(c.R, c.G, c.B)
        {
        }

        public override string ToString()
        {
            return $"RGB: #{r:X2}{g:X2}{b:X2} SPEC: #{EightBitColor:X2}";
        }

    }
}
