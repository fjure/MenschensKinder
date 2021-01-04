using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string color;
        public ImageDrawing Drawing
        {
            get;
        }

        /// <summary>
        /// Konstruiere ein ImageDrawing abhängig von der übergebenen Farbe.
        /// </summary>
        /// <param name="color">Die vom Spieler ausgewählte Farbe</param>
        public Figure(string color)
        {
            this.color = color;
            // TODO: Das ImageSource soll abhängig von der Farbe sein.
            Drawing = new ImageDrawing
            {
                ImageSource = new BitmapImage(new Uri("/img/redfigure.png", UriKind.RelativeOrAbsolute))
            };
        }
        

    }
}
