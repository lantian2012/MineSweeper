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
using AIMine.ViewModels;
using AIMine.Common;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace AIMine.Views
{
    public sealed partial class Board : UserControl
    {
        public Board()
        {
            this.InitializeComponent();
            ;
        }


        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", 
            typeof(int), typeof(Board), new PropertyMetadata(16));

        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.Register("ColumnCount",
            typeof(int), typeof(Board), new PropertyMetadata(16));

        public int ColumnCount 
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            gameViewModel = DataContext as GameViewModel;
            var rows = Enumerable.Range(0, RowCount).ToArray();
            var columns = Enumerable.Range(0, ColumnCount).ToArray();
            foreach (var row in rows) BoardGrid.RowDefinitions.Add(new RowDefinition());
            foreach (var column in columns) BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            foreach (var row in rows)
            {
                foreach (var column in columns)
                {
                    var boardViewBox = new Viewbox();
                    var boardSpace = new BoardSpace();
                    boardSpace.row = row;
                    boardSpace.column = column;
                    boardSpace.gameViewModel = gameViewModel;
                    //boardSpace.SetBinding(BoardSpace.SpaceStateProperty,
                    //   new Binding { Path = new PropertyPath(String.Format("game.m_pMines[{0}][{1}].uState", row, column)) });
                    //boardSpace.SetBinding(BoardSpace.SpaceStateProperty,
                    //   new Binding { Path = new PropertyPath("teststate") });
                    
                    boardViewBox.Child = boardSpace;
                    boardViewBox.Stretch = Stretch.Fill;
                    Grid.SetRow(boardViewBox, row);
                    Grid.SetColumn(boardViewBox, column);
                    BoardGrid.Children.Add(boardViewBox);
                    boardSpace.DataContext = gameViewModel.game.m_pMines[row][column];
                   
                }
            }
        }

        private GameViewModel gameViewModel;
    }
}
