﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SpectrumNextTools.Library.Models
{
    public class TileOrientation
    {
        public TileOrientation() : this(false, false, false)
        { }

        public TileOrientation(bool rotate, bool mirrorX, bool mirrorY)
        {
            this.Rotate = rotate;
            this.MirrorX = mirrorX;
            this.MirrorY = mirrorY;
        }

        public bool Rotate { get; private set; }
        public bool MirrorX { get; private set; }
        public bool MirrorY { get; private set; }

        public static TileOrientation Default()
        {
            return new TileOrientation();
        }
    }
}
