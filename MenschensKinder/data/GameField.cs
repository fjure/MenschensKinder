using MenschensKinder.data;
using System;
using System.Windows;
using System.Windows.Media;

namespace MenschensKinder
{
    /// <summary>
    /// Die Klasse GameField enthält Logik Eigenschaften um die UI-Ellipse mit Logik zu füllen
    /// </summary>
    class GameField
    {
        public Coordinate2D Coordinates { get; set; }
        public GameFieldType FieldType
        {
            get => DetermineGameFieldType(this.Coordinates);
        }
        public bool IsTaken
        {
            get;
            set;
        }
        public SolidColorBrush Color { get; set; }
        public Visibility IsVisible { get; set; }


        /// <summary>
        /// Konstruiere ein neues Objekt mit einem übergebenen (X,Y)-Punkt
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-Position</param>
        public GameField(int x, int y, SolidColorBrush color, Visibility visible)
        {
            // Weise der Coordinates Eigenschaft einen Wert mit den Parametern zu.
            Coordinates = new Coordinate2D(x, y);
            Color = color;
            IsVisible = visible;
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

        private bool IsBank(Coordinate2D coordinates)
        {
            int x = coordinates.X;
            int y = coordinates.Y;
            // Überprüfe ob das GameField ein Bankfeld ist
            if ((x == 5 && (y != 0 && y != 5 && y != 10)) || (y == 5 && (x != 0 && x != 5 && x != 10)))
                return true;
            else
                return false;
        }

        private bool IsStart(Coordinate2D coordinates)
        {
            int x = coordinates.X;
            int y = coordinates.Y;
            // Überprüfe ob das GameField ein Bankfeld ist
            if ((x == 0 && y == 4) || (x == 4 && y == 10) || (x == 6 && y == 0) || (x == 10 && y == 6))
                return true;
            else
                return false;
        }

        private bool IsEndField(Coordinate2D coordinates)
        {
            int x = coordinates.X;
            int y = coordinates.Y;
            // Überprüfe ob das GameField ein Bankfeld ist
            if ((x == 5 && (y == 10 || y == 0)) || (y == 5 && (x == 10 || x == 0)))
                return true;
            else
                return false;
        }

        private GameFieldType DetermineGameFieldType(Coordinate2D coordinates)
        {
            if (IsHouse(coordinates))
                return GameFieldType.HOUSE;
            else if (IsBank(coordinates))
                return GameFieldType.BANK;
            else if (IsStart(coordinates))
                return GameFieldType.STARTFIELD;
            else if (IsEndField(coordinates))
                return GameFieldType.ENDFIELD;
            else
                return GameFieldType.FIELD;
        }

        /// <summary>
        /// Override Methode der ToString Methode. Debug-Informationen einfacher darstellen
        /// </summary>
        /// <returns>Formatierten String mit den GameField Eigenschaften</returns>
        public override string ToString()
        {
            return String.Format("{0},{1}", Coordinates.ToString(), IsTaken.ToString());
            //return String.Format("{0}", IsVisible.ToString());
        }

        





    }
}
