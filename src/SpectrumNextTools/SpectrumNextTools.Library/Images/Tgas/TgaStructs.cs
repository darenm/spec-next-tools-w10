using System;
using System.Runtime.InteropServices;

namespace SpectrumNextTools.Library
{
    /*
    Source - http://paulbourke.net/dataformats/tga/

    DATA TYPE 1: Color-mapped images
    ________________________________________________________________________________
    | Offset | Length |                     Description                            |
    |--------|--------|------------------------------------------------------------|
    |    0   |     1  |  Number of Characters in Identification Field.             |
    |        |        |                                                            |
    |        |        |  This field is a one-byte unsigned integer, specifying     |
    |        |        |  the length of the Image Identification Field.  Its range  |
    |        |        |  is 0 to 255.  A value of 0 means that no Image            |
    |        |        |  Identification Field is included.                         |
    |--------|--------|------------------------------------------------------------|
    |    1   |     1  |  Color Map Type.                                           |
    |        |        |                                                            |
    |        |        |  This field contains a binary 1 for Data Type 1 images.    |
    |--------|--------|------------------------------------------------------------|
    |    2   |     1  |  Image Type Code.                                          |
    |        |        |                                                            |
    |        |        |  This field will always contain a binary 1.                |
    |        |        |  ( That's what makes it Data Type 1 ).                     |
    |--------|--------|------------------------------------------------------------|
    |    3   |     5  |  Color Map Specification.                                  |
    |        |        |                                                            |
    |    3   |     2  |  Color Map Origin.                                         |
    |        |        |  Integer ( lo-hi ) index of first color map entry.         |
    |    5   |     2  |  Color Map Length.                                         |
    |        |        |  Integer ( lo-hi ) count of color map entries.             |
    |    7   |     1  |  Color Map Entry Size.                                     |
    |        |        |  Number of bits in each color map entry.  16 for           |
    |        |        |  the Targa 16, 24 for the Targa 24, 32 for the Targa 32.   |
    |--------|--------|------------------------------------------------------------|
    |    8   |    10  |  Image Specification.                                      |
    |        |        |                                                            |
    |    8   |     2  |  X Origin of Image.                                        |
    |        |        |  Integer ( lo-hi ) X coordinate of the lower left corner   |
    |        |        |  of the image.                                             |
    |   10   |     2  |  Y Origin of Image.                                        |
    |        |        |  Integer ( lo-hi ) Y coordinate of the lower left corner   |
    |        |        |  of the image.                                             |
    |   12   |     2  |  Width of Image.                                           |
    |        |        |  Integer ( lo-hi ) width of the image in pixels.           |
    |   14   |     2  |  Height of Image.                                          |
    |        |        |  Integer ( lo-hi ) height of the image in pixels.          |
    |   16   |     1  |  Image Pixel Size.                                         |
    |        |        |  Number of bits in a stored pixel index.                   |
    |   17   |     1  |  Image Descriptor Byte.                                    |
    |        |        |  Bits 3-0 - number of attribute bits associated with each  |
    |        |        |             pixel.                                         |
    |        |        |  Bit 4    - reserved.  Must be set to 0.                   |
    |        |        |  Bit 5    - screen origin bit.                             |
    |        |        |             0 = Origin in lower left-hand corner.          |
    |        |        |             1 = Origin in upper left-hand corner.          |
    |        |        |             Must be 0 for Truevision images.               |
    |        |        |  Bits 7-6 - Data storage interleaving flag.                |
    |        |        |             00 = non-interleaved.                          |
    |        |        |             01 = two-way (even/odd) interleaving.          |
    |        |        |             10 = four way interleaving.                    |
    |        |        |             11 = reserved.                                 |
    |        |        |  This entire byte should be set to 0.  Don't ask me.       |
    |--------|--------|------------------------------------------------------------|
    |   18   | varies |  Image Identification Field.                               |
    |        |        |                                                            |
    |        |        |  Contains a free-form identification field of the length   |
    |        |        |  specified in byte 1 of the image record.  It's usually    |
    |        |        |  omitted ( length in byte 1 = 0 ), but can be up to 255    |
    |        |        |  characters.  If more identification information is        |
    |        |        |  required, it can be stored after the image data.          |
    |--------|--------|------------------------------------------------------------|
    | varies | varies |  Color map data.                                           |
    |        |        |                                                            |
    |        |        |  The offset is determined by the size of the Image         |
    |        |        |  Identification Field.  The length is determined by        |
    |        |        |  the Color Map Specification, which describes the          |
    |        |        |  size of each entry and the number of entries.             |
    |        |        |  Each color map entry is 2, 3, or 4 bytes.                 |
    |        |        |  Unused bits are assumed to specify attribute bits.        |
    |        |        |  The 4 byte entry contains 1 byte for blue, 1 byte         |
    |        |        |  for green, 1 byte for red, and 1 byte of attribute        |
    |        |        |  information, in that order.                               |
    |        |        |  The 3 byte entry contains 1 byte each of blue, green,     |
    |        |        |  and red.                                                  |
    |        |        |  The 2 byte entry is broken down as follows:               |
    |        |        |  ARRRRRGG GGGBBBBB, where each letter represents a bit.    |
    |        |        |  But, because of the lo-hi storage order, the first byte   |
    |        |        |  coming from the file will actually be GGGBBBBB, and the   |
    |        |        |  second will be ARRRRRGG. "A" represents an attribute bit. |
    |--------|--------|------------------------------------------------------------|
    | varies | varies |  Image Data Field.                                         |
    |        |        |                                                            |
    |        |        |  This field specifies (width) x (height) color map         |
    |        |        |  indices.  Each index is stored as an integral number      |
    |        |        |  of bytes (typically 1 or 2).   All fields are unsigned.   |
    |        |        |  The low-order byte of a two-byte field is stored first.   |
    --------------------------------------------------------------------------------
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TgaHeader
    {
        public byte IdLength;
        public byte ColorMapType;
        public byte ImageType;
        public ColorMapSpec ColorMapSpecification;
        public ImageSpec ImageSpecification;
        public TgaColorMap ColorMapData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorMapSpec
    {

        public ushort FirstEntryIndex;
        public ushort ColorMapLength;
        public byte ColorMapEntrySize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageSpec
    {
        public ushort XOrigin;
        public ushort YOrigin;
        public ushort ImageWidth;
        public ushort ImageHeight;
        public byte PixelDepth;
        public byte ImageDescriptor;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TgaColorMap
    {
        public TgaColor[] TgaColors;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TgaColor
    {
        public byte Blue;
        public byte Green;
        public byte Red;
    }

}
