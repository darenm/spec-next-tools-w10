using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumNextTools.Library.Models
{
    public class Tile : IEquatable<Tile>
    {
        private const int MatrixDimension = 8;
        private List<SpecColor> _assignedPalette;

        public byte[,] Pixels { get; private set; } = new byte[MatrixDimension, MatrixDimension]; // byte[x, y]
        public byte[,] ExportBytes { get; private set; } = new byte[MatrixDimension, MatrixDimension / 2]; // byte[x, y]

        public bool IsPaletteMatched { get; set; }
        public int MatchedPaletteId { get; set; }

        public Tile()
        { }

        public Tile(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length != MatrixDimension * MatrixDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "bytes array must be 64 in length");
            }

            var i = 0;
            foreach (var pixel in bytes)
            {
                var column = i % MatrixDimension;
                var row = i / MatrixDimension;
                Pixels[row, column] = pixel;
                i++;
            }
        }

        public Tile(byte[,] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length != MatrixDimension * MatrixDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "bytes array must be 64 in length");
            }

            Pixels = bytes;
        }

        public byte[,] ApplyTransform(bool rotate, bool mirrorX, bool mirrorY)
        {
            byte[,] transformedBytes;

            if (!rotate && !mirrorX && !mirrorY)
            {
                return Pixels;
            }

            // rotate always applied first


            if (rotate)
            {
                transformedBytes = RotateMatrix(Pixels, MatrixDimension);
            }
            else
            {
                transformedBytes = new byte[MatrixDimension, MatrixDimension];
                Array.Copy(Pixels, transformedBytes, MatrixDimension * MatrixDimension);
            }

            // Now do mirror - order doesn't matter

            if (mirrorX)
            {
                transformedBytes = MirrorXMatrix(transformedBytes, MatrixDimension);
            }

            if (mirrorY)
            {
                transformedBytes = MirrorYMatrix(transformedBytes, MatrixDimension);
            }

            return transformedBytes;
        }

        /// <summary>
        /// Returns the sorted <see cref="SpecColor"/> palette used in the raw image
        /// </summary>
        /// <returns></returns>
        public List<SpecColor> GetPaletteFromTile()
        {
            var paletteBytes = new List<byte>();
            var palette = new List<SpecColor>();
            foreach (var pixel in Pixels)
            {
                if (!paletteBytes.Contains(pixel))
                {
                    paletteBytes.Add(pixel);
                    palette.Add(new SpecColor(pixel));
                }
            }

            if (palette.Count > 16)
            {
                throw new TileException(this, $"Too many colors - {palette.Count}");
            }

            palette.Sort();

            return palette;
        }

        /// <summary>
        /// Assigns the supplied <paramref name="palette"/> to the <see cref="Tile"/>. This
        /// <b>does not</b> change the <see cref="Pixels"/> data, but instead populates the
        /// 
        /// </summary>
        /// <param name="paletteId"></param>
        /// <param name="palette"></param>
        public void AssignPalette(List<SpecColor> palette)
        {
            _assignedPalette = palette;

            // iterate through each byte in Pixels
            // match the byte to a palette entry - throw if doesn't match!
            // create nibble that matches the palette entry
            // combine two adjacent nibbles to a byte and write to 

            Nibble highNibble = null;

            for (var row = 0; row < MatrixDimension; row++)
            {
                for (var column = 0; column < MatrixDimension; column++)
                {
                    var isHighNibble = column % 2 == 0; // 0 is high, 1 is low

                    var rawSpecColor = new SpecColor(Pixels[row, column]);
                    var paletteIndex = palette.IndexOf(rawSpecColor);
                    if (paletteIndex > -1) // found
                    {
                        if (isHighNibble)
                        {
                            highNibble = new Nibble(paletteIndex);
                            // do nothing else until the next iteratiion
                        }
                        else
                        {
                            var lowNibble = new Nibble(paletteIndex);
                            var exportByte = Nibble.Combine(highNibble, lowNibble);
                            ExportBytes[row, column / 2] = exportByte;
                        }

                    }
                    else
                    {
                        throw new TileException(this, "Pixel color not found in palette");
                    }
                }
            }
        }

        static byte[,] RotateMatrix(byte[,] matrix, int n)
        {
            var ret = new byte[n, n];

            for (var row = 0; row < n; ++row)
            {
                for (var column = 0; column < n; ++column)
                {
                    ret[row, column] = matrix[n - column - 1, row];
                }
            }

            return ret;
        }

        static byte[,] MirrorXMatrix(byte[,] matrix, int n)
        {
            var ret = new byte[n, n];

            for (var row = 0; row < n; ++row)
            {
                for (var column = 0; column < n; ++column)
                {
                    ret[row, column] = matrix[row, n - column - 1];
                }
            }

            return ret;
        }
        static byte[,] MirrorYMatrix(byte[,] matrix, int n)
        {
            var ret = new byte[n, n];

            for (var row = 0; row < n; ++row)
            {
                for (var column = 0; column < n; ++column)
                {
                    ret[row, column] = matrix[n - row - 1, column];
                }
            }

            return ret;
        }

        public bool Match(Tile inputTile, out TileOrientation tileOrientation)
        {
            bool rotate = false, mirrorX = false, mirrorY = false;

            var result = Equals(inputTile);

            byte[,] mirrorXResult = null; // saving this incase I need it later

            // R0 X1 Y0
            if (!result)
            {
                rotate = false;
                mirrorX = true;
                mirrorY = false;
                mirrorXResult = MirrorXMatrix(Pixels, MatrixDimension);
                result = MatchBytes(inputTile.Pixels, mirrorXResult);
            }

            // R0 X0 Y1
            if (!result)
            {
                rotate = false;
                mirrorX = false;
                mirrorY = true;
                var mirrorYResult = MirrorYMatrix(Pixels, MatrixDimension);
                result = MatchBytes(inputTile.Pixels, mirrorYResult);
            }

            // R0 X1 Y1
            if (!result)
            {
                rotate = false;
                mirrorX = true;
                mirrorY = true;
                // note: y mirror mirrorXResult
                var mirrorYResult = MirrorYMatrix(mirrorXResult, MatrixDimension);
                result = MatchBytes(inputTile.Pixels, mirrorYResult);
            }

            // now we need to rotate
            byte[,] rotateResult = null; // saving this incase I need it later

            // R1 X0 Y0
            if (!result)
            {
                rotate = true;
                mirrorX = false;
                mirrorY = false;

                rotateResult = RotateMatrix(Pixels, MatrixDimension);
                result = MatchBytes(inputTile.Pixels, rotateResult);
            }

            // R1 X1 Y0
            if (!result)
            {
                rotate = true;
                mirrorX = true;
                mirrorY = false;
                // reuse rotate result
                mirrorXResult = MirrorXMatrix(rotateResult, MatrixDimension);
                result = MatchBytes(inputTile.Pixels, mirrorXResult);
            }

            // R1 X0 Y1
            if (!result)
            {
                rotate = true;
                mirrorX = false;
                mirrorY = true;

                // reuse rotate result
                var mirrorYResult = MirrorYMatrix(rotateResult, MatrixDimension);
                result = MatchBytes(inputTile.Pixels, mirrorYResult);
            }

            // R1 X1 Y1
            if (!result)
            {
                rotate = true;
                mirrorX = true;
                mirrorY = true;

                // note: y mirror mirrorXResult
                var mirrorYResult = MirrorYMatrix(mirrorXResult, MatrixDimension);
                result = MatchBytes(inputTile.Pixels, mirrorYResult);
            }

            tileOrientation = new TileOrientation(rotate, mirrorX, mirrorY);
            return result;
        }

        //public override string ToString()
        //{
        //    var sb = new StringBuilder();
        //    for (var i = 0; i < MatrixDimension; i++)
        //    {
        //        for (var j = 0; j < MatrixDimension; j++)
        //        {
        //            sb.Append(string.Format("{0} ", Pixels[i, j]));
        //        }
        //        sb.AppendLine();
        //    }
        //    return sb.ToString();

        //}

        public override bool Equals(object obj)
        {
            var item = obj as Tile;
            return Equals(item);
        }

        public override int GetHashCode()
        {
            return Pixels.GetHashCode();
        }

        public bool Equals(Tile other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.Pixels.Length != Pixels.Length)
            {
                return false;
            }

            return MatchBytes(Pixels, other.Pixels);
        }

        private static bool MatchBytes(byte[,] array1, byte[,] array2)
        {
            for (int row = 0; row < MatrixDimension; row++)
            {
                for (int column = 0; column < MatrixDimension; column++)
                {
                    if (array1[row, column] != array2[row, column])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool operator ==(Tile t1, Tile t2)
        {
            if (t1 is null)
            {
                return t2 is null;
            }

            return t1.Equals(t2);
        }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1 == t2);
        }
    }
}
