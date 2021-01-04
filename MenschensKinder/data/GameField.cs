using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace MenschensKinder
{
    /// <summary>
    /// Die Klasse GameField enthält Logik Eigenschaften um die UI-Ellipse mit Logik zu füllen
    /// </summary>
    class GameField
    {
        public Coordinate2D Coordinates { get; }
        private bool House
        {
            get => IsHouse(this.Coordinates);
        }


        /// <summary>
        /// Konstruiere ein neues Objekt mit einem übergebenen (X,Y)-Punkt
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-Position</param>
        public GameField(int x, int y)
        {
            // Weise der Coordinates Eigenschaft einen Wert mit den Parametern zu.
            Coordinates = new Coordinate2D(x, y);
        }

        /// <summary>
        /// Überprüft ob das GameField ein Haus ist
        /// </summary>
        /// <param name="coordinates">Die Koordinaten des GameFields, bzw. der Ellipse</param>
        /// <returns></returns>
        private bool IsHouse(Coordinate2D coordinates)
        {
            int x = coordinates.X;
            int y = coordinates.Y;
            // Überprüfe ob das GameField in den Ecken liegt
            if ((x == 0 || x == 1 || x == 9 || x == 10) && (y == 0 || y == 1 || y == 9 || y == 10))
                return true;
            else
                return false;
            
        }
        /// <summary>
        /// Override Methode der ToString Methode. Debug-Informationen einfacher darstellen
        /// </summary>
        /// <returns>Formatierten String mit den GameField Eigenschaften</returns>
        public override string ToString()
        {
            return String.Format("{0},{1}", Coordinates.ToString(), (House) ? "T" : "F");
        }





    }
}
