using MenschensKinder.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MenschensKinder
{
    /// <summary>
    /// Klasse um die Figuren des Player und der KIPlayer verwalten zu können. Enthält z.B. die Position der jeweiligen Figure, ihr ImageDrawing, und den Verweis auf das jeweilige Bild.
    /// </summary>
    class Figure
    {
        public Coordinate2D FigureCoordinate { get; set; }
        private PlayerColor color;
        private readonly Style figureBtnStyle = App.Current.MainWindow.FindResource("NoHoverButton") as Style;
        public Button FigureButton { get; }

        /// <summary>
        /// Konstruiere ein ImageDrawing abhängig von der übergebenen Farbe.
        /// </summary>
        /// <param name="color">Die vom Spieler ausgewählte Farbe</param>
        public Figure(PlayerColor color)
        {
            this.color = color;
            // TODO: Das ImageSource soll abhängig von der Farbe sein.
            FigureButton = new Button
            {
                Content = new Image() { Source = DeterminePictureByColor(color) },
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0.0),
                Style = figureBtnStyle,
            };
            FigureButton.Click += FigureClicked;
        }

        /// <summary>
        /// Entscheide anhand der übergebenen Figur Farbe welches Bild geladen werden muss
        /// </summary>
        /// <param name="color">Farbe des Players bzw. der Figur</param>
        /// <returns>BitmapImage mit der geladenen Datei</returns>
        private BitmapImage DeterminePictureByColor(PlayerColor color)
        {
            if(color == PlayerColor.RED)
            {
                return new BitmapImage(new Uri("/img/redfigure.png", UriKind.RelativeOrAbsolute));
            }

            if (color == PlayerColor.BLUE)
            {
                return new BitmapImage(new Uri("/img/bluefigure.png", UriKind.RelativeOrAbsolute));
            }

            if (color == PlayerColor.GREEN)
            {
                return new BitmapImage(new Uri("/img/greenfigure.png", UriKind.RelativeOrAbsolute));
            }

            if (color == PlayerColor.YELLOW)
            {
                return new BitmapImage(new Uri("/img/yellowfigure.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Button Handler um auf Klick auf die Figur zu reagieren
        /// </summary>
        /// <param name="sender">Figur Button als sender</param>
        /// <param name="e"></param>
        internal void FigureClicked(Object sender, RoutedEventArgs e)
        {
            // TODO: Anhand der ausgewählten Figur entscheiden, welche Figur bewegt wird
            MessageBox.Show(this.FigureCoordinate.ToString());
        }

    }
}
