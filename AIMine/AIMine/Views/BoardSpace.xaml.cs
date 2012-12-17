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
        private void SpaceButton_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public GameViewModel gameViewModel{ get; set;}

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
        }

        private void UserControl_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            if (!(gameViewModel.game.m_uGameState == GameState.Lose || gameViewModel.game.m_uGameState == GameState.Victory))
            {
                Pointer ptr = e.Pointer;
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(UserControlBS);
                if (ptrPt.Properties.IsMiddleButtonPressed)
                {
                    gameViewModel.twoDown(row, column);
                    gameViewModel.towUp(row, column);
                }
                if (ptrPt.Properties.IsLeftButtonPressed)
                {
                    gameViewModel.leftDown(row, column);
                }
                if (ptrPt.Properties.IsRightButtonPressed)
                {
                    gameViewModel.rightDown(row, column);
                }
                if (gameViewModel.game.m_uGameState == GameState.Lose)
                    gameViewModel.loseDisplay();
                if (gameViewModel.game.m_uGameState == GameState.Victory)
                    gameViewModel.winDisplay();
            }

        }

        private void UserControlBS_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!(gameViewModel.game.m_uGameState == GameState.Lose || gameViewModel.game.m_uGameState == GameState.Victory))
            {
                Pointer ptr = e.Pointer;
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(UserControlBS);
                gameViewModel.rightUp(row, column);
                if (gameViewModel.game.m_uGameState == GameState.Lose)
                    gameViewModel.loseDisplay();
                if (gameViewModel.game.m_uGameState == GameState.Victory)
                    gameViewModel.winDisplay();
            }
        }

        private void SpaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(gameViewModel.game.m_uGameState == GameState.Lose || gameViewModel.game.m_uGameState == GameState.Victory))
            {
                RectSpace.Fill.Opacity = 0;
                gameViewModel.leftUp(row, column);
                if (gameViewModel.game.m_uGameState == GameState.Lose)
                    gameViewModel.loseDisplay();
                if (gameViewModel.game.m_uGameState == GameState.Victory)
                    gameViewModel.winDisplay();
                gameViewModel.leftDown(row, column);
            }
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
                case State.Num1:
                    picuture = "ms-appx:///Assets/Num1.png";
                    break;
                case State.Num2:
                    picuture = "ms-appx:///Assets/Num2.png";
                    break;
                case State.Num3:
                    picuture = "ms-appx:///Assets/Num3.png";
                    break;
                case State.Num4:
                    picuture = "ms-appx:///Assets/Num4.png";
                    break;
                case State.Num5:
                    picuture = "ms-appx:///Assets/Num5.png";
                    break;
                case State.Num6:
                    picuture = "ms-appx:///Assets/Num6.png";
                    break;
                case State.Num7:
                    picuture = "ms-appx:///Assets/Num7.png";
                    break;
                case State.Num8:
                    picuture = "ms-appx:///Assets/Num8.png";
                    break;
                case State.Dicey:
                    picuture = "ms-appx:///Assets/Dicey.png";
                    break;
                case State.Dicey_down:
                    picuture = "ms-appx:///Assets/Dicey.png";
                    break;
                case State.Flag:
                    picuture = "ms-appx:///Assets/Flag.png";
                    break;
                case State.Normal:
                    picuture = "ms-appx:///Assets/Normal.png";
                    break;
                case State.Empty:
                    picuture = "ms-appx:///Assets/Empty.png";
                    break;
                case State.Mine:
                    picuture = "ms-appx:///Assets/Mine.png";
                    break;
                case State.Blast:
                    picuture = "ms-appx:///Assets/Blast.png";
                    break;
                default:
                    picuture = "ms-appx:///Assets/Num1.png";
                    break;
            }
            return picuture;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class EstimationFormatterColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Estimation thisEstimation = (Estimation)value;
            string color;
            switch (thisEstimation)
            {
                case Estimation.Mine:
                    color = "#FFD86780";
                    break;
                case Estimation.Empty:
                    color = "#FF998EDC";
                    break;
                case Estimation.HighestEmpty:
                    color ="#FF55FFFF";
                    break;
                default:
                    color = "#FF998EDC";
                    break;
            }
            return color;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class EstimationFormatterOpa : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Estimation thisEstimation = (Estimation)value;
            string opa;
            switch (thisEstimation)
            {
                case Estimation.Mine:
                    opa = "0.5";
                    break;
                case Estimation.Empty:
                    opa = "0.5";
                    break;
                case Estimation.HighestEmpty:
                    opa = "0.5";
                    break;
                default:
                    opa = "0";
                    break;
            }
            return opa;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


}
