using SpecNextTiler.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SpecNextTiler.Controls
{
    public sealed partial class PaletteControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<SpecColor> colors = new ObservableCollection<SpecColor>(SpecBase256Palette.Colors);

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SpecColor> Colors
        {
            get => this.colors;
            set => this.colors = value;
        }
        public PaletteControl()
        {
            this.InitializeComponent();
            this.SelectedColor = Colors[0];
        }

        private SpecColor selectedColor;
        public SpecColor SelectedColor
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
    }
}
