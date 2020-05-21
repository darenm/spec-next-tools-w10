using SpectrumNextTools.Library.Images.Bitmaps;
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

        private bool _createNewFile;
        private bool _updateImageData;

        public NextBmpConverter(BitmapOptions bmpOptions)
        {
            _bmpOptions = bmpOptions;
        }

        public BitmapImage InBitmap => this._inBitmap;

        public void ProcessImage()
        {
            _createNewFile = (!string.IsNullOrWhiteSpace(_bmpOptions.OutFileName)) && (!_bmpOptions.InFileName.Equals(_bmpOptions.OutFileName));
            _updateImageData = _bmpOptions.MinimizePalette || _bmpOptions.UseStdPalette;

            try
            {
                _inBitmap = new BitmapImage(_bmpOptions.InFileName);
                _inBitmap.Validate();
                _inBitmap.LoadPaletteAndImageData(_createNewFile, _updateImageData);
                _inBitmap.UpdateColors(_bmpOptions.UseStdPalette, _bmpOptions.RoundingMode, _bmpOptions.MinimizePalette);

                if (_createNewFile)
                {
                    _inBitmap.SaveAs(_bmpOptions.OutFileName);
                }
                else
                {
                    _inBitmap.Save();
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to process BMP {_bmpOptions.InFileName} - {ex.Message}", ex);
            }
        }
    }
}
