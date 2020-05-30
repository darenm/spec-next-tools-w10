using System;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Template10.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SpecNextTiler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public event EventHandler<Exception> NavigationFailed;

        public ShellPage()
        {
            InitializeComponent();
        }

        public new Frame Frame
        {
            get => MainNavigationView.Content as Frame;
            set => MainNavigationView.Content = value;
        }

        private async void MainNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // TODO:
            }
            else
            {
                switch (args.InvokedItemContainer)
                {
                    case NavigationViewItem item when Equals(item, HomeNavigationViewItem):
                        {
                            await NavigateAsync(nameof(MainPage));
                            break;
                        }
                    case NavigationViewItem item when Equals(item, LoadTileSetNavigationViewItem):
                        {
                            await NavigateAsync(nameof(LoadTileSetPage));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private async Task NavigateAsync(string key)
        {
            var navigation = Frame.GetNavigationService();
            var result = await navigation.NavigateAsync(key);
            if (result.Success)
            {
                return;
            }
            NavigationFailed?.Invoke(Frame, result.Exception);
        }
    }
}
