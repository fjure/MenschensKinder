using MenschensKinder.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MenschensKinder
{
    /// <summary>
    /// Die Klasse Player soll Eigenschaften des logischen Spielers enthalten, wie z.B. Farbe, seine Figuren usw.
    /// </summary>
    class Player
    {
        private bool hasInit;

        public int Order { get; set; }
        public PlayerColor Color { get; }
        // Dictionary um jeder Integer ID einer Figure zuzordnen. (1 -> Figure1, 2 -> Figure2, usw.)
        private readonly IDictionary<int, Figure> figures = new Dictionary<int, Figure>();
        public List<Coordinate2D> StartCoordinates { get => DetermineStartCoordinatesByColor(Color); } 
        public bool IsInit {
            get => this.hasInit;
            set => this.hasInit = value;
        }
        public int Turns { get; set; }

        /// <summary>
        /// Konstruiere ein Player Objekt mit der übergebenen Color
        /// </summary>
        /// <param name="color">Die gewählte Farbe</param>
        public Player(PlayerColor color)
        {
            this.Color = color;
            this.Turns = 3;
            ConstructFigures(color);
        }

        /// <summary>
        /// Konstruiere die Figuren des Spielers. Dies ist die Erstinitialisierung
        /// </summary>
        /// <param name="color">Die Farbe des Spielers, um zu entscheiden wo die Erstkoordinaten der Figuren ist</param>
        private void ConstructFigures(PlayerColor color)
        {
            // Wandel die Liste mit den Startkoordinaten in ein Array um, da die Elemente in der Zählschleife leichter indexierbar ist.
            Coordinate2D[] coordinate = DetermineStartCoordinatesByColor(color).ToArray();
            if(!hasInit)
            {
                for(int i = 1; i <= 4; i++)
                {
                    figures.Add(i, new Figure(color));
                    Figure figure = ReturnFigure(i);
                    figure.StartCoordinate = coordinate[i - 1];
                    figure.FigureCoordinate = coordinate[i - 1];
                }
            }
        }

        public void DisableFigures()
        {
            foreach(Figure figure in figures.Values)
            {
                figure.FigureButton.IsEnabled = false;
            }
        }

        public void EnableFigures()
        {
            foreach(Figure figure in figures.Values)
            {
                figure.FigureButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Entscheidet anhand der Spielerfarbe welche Startkoordinaten die Figuren haben sollen
        /// </summary>
        /// <param name="color">Gewählte Spielerfarbe</param>
        /// <returns>Eine Liste mit den Startkoordinaten</returns>
        private List<Coordinate2D> DetermineStartCoordinatesByColor(PlayerColor color)
        {
            List<Coordinate2D> startCoordinates = new List<Coordinate2D>();
            if(color == PlayerColor.RED)
            {
                startCoordinates.Add(new Coordinate2D(0, 0));
                startCoordinates.Add(new Coordinate2D(0, 1));
                startCoordinates.Add(new Coordinate2D(1, 0));
                startCoordinates.Add(new Coordinate2D(1, 1));
                return startCoordinates;
            }

            if(color == PlayerColor.BLUE)
            {
                startCoordinates.Add(new Coordinate2D(9, 0));
                startCoordinates.Add(new Coordinate2D(9, 1));
                startCoordinates.Add(new Coordinate2D(10, 0));
                startCoordinates.Add(new Coordinate2D(10, 1));
                return startCoordinates;
            }

            if (color == PlayerColor.GREEN)
            {
                startCoordinates.Add(new Coordinate2D(0, 9));
                startCoordinates.Add(new Coordinate2D(0, 10));
                startCoordinates.Add(new Coordinate2D(1, 9));
                startCoordinates.Add(new Coordinate2D(1, 10));
                return startCoordinates;
            }

            if (color == PlayerColor.YELLOW)
            {
                startCoordinates.Add(new Coordinate2D(9, 9));
                startCoordinates.Add(new Coordinate2D(9, 10));
                startCoordinates.Add(new Coordinate2D(10, 9));
                startCoordinates.Add(new Coordinate2D(10, 10));
                return startCoordinates;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gibt die Figure für die jeweilige ID wieder.
        /// </summary>
        /// <param name="id">ID der Figure, der Schlüssel im Dictionary</param>
        /// <returns>Die Figure zur ID, den Wert im Dictionary</returns>
        public Figure ReturnFigure(int id)
        {
            // Überprüfe ob die ID vorhanden ist
            if(figures.ContainsKey(id))
            {
                // Extrahiere die Figure zur übergebenen ID
                if(figures.TryGetValue(id, out Figure figure))
                {
                    return figure;
                }
            }
            return null;
        }

        /// <summary>
        /// Gibt die Werte des Dictionaries (also alle Figures) als Liste wieder.
        /// </summary>
        /// <returns>Values (Figuren) umgewandelt in eine Liste</returns>
        public List<Figure> ReturnFigures()
        {
            return figures.Values.ToList();
        }

    }
}
