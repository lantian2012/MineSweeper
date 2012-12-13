using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AIMine.Views;
using AIMine.ViewModels;
namespace AIMine.Views
{
    public sealed partial class SimpleSettingsNarrow : UserControl
    {
        public SimpleSettingsNarrow()
        {
            this.InitializeComponent();
            settings = (Application.Current as App).SettingsViewModel;
        }

        private void MySettingsBackClicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }
            SettingsPane.Show();
        }
        private SettingsViewModel settings;

        private void ComboSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboSize.SelectedIndex)
            {
                case 0:
                    settings.m_uXnum = 10;
                    settings.m_uYnum = 10;
                    settings.m_uMineNum = 15;
                    break;
                case 1:
                    settings.m_uXnum = 16;
                    settings.m_uYnum = 16;
                    settings.m_uMineNum = 40;
                    break;
                case 2:
                    settings.m_uXnum = 20;
                    settings.m_uYnum = 20;
                    settings.m_uMineNum = 80;
                    break;
            }
        }
    }
}