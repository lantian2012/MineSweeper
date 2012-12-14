using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using AIMine.Views;
using AIMine.ViewModels;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace AIMine
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool settingPanelInitialized = false;
        Rect _windowBounds;
        double _settingsWidth = 346;
        Popup _settingsPopup;


        public MainPage()
        {
            this.InitializeComponent();
            var _settingsViewModel = (Application.Current as App).SettingsViewModel;
            _windowBounds = Window.Current.Bounds;
            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;

        
        }

        void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (!settingPanelInitialized)
            {
                SettingsCommand cmd = new SettingsCommand("AIMine", "Layout Options", (x) =>
                {
                    _settingsPopup = new Popup();
                    _settingsPopup.Closed += OnPopupClosed;
                    Window.Current.Activated += OnWindowActivated;
                    _settingsPopup.IsLightDismissEnabled = true;
                    _settingsPopup.Width = _settingsWidth;
                    _settingsPopup.Height = _windowBounds.Height;

                    SimpleSettingsNarrow mypane = new SimpleSettingsNarrow();
                    mypane.Width = _settingsWidth;
                    mypane.Height = _windowBounds.Height;

                    _settingsPopup.Child = mypane;
                    _settingsPopup.SetValue(Canvas.LeftProperty, _windowBounds.Width - _settingsWidth);
                    _settingsPopup.SetValue(Canvas.TopProperty, 0);
                    _settingsPopup.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(cmd);
                settingPanelInitialized = true;
            }
        }
        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                _settingsPopup.IsOpen = false;
            }
        }
        void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }






        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
        }

        private SettingsViewModel _settingsViewModel { get; set; }


    }
}
