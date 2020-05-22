using System;
using System.Collections.Generic;
using System.Text;

namespace SpectrumNextTools.Library.Models
{
    public class Nibble
    {
        private readonly byte _nibble;

        public byte AsLowNibble => (byte)(_nibble & 0b0000_1111);

        public byte AsHighNibble => (byte)((_nibble << 4) & 0b1111_0000);

        public Nibble(int nibble) : this((byte)nibble)
        { }

        public Nibble(byte nibble)
        {
            if (nibble > 15)
            {
                throw new ArgumentOutOfRangeException(nameof(nibble), "Value cannot exceed 15");
            }

            _nibble = nibble;
        }

        public static byte Combine(Nibble highNibble, Nibble lowNibble)
        {
            return (byte)(highNibble.AsHighNibble + lowNibble.AsLowNibble);
        }

    }
}
