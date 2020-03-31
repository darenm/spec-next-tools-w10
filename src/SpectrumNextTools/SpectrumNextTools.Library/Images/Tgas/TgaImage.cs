using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectrumNextTools.Library
{
    public class TgaImage 
    {
        private readonly string _filename;
        private TgaHeader _tgaHeader;

        public TgaImage(string filename)
        {
            _filename = filename;
        }

        public TgaHeader Header => _tgaHeader;

        public void Validate()
        {
            using (FileStream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        _tgaHeader.IdLength = reader.ReadByte();
                        _tgaHeader.ColorMapType = reader.ReadByte();
                        _tgaHeader.ImageType = reader.ReadByte();

                        _tgaHeader.ColorMapSpecification.FirstEntryIndex = reader.ReadUInt16();
                        _tgaHeader.ColorMapSpecification.ColorMapLength = reader.ReadUInt16();
                        _tgaHeader.ColorMapSpecification.ColorMapEntrySize = reader.ReadByte();


                        _tgaHeader.ImageSpecification.XOrigin = reader.ReadUInt16();
                        _tgaHeader.ImageSpecification.YOrigin = reader.ReadUInt16();
                        _tgaHeader.ImageSpecification.ImageWidth = reader.ReadUInt16();
                        _tgaHeader.ImageSpecification.ImageHeight = reader.ReadUInt16();
                        _tgaHeader.ImageSpecification.PixelDepth = reader.ReadByte();
                        _tgaHeader.ImageSpecification.ImageDescriptor = reader.ReadByte();

                        _tgaHeader.ColorMapData.TgaColors = new TgaColor[256];
                        for (int i = 0; i < 256; i++)
                        {
                            _tgaHeader.ColorMapData.TgaColors[i].Blue = reader.ReadByte();
                            _tgaHeader.ColorMapData.TgaColors[i].Green = reader.ReadByte();
                            _tgaHeader.ColorMapData.TgaColors[i].Red = reader.ReadByte();
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception($"Can't read the TGA header in file {_filename}.");
                }
            }
        }
    }
}
