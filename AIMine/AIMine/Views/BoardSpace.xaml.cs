using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using AIMineComponent;
using AIMine.Common;
using AIMine.ViewModels;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace AIMine.Views
{
    public sealed partial class BoardSpace : UserControl
    {
        public BoardSpace()
        {
            this.InitializeComponent();
            
        }
        public int row { get; set; }
        public int column { get; set; }
        public State SpaceState
        {
            get { return (State)GetValue(SpaceStateProperty); }
            set { SetValue(SpaceStateProperty, value); }
        }
        public static DependencyProperty SpaceStateProperty =
           DependencyProperty.Register("SpaceState",
           typeof(State), typeof(BoardSpace),
           new PropertyMetadata(State.Normal, SpaceStateChanged));
        private static void SpaceStateChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            (d as BoardSpace).UpdateSpaceState(false);
        }
        private void UpdateSpaceState(bool useTransitions)
        {
            switch (SpaceState)
            {
                    /*
                case State.Normal:
                    ImageBrush brushnormal = new ImageBrush();
                    //brushnormal.ImageSource = new BitmapImage(new Uri(@"Num1.PNG", UriKind.RelativeOrAbsolute));
                    brushnormal.ImageSource = new BitmapImage(new Uri("ms_appx:///Assets/Num1.png"));
                    //brushnormal.ImageSource = new Uri("Num1.PNG" & Image, UriKind.RelativeOrAbsolute);
                    brushnormal.Stretch = Stretch.Fill;
                    break;
                case State.Num1:
                    ImageBrush brushnum1 = new ImageBrush();
                    brushnum1.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Background.png"));
                    brushnum1.Stretch = Stretch.Fill;
                    break;
                case State.Num2:
                    ImageBrush brushnum2 = new ImageBrush();
                    brushnum2.ImageSource = new BitmapImage(new Uri(@"Num2.PNG", UriKind.RelativeOrAbsolute));
                    brushnum2.Stretch = Stretch.Fill;
                    break;
                case State.Num3:
                    ImageBrush brushnum3 = new ImageBrush();
                    brushnum3.ImageSource = new BitmapImage(new Uri(@"Num3.PNG", UriKind.RelativeOrAbsolute));
                    brushnum3.Stretch = Stretch.Fill;
                    
                    break;
                 */

                case State.Num1:
                    SpaceButton.Content = "1";
                    break;
                case State.Num2:
                    SpaceButton.Content = "2";
                    break;
                case State.Num3:
                    SpaceButton.Content = "3";
                    break;
                case State.Num4:
                    SpaceButton.Content = "4";
                    break;
            }
        }

        private void SpaceButton_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSpaceState(false);
        }

        private void SpaceButton_Click(object sender, RoutedEventArgs e)
        {
            gameViewModel.leftClick(row, column);
            
        }

        public GameViewModel gameViewModel{ get; set;}

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //SpaceButton.DataContext = gameViewModel.game.m_pMines[row][column];
        }

    }

    public class StateFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            State thisState = (State)value;
            string picuture;
            switch (thisState)
            {
                case State.Normal:
                    picuture = "ms-appx:///Assets/Num1.png";
                    break;
                case State.Num1:
                    picuture = "ms-appx:///Assets/Num1.png";
                    break;
                case State.Num2:
                    picuture = "ms-appx:///Assets/Num2.png";
                    break;
                case State.Num3:
                    picuture = "ms-appx:///Assets/Num3.png";
                    break;
                default:
                    picuture = "ms-appx:///Assets/Num3.png";
                    break;
            }
            return picuture;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
