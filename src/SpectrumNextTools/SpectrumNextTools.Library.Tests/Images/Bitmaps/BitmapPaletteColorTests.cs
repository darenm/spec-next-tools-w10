using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpectrumNextTools.Library.Images.Bitmaps;

namespace SpectrumNextTools.Library.Tests.Images.Bitmaps
{
    [TestClass]
    public class BitmapPaletteColorTests
    {
        [TestMethod]
        public void BitmapPaletteColor_Initialize_Bytes()
        {
            var color = new BitmapPaletteColor(128, 128, 128, 0);
            Assert.IsNotNull(color);
        }

        [TestMethod]
        public void BitmapPaletteColor_Initialize_Byte_Array()
        {
            var color1 = new BitmapPaletteColor(new byte[] { 1, 2, 3, 4 }, 0);

            Assert.AreEqual(1, color1.R);
            Assert.AreEqual(2, color1.G);
            Assert.AreEqual(3, color1.B);
            Assert.AreEqual(4, color1.A);

            var color2 = new BitmapPaletteColor(new byte[] { 128, 128, 128, 0, 1, 2, 3, 4 }, 4 * 1);

            Assert.AreEqual(1, color2.R);
            Assert.AreEqual(2, color2.G);
            Assert.AreEqual(3, color2.B);
            Assert.AreEqual(4, color2.A);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]

        public void BitmapPaletteColor_Initialize_Byte_Array_Invalid_Length()
        {
            var color1 = new BitmapPaletteColor(new byte[] { 1, 2, 3 }, 0);

            Assert.AreEqual(1, color1.R);
            Assert.AreEqual(2, color1.G);
            Assert.AreEqual(3, color1.B);
            Assert.AreEqual(4, color1.A);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BitmapPaletteColor_Initialize_Byte_Array_Invalid_Offset()
        {
            var color1 = new BitmapPaletteColor(new byte[] { 128, 128, 128, 0, 1, 2, 3, 4 }, 1);

            Assert.AreEqual(1, color1.R);
            Assert.AreEqual(2, color1.G);
            Assert.AreEqual(3, color1.B);
            Assert.AreEqual(4, color1.A);
        }

        [TestMethod]
        public void BitmapPaletteColor_Equals()
        {
            var color1 = new BitmapPaletteColor(new byte[] { 10, 10, 10, 0 }, 0);
            var color2 = new BitmapPaletteColor(new byte[] { 20, 20, 20, 0 }, 0);
            var color3 = new BitmapPaletteColor(10, 10, 10, 0);

            Assert.IsFalse(color1.Equals(color2));
            Assert.IsFalse(color1 == color2);
            Assert.IsTrue(color1 != color2);
            Assert.IsTrue(color1 == color3);
            Assert.IsTrue(color1.Equals(color3));
        }

        [TestMethod]
        public void BitmapPaletteColor_CompareTo()
        {
            var color1 = new BitmapPaletteColor(new byte[] { 10, 10, 10, 0 }, 0);
            var color2 = new BitmapPaletteColor(new byte[] { 20, 20, 20, 0 }, 0);
            var color3 = new BitmapPaletteColor(new byte[] { 10, 10, 10, 0 }, 0);

            var color4 = new BitmapPaletteColor(new byte[] { 1, 2, 3, 0 }, 0);
            var color5 = new BitmapPaletteColor(new byte[] { 0, 255, 255, 0 }, 0);

            Assert.IsTrue(color1.CompareTo(color2) == -1);
            Assert.IsTrue(color2.CompareTo(color1) == 1);
            Assert.IsTrue(color1.CompareTo(color3) == 0);

            Assert.IsTrue(color4.CompareTo(color5) == 1);
        }
    }
}
