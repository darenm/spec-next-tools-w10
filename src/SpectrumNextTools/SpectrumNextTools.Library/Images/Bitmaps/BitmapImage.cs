using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectrumNextTools.Library.Images.Bitmaps
{
    public class BitmapImage
    {
        private readonly string _filename;
        private BitmapHeader _bitmapHeader;
        private BitmapInformationHeader _bitmapInformationHeader;
        private uint _paletteOffset;
        private int _imageSize;

        public BitmapHeader Header => this._bitmapHeader;

        public BitmapInformationHeader InformationHeader => this._bitmapInformationHeader;

        public byte[] Palette { get; } = new byte[BitmapConstants.PaletteSize];

        public byte[] MinPalette { get; } = new byte[BitmapConstants.PaletteSize];

        public byte[] StdPaletteIndex { get; } = new byte[BitmapConstants.NumPaletteColors];
        public byte[] MinPaletteIndex { get; } = new byte[BitmapConstants.NumPaletteColors];

        public byte[] Image { get; private set; }

        public BitmapImage(string filename)
        {
            _filename = filename;
        }

        internal void SaveAs(string outFileName)
        {
            try
            {
                File.Copy(_filename, outFileName, true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to copy {_filename} to {outFileName} - {ex.Message}", ex);
            }

            using (var fileStream = new FileStream(outFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                SaveImage(fileStream);
            }
        }

        internal void Save()
        {
            using (var fileStream = new FileStream(_filename, FileMode.Open, FileAccess.ReadWrite))
            {
                SaveImage(fileStream);
            }
        }

        private void SaveImage(FileStream fileStream)
        {
            // Save palette data
            try
            {
                fileStream.Seek(_paletteOffset, SeekOrigin.Begin);
                fileStream.Write(Palette, 0, Palette.Length);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to write BMP palette data for {_filename} - {ex.Message}", ex);
            }

            // Save bitmap data
            try
            {
                fileStream.Seek(_bitmapHeader.ImageDataOffset, SeekOrigin.Begin);
                fileStream.Write(Image, 0, Image.Length);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to write BMP image data for {_filename} - {ex.Message}", ex);
            }
        }

        public void Validate()
        {
            using (var fileStream = new FileStream(_filename, FileMode.Open))
            {
                ValidateFile(fileStream);
            }
        }

        private void ValidateFile(FileStream fileStream)
        {

            try
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    // Header

                    _bitmapHeader.Type = reader.ReadBytes(2);
                    _bitmapHeader.FileSize = reader.ReadUInt32();
                    _bitmapHeader.Reserved1 = reader.ReadUInt16();
                    _bitmapHeader.Reserved2 = reader.ReadUInt16();
                    _bitmapHeader.ImageDataOffset = reader.ReadUInt32();

                    // Information Header
                    _bitmapInformationHeader.HeaderSize = reader.ReadUInt32();
                    _bitmapInformationHeader.Width = reader.ReadInt32();
                    _bitmapInformationHeader.Height = reader.ReadInt32();
                    _bitmapInformationHeader.Planes = reader.ReadUInt16();
                    _bitmapInformationHeader.BitsPerPixel = reader.ReadUInt16();
                    _bitmapInformationHeader.CompressionType = reader.ReadUInt32();
                    _bitmapInformationHeader.ImageSize = reader.ReadUInt32();
                    _bitmapInformationHeader.XResolution = reader.ReadInt32();
                    _bitmapInformationHeader.YResolution = reader.ReadInt32();
                    _bitmapInformationHeader.NumberOfColors = reader.ReadUInt32();
                    _bitmapInformationHeader.ImportantColors = reader.ReadUInt32();

                    // Validate
                    if (_bitmapHeader.Type[0] != 'B' && _bitmapHeader.Type[1] != 'M')
                    {
                        throw new Exception("Not a BMP file");
                    }

                    if (_bitmapHeader.FileSize < BitmapConstants.MinBmpFileSize)
                    {
                        throw new Exception("Invalid size of BMP file.");
                    }

                    if (_bitmapHeader.ImageDataOffset >= _bitmapHeader.FileSize)
                    {
                        throw new Exception("Invalid header of BMP file.");
                    }

                    if (_bitmapInformationHeader.HeaderSize < BitmapConstants.MinDibHeaderSize)
                    {
                        throw new Exception("Invalid/unsupported header of BMP file.");
                    }

                    _paletteOffset = BitmapConstants.FileHeaderSize + _bitmapInformationHeader.HeaderSize;

                    if (_bitmapInformationHeader.Width == 0)
                    {
                        throw new Exception("Invalid image width in BMP file.");
                    }

                    if (_bitmapInformationHeader.Height == 0)
                    {
                        throw new Exception("Invalid image height in BMP file.");
                    }

                    if (_bitmapInformationHeader.BitsPerPixel != 8)
                    {
                        throw new Exception("Not an 8-bit BMP file.");
                    }

                    if (_bitmapInformationHeader.CompressionType != 0)
                    {
                        throw new Exception("Not an uncompressed BMP file.");
                    }

                }
            }
            catch (EndOfStreamException)
            {
                throw new Exception($"Can't read the BMP header in file {fileStream.Name}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't process the BMP {fileStream.Name} - {ex.Message}", ex);
            }
        }

        internal void LoadPaletteAndImageData(bool createNewFile, bool updateImageData)
        {
            try
            {
                // Calculate image size.
                // Note: Image width is padded to a multiple of 4 bytes.
                var paddedImageWidth = (InformationHeader.Width + 3) & ~0x03;
                var imageHeight = Math.Abs(InformationHeader.Height);
                _imageSize = paddedImageWidth * imageHeight;

                if (updateImageData)
                {
                    this.Image = new byte[_imageSize];
                }

                using (var fileStream = new FileStream(_filename, FileMode.Open))
                {
                    if (fileStream.Seek(_paletteOffset, 0) == 0)
                    {
                        throw new Exception($"Can't access the BMP image data in file {_filename}.");
                    }

                    if (fileStream.Read(Palette, 0, Palette.Length) != Palette.Length)
                    {
                        throw new Exception($"Can't read the BMP palette in file {_filename}.");
                    }

                    if (updateImageData)
                    {
                        if (fileStream.Seek(Header.ImageDataOffset, 0) == 0)
                        {
                            throw new Exception($"Can't access the BMP image data in file {_filename}.");
                        }

                        if (fileStream.Read(Image, 0, Image.Length) != Image.Length)
                        {
                            throw new Exception($"Can't read the BMP image data in file {_filename}.");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to process BMP - {ex.Message}", ex);
            }
        }

        internal void UpdateColors(bool useStdPalette, RoundingMode roundingMode, bool minimizePalette)
        {
            if (useStdPalette)
            {
                // Convert the colors in the palette to the Spectrum Next standard palette RGB332 colors.
                ConvertStandardPalette();

                // Update the image pixels to use the new palette indexes of the standard palette colors.
                for (int i = 0; i < _imageSize; i++)
                {
                    Image[i] = StdPaletteIndex[Image[i]];
                }
            }
            else
            {
                ConvertPalette();

                if (minimizePalette)
                {
                    // Minimize the converted palette by removing any duplicated colors and sort it
                    // in ascending RGB order. Any unused palette entries at the end are set to 0 (black).
                    byte[] minimizedPalette = CreateMinimizedPalette();

                    // Create an index table containing the palette indexes of the minimized palette
                    // that correspond to the palette indexes of the originally converted palette.
                    CreateMinimizedPaletteIndexTable(minimizedPalette);

                    minimizedPalette.CopyTo(Palette, 0);

                    // Update the image pixels to use the palette indexes of the minimized palette.
                    for (int i = 0; i < Image.Length; i++)
                    {
                        Image[i] = MinPaletteIndex[Image[i]];
                    }
                }
            }

            // Convert the colors in the palette to the Spectrum Next standard palette RGB332 colors.
            void ConvertStandardPalette()
            {
                // Update the colors in the palette.
                // The original RGB888 colors in the palette are converted to the RGB332/
                // RGB333 colors in the standard palette and then back to their equivalent
                // RGB888 colors.
                for (int i = 0; i < BitmapConstants.NumPaletteColors; i++)
                {
                    // BMP palette contains BGRA colors.
                    byte r8 = Palette[i * 4 + 2];
                    byte g8 = Palette[i * 4 + 1];
                    byte b8 = Palette[i * 4 + 0];

                    // Convert the RGB888 color to an RGB332 color.
                    // The RGB332 value is also the index for this color in the standard
                    // palette. The pixels having palette index i will be updated with this
                    // new palette index which points to the new location of the converted
                    // RGB888 color that was originally stored at index i.
                    byte r3 = Color8ToColor3(r8, roundingMode);
                    byte g3 = Color8ToColor3(g8, roundingMode);
                    byte b2 = Color8ToColor2(b8, roundingMode);
                    StdPaletteIndex[i] = (byte)((r3 << 5) | (g3 << 2) | (b2 << 0));

                    // Create the standard RGB332/RGB333 color for this palette index.
                    // The standard RGB332 color has the same value as its index in the
                    // standard palette. The actual color displayed on the Spectrum Next
                    // is an RGB333 color where the lowest blue bit as a bitwise OR between
                    // the two blue bits in the RGB332 color.
                    byte std_r3 = (byte)((i >> 5) & 0x07);
                    byte std_g3 = (byte)((i >> 2) & 0x07);
                    byte std_b2 = (byte)((i >> 0) & 0x03);
                    byte std_b3 = Color2ToColor3(std_b2);

                    // Convert the standard RGB333 color back to an RGB888 color.
                    r8 = Color3ToColor8(std_r3);
                    g8 = Color3ToColor8(std_g3);
                    b8 = Color3ToColor8(std_b3);

                    // Update the palette with the RGB888 representation of the standard RGB333 color.
                    Palette[i * 4 + 3] = 0;
                    Palette[i * 4 + 2] = r8;
                    Palette[i * 4 + 1] = g8;
                    Palette[i * 4 + 0] = b8;
                }
            }

            void ConvertPalette()
            {
                // Update the colors in the palette.
                // The original RGB888 colors in the palette are converted to
                // RGB333 colors and then back to their equivalent RGB888 colors.
                for (int i = 0; i < BitmapConstants.NumPaletteColors; i++)
                {
                    // BMP palette contains BGRA colors.
                    byte r8 = Palette[i * 4 + 2];
                    byte g8 = Palette[i * 4 + 1];
                    byte b8 = Palette[i * 4 + 0];

                    byte r3 = Color8ToColor3(r8, roundingMode);
                    byte g3 = Color8ToColor3(g8, roundingMode);
                    byte b3 = Color8ToColor3(b8, roundingMode);

                    r8 = Color3ToColor8(r3);
                    g8 = Color3ToColor8(g3);
                    b8 = Color3ToColor8(b3);

                    Palette[i * 4 + 3] = 0;
                    Palette[i * 4 + 2] = r8;
                    Palette[i * 4 + 1] = g8;
                    Palette[i * 4 + 0] = b8;
                }
            }

            void CreateMinimizedPaletteIndexTable(byte[] minimizedPalette)
            {
                for (int i = 0; i < BitmapConstants.NumPaletteColors; i++)
                {
                    for (int j = 0; j < BitmapConstants.NumPaletteColors; j++)
                    {
                        if (Palette[i] == minimizedPalette[i])
                        {
                            MinPaletteIndex[i] = (byte)j;
                        }
                    }
                }
            }

            byte[] CreateMinimizedPalette()
            {
                var minPaletteColors = new byte[Palette.Length];
                Palette.CopyTo(minPaletteColors, 0);
                var bitmapPalette = new BitmapPalette(minPaletteColors);
                minPaletteColors = bitmapPalette.Sort().RemoveDuplicates().ToArray();
                return minPaletteColors;
            }

        }

        private static byte Color8ToColor3(byte color8, RoundingMode roundingMode)
        {
            double color3 = (color8 * 7.0) / 255.0;
            switch (roundingMode)
            {
                case RoundingMode.Floor:
                    return (byte)Math.Floor(color3);

                case RoundingMode.Ceil:
                    return (byte)Math.Ceiling(color3);

                case RoundingMode.Unknown:
                case RoundingMode.Round:
                default:
                    return (byte)Math.Round(color3);
            }
        }

        private static byte Color8ToColor2(byte color8, RoundingMode roundingMode)
        {
            double color2 = (color8 * 3.0) / 255.0;
            switch (roundingMode)
            {
                case RoundingMode.Floor:
                    return (byte)Math.Floor(color2);

                case RoundingMode.Ceil:
                    return (byte)Math.Ceiling(color2);

                case RoundingMode.Unknown:
                case RoundingMode.Round:
                default:
                    return (byte)Math.Round(color2);
            }
        }

        private static byte Color2ToColor3(byte color2)
        {
            return (byte)((color2 << 1) | (((color2 >> 1) | color2) & 0x01));
        }

        private static byte Color3ToColor8(byte color3)
        {
            return (byte)Math.Round((color3 * 255.0) / 7.0);
        }
    }
}
