using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectrumNextTools.Library.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpectrumNextTools.Library.Tests.Models
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void Tile_Constructor_Default()
        {
            var tile = new Tile();
            Assert.IsNotNull(tile);
        }

        [TestMethod]
        public void Tile_Constructor_ByteArray()
        {
            byte[] pixels = new byte[]
            {
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
            };

            var tile = new Tile(pixels);
            Assert.IsNotNull(tile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Tile_Constructor_ByteArray_Fail_WrongNumberOfItems()
        {
            byte[] pixels = new byte[]
            {
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
                0,1,0,1,0,1,0,1,
            };

            var tile = new Tile(pixels);
            Assert.IsNotNull(tile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Tile_Constructor_ByteArray_Fail_Null()
        {
            byte[] pixels = null;

            var tile = new Tile(pixels);
            Assert.IsNotNull(tile);
        }

        [TestMethod]
        public void Tile_Constructor_MultiDimensionByteArray()
        {
            byte[,] pixels = new byte[,]
            {
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
                { 1,1,1,1,1,1,1,1, },
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
            };

            var tile = new Tile(pixels);
            Assert.IsNotNull(tile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Tile_Constructor_MultiDimensionByteArray_Fail_ShortRow()
        {
            byte[,] pixels = new byte[,]
            {
                { 0,1,1,1,1,0,1, },
                { 0,1,1,0,1,0,1, },
                { 1,1,1,1,1,1,1, },
                { 0,1,1,1,1,0,1, },
                { 0,1,1,1,1,0,1, },
                { 0,1,1,1,1,0,1, },
                { 0,1,1,1,1,0,1, },
                { 0,1,1,1,1,0,1, },
            };

            var tile = new Tile(pixels);
            Assert.IsNotNull(tile);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Tile_Constructor_MultiDimensionByteArray_Fail_TooFewtRows()
        {
            byte[,] pixels = new byte[,]
            {
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
                { 1,1,1,1,1,1,1,1, },
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
                { 0,1,1,1,0,1,0,1, },
            };

            var tile = new Tile(pixels);
            Assert.IsNotNull(tile);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R0X0Y0()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: false, mirrorX: false, mirrorY: false);
            CollectionAssert.AreEqual(expected, actual);
            var matchTile = new Tile(actual);

            var matchResult = tile.Match(matchTile, out TileOrientation tileOrientation);
            Assert.IsTrue(matchResult);
            Assert.IsTrue(!tileOrientation.Rotate && !tileOrientation.MirrorX && !tileOrientation.MirrorY);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R1X0Y0()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                0,0,0,0,0,1,0,0,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                0,0,0,0,0,1,0,0,
                1,1,1,1,1,1,1,1,
                0,0,0,0,0,1,0,0,
                1,1,1,1,1,1,1,1,
            };

            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: true, mirrorX: false, mirrorY: false);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R0X1Y0()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,1,1,1,1,1,1,1,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
            };

            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: false, mirrorX: true, mirrorY: false);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R0X1Y1()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
                1,1,1,1,1,1,1,1,
                1,0,1,0,1,1,1,0,
                1,0,1,0,1,1,1,0,
            };

            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: false, mirrorX: true, mirrorY: true);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R1X1Y0()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                0,0,1,0,0,0,0,0,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                0,0,1,0,0,0,0,0,
                1,1,1,1,1,1,1,1,
                0,0,1,0,0,0,0,0,
                1,1,1,1,1,1,1,1,
            };

            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: true, mirrorX: true, mirrorY: false);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R0X0Y1()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };
            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: false, mirrorX: false, mirrorY: true);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R1X0Y1()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                1,1,1,1,1,1,1,1,
                0,0,0,0,0,1,0,0,
                1,1,1,1,1,1,1,1,
                0,0,0,0,0,1,0,0,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                0,0,0,0,0,1,0,0,
            };
            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: true, mirrorX: false, mirrorY: true);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tile_ApplyTransform_R1X1Y1()
        {
            byte[] pixels = new byte[]
            {
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                1,1,1,1,1,1,1,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
                0,1,1,1,0,1,0,1,
            };

            byte[] expected = new byte[]
            {
                1,1,1,1,1,1,1,1,
                0,0,1,0,0,0,0,0,
                1,1,1,1,1,1,1,1,
                0,0,1,0,0,0,0,0,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,
                0,0,1,0,0,0,0,0,
            };

            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: true, mirrorX: true, mirrorY: true);
            CollectionAssert.AreEqual(expected, actual);
        }

    }
}
