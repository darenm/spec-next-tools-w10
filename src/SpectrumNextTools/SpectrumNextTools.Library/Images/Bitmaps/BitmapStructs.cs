using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SpectrumNextTools.Library.Images.Bitmaps
{
    /* 

    Header
    ________________________________________________________________________________
    | Offset | Length |                     Description                            |
    |--------|--------|------------------------------------------------------------|
    |    0   |     2  |  Magic identifier.                                         |
    |        |        |                                                            |
    |        |        |  This field is a two byte string - should be 'BM'          |
    |--------|--------|------------------------------------------------------------|
    |    2   |     4  |  File size in bytes                                        |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    6   |     2  |  reserved1.                                                |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    8   |     2  |  reserved2.                                                |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    10  |     4  |  Offset to image data, bytes                               |
    |        |        |                                                            |
    ________________________________________________________________________________

    Information
    ________________________________________________________________________________
    | Offset | Length |                     Description                            |
    |--------|--------|------------------------------------------------------------|
    |    0   |     4  |  Header length in bytes.                                   |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    4   |     4  |  Width in bytes.                                           |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    8   |     4  |  Height in bytes.                                          |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    12  |     2  |  Number of color panes                                     |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    14  |     2  |  bits per pixel                                            |
    |        |        |                                                            |
    |        |        |      1, 4, 8 or 24                                         |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    16  |     4  |  Compression type                                          |
    |        |        |                                                            |
    |        |        |      0 - no compression                                    |
    |        |        |      1 - 8 bit run length encoding                         |
    |        |        |      2 - 4 bit run length encoding                         |
    |        |        |      3 - RGB bitmap with mask                              |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    20  |     4  |  Image size in bytes.                                      |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    24  |     4  |  xresolution in bytes.                                     |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    28  |     4  |  yresolution in bytes.                                     |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    32  |     4  |  Number of colors in bytes.                                |
    |        |        |                                                            |
    |--------|--------|------------------------------------------------------------|
    |    36  |     4  |  Number of important colors in bytes.                      |
    |        |        |                                                            |
   ________________________________________________________________________________


    Notes on Palette format

        RGBA - so 4 bytes per entry


    */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct BitmapHeader
    {
        public byte[] Type; // should be "BM"
        public uint FileSize;
        public ushort Reserved1;
        public ushort Reserved2;
        public uint ImageDataOffset;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct BitmapInformationHeader
    {
        public uint HeaderSize;
        public int Width;
        public int Height;
        public ushort Planes;
        public ushort BitsPerPixel;
        public uint CompressionType;
        public uint ImageSize;
        public int XResolution;
        public int YResolution;
        public uint NumberOfColors;
        public uint ImportantColors;
    }

}
