using Prism.Ioc;
using SpecNextTiler.ViewModel;
using SpecNextTiler.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Template10;
using Template10.Ioc;
using Template10.Navigation;
using Template10.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SpecNextTiler
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : ApplicationBase
    {
        private INavigationService _nav;

        public App() => InitializeComponent();

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterView<MainPage, MainPageViewModel>();
        }

        public override void OnInitialized()
        {
            var frame = new Frame();
            Window.Current.Content = new ShellPage { Frame = frame };
            Window.Current.Activate();
            _nav = NavigationFactory
                .Create(frame, Guid.Empty.ToString())
                .AttachGestures(Window.Current, Gesture.Back, Gesture.Forward, Gesture.Refresh);
        }

        public override async Task OnStartAsync(IStartArgs args)
        {
            if (args.StartKind == StartKinds.Launch)
            {
                await _nav.NavigateAsync(nameof(MainPage));
            }
        }
    }
}
