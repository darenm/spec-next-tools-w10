using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectrumNextTools.Library.Images.Bitmaps
{
    public class BitmapImage
    {
        private readonly string _filename;

        private readonly byte[] _header = new byte[BitmapConstants.HeaderSize];
        private readonly byte[] _palette = new byte[BitmapConstants.PaletteSize];
        private readonly byte[] _minPalette = new byte[BitmapConstants.PaletteSize];
        private readonly byte[] _minPaletteIndex = new byte[BitmapConstants.NumPaletteColors];
        private readonly byte[] _stdPaletteIndex = new byte[BitmapConstants.NumPaletteColors];

        private BitmapHeader _bitmapHeader;
        private BitmapInformationHeader _bitmapInformationHeader;
        private readonly int _paletteOffset;
        private readonly int _imageOffset;
        private readonly int _imageWidth;
        private readonly int _imageHeight;

        private byte[] _image; // uninitalized

        public BitmapHeader Header => this._bitmapHeader;

        public BitmapInformationHeader InformationHeader => this._bitmapInformationHeader; 

        public byte[] Palette => this._palette;

        public byte[] MinPalette => this._minPalette;

        public byte[] MinPaletteIndex => this._minPaletteIndex;

        public byte[] StdPaletteIndex => this._stdPaletteIndex;

        public byte[] Image { get => this._image; set => this._image = value; }

        public int PaletteOffset => this._paletteOffset;

        public int ImageOffset => this._imageOffset;

        public int ImageWidth => this._imageWidth;

        public int ImageHeight => this._imageHeight;


        public BitmapImage(string filename)
        {
            _filename = filename;
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
                throw new Exception($"Can't read the BMP header in file {fileStream.Name} - {ex.Message}");
            }


            try
            {
                //ProcessHeader();
            }
            catch (Exception ex)
            {
                throw new Exception($"The file { fileStream.Name } is not a valid or supported BMP file - {ex.Message}.");
            }
        }

    }
}
