using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MenschensKinder
{
    /// <summary>
    /// Interaktionslogik für GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
            InitGameboard();
        }

        private void InitGameboard()
        {
            IEnumerable<Ellipse> ellipses = CreateField(11);
            foreach (Ellipse ellipse in ellipses)
                grid.Children.Add(ellipse);   
        }

        private IEnumerable<Ellipse> CreateField(int number)
        {
            Style ellipseStyle = this.FindResource("FieldEllipse") as Style;
            Style diceBtnStyle = this.FindResource("DiceBtnStyle") as Style;
            for (int i = 0; i < number; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition()
                {
                    Width = new GridLength(grid.Width / number),
                };

                RowDefinition rowDef = new RowDefinition()
                {
                    Height = new GridLength(grid.Height / number),
                };

                grid.ColumnDefinitions.Add(colDef);
                grid.RowDefinitions.Add(rowDef);

                for(int j = 0; j < number; j++)
                {
                    SolidColorBrush colorBrush = new SolidColorBrush()
                    {
                        Color = DetermineColor(i, j),
                    };

                    Ellipse ellipse = new Ellipse()
                    {
                        Style = (i == 5 && j == 5) ? null : ellipseStyle,
                        Width = Math.Round(grid.Width / number),
                        Height = Math.Round(grid.Height / number),
                        Fill = colorBrush,
                        Visibility = DetermineVisibility(i,j) ? Visibility.Visible : Visibility.Hidden,
                    };

                    if (i == 5 && j == 5)
                    {
                        Button dice = new Button()
                        {
                            Style = diceBtnStyle,
                            FontSize = 36,
                            //Content = "Würfel",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                        };

                        Grid.SetColumn(dice, i);
                        Grid.SetRow(dice, j);
                        grid.Children.Add(dice);
                    }

                    Grid.SetColumn(ellipse, i);
                    Grid.SetRow(ellipse, j);
                    yield return ellipse;
                }
            }
        }

        private Color DetermineColor(int x, int y)
        {
            // Rotes Team
            if (((x == 0 || x == 1) && (y == 0 || y == 1)) 
                || (y == 5 && (x == 1 || x == 2|| x == 3|| x == 4)) 
                || (y == 4 && x == 0))
                return Color.FromArgb(255, 255, 0, 0);
            // Blaues Team
            else if (((x == 9 || x == 10) && (y == 0 || y == 1))
                || x == 5 && (y == 1 || y == 2 || y == 3 || y == 4)
                || (y == 0 && x == 6))
                return Color.FromArgb(255, 0, 0, 255);
            // Grünes Team
            else if (((x == 0 || x == 1) && (y == 9 || y == 10))
                || (x == 5 && (y == 6 || y == 7 || y == 8 || y == 9))
                || (y == 10 && x == 4))
                return Color.FromArgb(255, 0, 255, 0);
            // Gelbes Team
            else if (((x == 9 || x == 10) && (y == 9 || y == 10))
                || (y == 5 && (x == 6 || x == 7 || x == 8 || x == 9))
                || (y == 6 && x == 10))
                return Color.FromArgb(255, 255, 255, 0);
            else
                return Color.FromArgb(255, 255, 255, 255);
        }

        private bool DetermineVisibility(int x, int y)
        {
            if (x == 5 && y == 5)
                return false;
            else if (((x == 2 || x == 3) && (y == 0 || y == 1 || y == 2 || y == 3)) 
                || ((x == 0 || x == 1) && (y == 2 || y == 3)))
                return false;
            else if (((x == 7 || x == 8) && (y == 0 || y == 1 ||y == 2 || y == 3))
                || ((x == 9 || x == 10) && (y == 2 || y == 3)))
                return false;
            else if (((x == 2 || x == 3) && (y == 7 || y == 8 || y == 9 || y == 10))
                || ((x == 0 || x == 1) && (y == 7 || y == 8)))
                return false;
            else if (((x == 7 || x == 8) && (y == 7 || y == 8 || y == 9 || y == 10))
                || ((x == 9 || x == 10) && (y == 7 || y == 8)))
                return false;
            else
                return true;
        }
    }
}
