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
        private const int matrixDimension = 8;
        private readonly byte[,] pixels = new byte[matrixDimension, matrixDimension]; // byte[x, y]

        public byte[,] Pixels => pixels;

        public Tile()
        { }

        public Tile(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length != matrixDimension * matrixDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "bytes array must be 64 in length");
            }

            var i = 0;
            foreach (var pixel in bytes)
            {
                var column = i % matrixDimension;
                var row = i / matrixDimension;
                pixels[row, column] = pixel;
                i++;
            }
        }

        public Tile(byte[,] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length != matrixDimension * matrixDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "bytes array must be 64 in length");
            }

            this.pixels = bytes;
        }

        public byte[,] ApplyTransform(bool rotate, bool mirrorX, bool mirrorY)
        {
            byte[,] transformedBytes;

            if (!rotate && !mirrorX && !mirrorY)
            {
                return this.pixels;
            }

            // rotate always applied first


            if (rotate)
            {
                transformedBytes = RotateMatrix(pixels, matrixDimension);
            }
            else
            {
                transformedBytes = new byte[matrixDimension, matrixDimension];
                Array.Copy(pixels, transformedBytes, matrixDimension * matrixDimension);
            }

            // Now do mirror - order doesn't matter

            if (mirrorX)
            {
                transformedBytes = MirrorXMatrix(transformedBytes, matrixDimension);
            }

            if (mirrorY)
            {
                transformedBytes = MirrorYMatrix(transformedBytes, matrixDimension);
            }

            return transformedBytes;
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

            var result = this.Equals(inputTile);

            byte[,] mirrorXResult = null; // saving this incase I need it later

            // R0 X1 Y0
            if (!result)
            {
                rotate = false;
                mirrorX = true;
                mirrorY = false;
                mirrorXResult = MirrorXMatrix(this.pixels, matrixDimension);
                result = MatchBytes(inputTile.pixels, mirrorXResult);
            }

            // R0 X0 Y1
            if (!result)
            {
                rotate = false;
                mirrorX = false;
                mirrorY = true;
                var mirrorYResult = MirrorYMatrix(this.pixels, matrixDimension);
                result = MatchBytes(inputTile.pixels, mirrorYResult);
            }

            // R0 X1 Y1
            if (!result)
            {
                rotate = false;
                mirrorX = true;
                mirrorY = true;
                // note: y mirror mirrorXResult
                var mirrorYResult = MirrorYMatrix(mirrorXResult, matrixDimension);
                result = MatchBytes(inputTile.pixels, mirrorYResult);
            }

            // now we need to rotate
            byte[,] rotateResult = null; // saving this incase I need it later

            // R1 X0 Y0
            if (!result)
            {
                rotate = true;
                mirrorX = false;
                mirrorY = false;

                rotateResult = RotateMatrix(this.pixels, matrixDimension);
                result = MatchBytes(inputTile.pixels, rotateResult);
            }

            // R1 X1 Y0
            if (!result)
            {
                rotate = true;
                mirrorX = true;
                mirrorY = false;
                // reuse rotate result
                mirrorXResult = MirrorXMatrix(rotateResult, matrixDimension);
                result = MatchBytes(inputTile.pixels, mirrorXResult);
            }

            // R1 X0 Y1
            if (!result)
            {
                rotate = true;
                mirrorX = false;
                mirrorY = true;

                // reuse rotate result
                var mirrorYResult = MirrorYMatrix(rotateResult, matrixDimension);
                result = MatchBytes(inputTile.pixels, mirrorYResult);
            }

            // R1 X1 Y1
            if (!result)
            {
                rotate = true;
                mirrorX = true;
                mirrorY = true;

                // note: y mirror mirrorXResult
                var mirrorYResult = MirrorYMatrix(mirrorXResult, matrixDimension);
                result = MatchBytes(inputTile.pixels, mirrorYResult);
            }

            tileOrientation = new TileOrientation(rotate, mirrorX, mirrorY);
            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < matrixDimension; i++)
            {
                for (var j = 0; j < matrixDimension; j++)
                {
                    sb.Append(string.Format("{0} ", pixels[i, j]));
                }
                sb.AppendLine();
            }
            return sb.ToString();

        }

        public override bool Equals(object obj)
        {
            var item = obj as Tile;
            return this.Equals(item);
        }

        public override int GetHashCode()
        {
            return pixels.GetHashCode();
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

            if (other.pixels.Length != this.pixels.Length)
            {
                return false;
            }

            return MatchBytes(this.pixels, other.pixels);
        }

        private static bool MatchBytes(byte[,] array1, byte[,] array2)
        {
            for (int row = 0; row < matrixDimension; row++)
            {
                for (int column = 0; column < matrixDimension; column++)
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
