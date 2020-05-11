using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpecNextColorConverter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Convert_Click(object sender, RoutedEventArgs e)
        {
            var cd = new ContentDialog();
            cd.PrimaryButtonText = "Ok";
            cd.IsPrimaryButtonEnabled = true;

            var isError = false;

            if (string.IsNullOrWhiteSpace(this.RGBInHex.Text))
            {
                cd.Content = "Must supply a color";
                isError = true;
            }
            if (!isError && this.RGBInHex.Text.Length != 6)
            {
                cd.Content = "Must supply a color as 6 Hex characters - FF111A";
                isError = true;
            }

            if (!isError)
            {
                try
                {
                    Result.Text = ConvertRGBHex(this.RGBInHex.Text);
                }
                catch (Exception ex)
                {
                    cd.Content = ex.Message;
                    isError = true;
                }
            }

            if (isError)
            {
                await cd.ShowAsync();
            }
        }

        private string ConvertRGBHex(string text)
        {
            var rHex = text.Substring(0, 2);
            var gHex = text.Substring(2, 2);
            var bHex = text.Substring(4, 2);
            var r = ConvertHexByte(rHex);
            var g = ConvertHexByte(gHex);
            var b = ConvertHexByte(bHex);

            r = r & 0b1110_0000;
            g = g & 0b1110_0000;
            b = b & 0b1110_0000;

            r = r >> 5;
            g = g >> 5;
            b = b >> 5;

            return $"{r}, {g}, {b}";
        }

        private int ConvertHexByte(string hex)
        {
            if (hex.Length != 2)
            {
                throw new ArgumentException("hex should be 2 characters in length");
            }

            return int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
        }
    }
}
