using System.Diagnostics;
using System.Linq;

namespace SpectrumNextTools.Library.Models
{
    public class TileMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly TileMapEntry[,] _tiles;

        public TileMapEntry this[int row, int column]
        {
            get
            {
                return _tiles[row, column];
            }
            set
            {
                _tiles[row, column] = value;
            }
        }

        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new TileMapEntry[height, width];
        }

        public void PlaceTile(int row, int column, int tileIndex)
        {
            PlaceTile(row, column, tileIndex, TileOrientation.Default());
        }

        public void PlaceTile(int row, int column, int tileIndex, TileOrientation orientation)
        {
            _tiles[row, column] = new TileMapEntry(tileIndex, orientation);
        }

        public void ReplaceTilesByTileIndex(int oldTileIndex, int newTileIndex, TileOrientation orientation)
        {
            if (oldTileIndex == newTileIndex)
            {
                return;
            }

            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (_tiles[row, column].Index == oldTileIndex)
                    {
                        _tiles[row, column] = new TileMapEntry(newTileIndex, orientation);
                    }
                }
            }
        }
        public void UpdateTileIndex(int oldTileIndex, int newTileIndex)
        {
            if (oldTileIndex == newTileIndex)
            {
                return;
            }

            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (_tiles[row, column].Index == oldTileIndex)
                    {
                        var orientation = _tiles[row, column].Orientation;
                        _tiles[row, column] = new TileMapEntry(newTileIndex, orientation);
                    }
                }
            }
        }

    }

    public class TileMapEntry
    {
        public TileMapEntry(int index, TileOrientation orientation)
        {
            Index = index;
            Orientation = orientation;
        }

        public int Index { get; private set; }
        public TileOrientation Orientation { get; private set; }
    }
}
