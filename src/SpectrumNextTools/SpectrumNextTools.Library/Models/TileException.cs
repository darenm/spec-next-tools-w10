using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumNextTools.Library.Models
{
    public class TileException : Exception
    {
        public Tile Tile { get; private set; }

        public TileException()
        {
        }

        public TileException(Tile tile)
        {
            Tile = tile;
        }

        public TileException(Tile tile, string message) : base(message)
        {
            Tile = tile;
        }
        public TileException(Tile tile, string message, Exception innerException) : base(message, innerException)
        {
            Tile = tile;
        }
    }
}
