using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpectrumNextTools.Library.Images.Bitmaps;
using System;
using System.Linq;

namespace SpectrumNextTools.Library.Tests.Images.Bitmaps
{
    [TestClass]
    public class BitmapPaletteTests
    {
        [TestMethod]
        public void BitmapPalette_Initialize()
        {
            var palette = new BitmapPalette(new byte[] { 10, 10, 10, 10,   1, 1, 1, 1,   50, 50, 50, 50 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BitmapPalette_Initialize_Fail()
        {
            var palette = new BitmapPalette(new byte[] { 10, 10, 10, 10,   1, 1, 1, 1,   50, 50, 50 });
        }

        [TestMethod]
        public void BitmapPalette_Sort()
        {
            var palette = new BitmapPalette( GoodPaletteList());
            palette.Sort();

            Assert.AreEqual(new BitmapPaletteColor(0, 254, 255, 10), palette.Colors.First());
            Assert.AreEqual(new BitmapPaletteColor(128, 1, 23, 0), palette.Colors.Last());
        }

        [TestMethod]
        public void BitmapPalette_RemoveDuplicates()
        {
            var palette = new BitmapPalette(GoodPaletteList());

            var duplicate = new BitmapPaletteColor(10,10,10,10);

            Assert.AreEqual(duplicate, palette.Colors[3]);

            var count1 = palette.Colors.Count(c => c == duplicate);

            Assert.AreEqual(2, count1);
            palette.Sort().RemoveDuplicates();

            var count2 = palette.Colors.Count(c => c == duplicate);
            Assert.AreEqual(1, count2);

            Assert.AreEqual(5, palette.NumberOfUniqueColors);

            var black = new BitmapPaletteColor(0, 0, 0, 0);
            for (int i = palette.NumberOfUniqueColors; i < palette.Colors.Length; i++)
            {
                Assert.AreEqual(black, palette.Colors[i]);
            }
        }

        [TestMethod]
        public void BitmapPalette_ToArray()
        {
            var palette = new BitmapPalette(GoodPaletteList());
            var minimizedPalette = palette.Sort().RemoveDuplicates().ToArray();
            Assert.AreEqual(palette.Colors.Length *4, minimizedPalette.Length);

            CollectionAssert.AreEqual(palette.Colors[0].ToArray(), minimizedPalette.Take(4).ToArray());
            CollectionAssert.AreEqual(palette.Colors[2].ToArray(), minimizedPalette.Skip(8).Take(4).ToArray());
            CollectionAssert.AreEqual(palette.Colors[palette.Colors.Length -1].ToArray(), minimizedPalette.Skip((palette.Colors.Length-1)*4).Take(4).ToArray());
        }



        private static byte[] GoodPaletteList()
        {
            return new byte[]
            {
                128, 1, 23, 0,
                10, 10, 10, 10,
                5, 3, 65, 10,
                10, 10, 10, 10,
                1, 254, 33, 10,
                0, 254, 255, 10,
                0, 254, 255, 10,
            };
        }
    }
}
