using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumNextTools.Library.Images.Bitmaps
{
    public class BitmapOptions
    {
        public RoundingMode RoundingMode { get; set; }
        public bool MinimizePalette { get; set; }
        public bool UseStdPalette { get; set; }
        public string InFileName { get; set; }
        public string OutFileName { get; set; }
    }
}
