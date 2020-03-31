﻿using SpectrumNextTools.Library.Images.Bitmaps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectrumNextTools.Library
{
    public class NextBmpConverter
    {
        private readonly BitmapOptions _bmpOptions;
        private BitmapImage _inBitmap;
        private BitmapImage _outBitmap;

        private bool _createNewFile;
        private bool _updateImageData;

        public NextBmpConverter(BitmapOptions bmpOptions)
        {
            _bmpOptions = bmpOptions;
        }

        public BitmapImage InBitmap => this._inBitmap;
        public BitmapImage OutBitmap => this._outBitmap;

        public void ProcessImage()
        {
            _createNewFile = (!string.IsNullOrWhiteSpace(_bmpOptions.OutFileName)) && (!_bmpOptions.InFileName.Equals(_bmpOptions.OutFileName));
            _updateImageData = _bmpOptions.MinimizePalette || _bmpOptions.UseStdPalette;

            try
            {
                _inBitmap = new BitmapImage(_bmpOptions.InFileName);
                _inBitmap.Validate();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
