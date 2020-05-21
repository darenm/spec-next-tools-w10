using SpecNextTiler.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using System.Text;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpecNextTiler.Controls
{
    public sealed partial class PaletteControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<WinSpecColor> colors = new ObservableCollection<WinSpecColor>(SpecBase256Palette.Colors);

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<WinSpecColor> Colors
        {
            get => this.colors;
            set
            {
                this.colors = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Colors)));
            }
        }
        public PaletteControl()
        {
            this.InitializeComponent();
            this.SelectedColor = Colors[0];
        }

        private WinSpecColor selectedColor;
        public WinSpecColor SelectedColor
        {
            get => this.selectedColor;
            set
            {
                if (this.selectedColor != value)
                {
                    this.selectedColor = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedColor)));
                }
            }
        }

#pragma warning disable CA1822 // Mark members as static - used in binding
        public string FormatIndex(int value)
#pragma warning restore CA1822 // Mark members as static
        {
            return $"INDEX: {value:X2}";
        }
#pragma warning disable CA1822 // Mark members as static - used in binding
        public string FormatOffset(int value)
#pragma warning restore CA1822 // Mark members as static
        {
            var paletteOffset = value / 16;
            var colorIndex = value % 16;
            return $"Off: {paletteOffset:X2} Ind: {colorIndex:X2}";
        }

        private void Export8BitColorAsm(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            var sb = new StringBuilder();
            for (int i = 0; i < colors.Count; i++)
            {
                sb.AppendLine($"    db ${colors[i].EightBitColor:X2}    ; ({i:D2})  ({i:X2})");
            }
            dataPackage.SetText(sb.ToString());
            Clipboard.SetContent(dataPackage);
        }

        private void Export9BitColorAsm(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            var sb = new StringBuilder();
            for (int i = 0; i < colors.Count; i++)
            {
                sb.AppendLine($"    db ${colors[i].EightBitColor:X2}, ${colors[i].NineBitColor:X2}    ; ({i:D2})  ({i:X2})");
            }
            dataPackage.SetText(sb.ToString());
            Clipboard.SetContent(dataPackage);
        }
    }
}
