using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecNextTiler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpecNextTiler.Tests.Models
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
        public void Constructor_ByteArray()
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
            var actual = tile.ApplyTransform(rotate: false, mirrorX: false, mirrorY: false);
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

            //byte[] expected = new byte[]
            //{
            //    0,1,0,1,0,1,1,1,
            //    0,1,0,1,0,1,1,1,
            //    1,1,1,1,1,1,1,1,
            //    0,1,0,1,0,1,1,1,
            //    0,1,0,1,0,1,1,1,
            //    0,1,0,1,0,1,1,1,
            //    0,1,0,1,0,1,1,1,
            //    0,1,0,1,0,1,1,1,
            //};

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
            var actual = tile.ApplyTransform(rotate: false, mirrorX: true, mirrorY: false);
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
                0,1,0,1,0,1,1,1,
                0,1,0,1,0,1,1,1,
                1,1,1,1,1,1,1,1,
                0,1,0,1,0,1,1,1,
                0,1,0,1,0,1,1,1,
                0,1,0,1,0,1,1,1,
                0,1,0,1,0,1,1,1,
                0,1,0,1,0,1,1,1,
            };
            var tile = new Tile(pixels);
            var actual = tile.ApplyTransform(rotate: false, mirrorX: false, mirrorY: true);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
