using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschensKinder
{
    /// <summary>
    /// Die Klasse Player soll Eigenschaften des logischen Spielers enthalten, wie z.B. Farbe, seine Figuren usw.
    /// </summary>
    class Player
    {
        private bool hasInit;

        public string Color { get; }
        // Dictionary um jeder Integer ID einer Figure zuzordnen. (1 -> Figure1, 2 -> Figure2, usw.)
        private IDictionary<int, Figure> figures = new Dictionary<int, Figure>();
        public bool IsInit {
            get => this.hasInit;
            set => this.hasInit = value;
        }

        /// <summary>
        /// Konstruiere ein Player Objekt mit der übergebenen Color
        /// </summary>
        /// <param name="color">Die gewählte Farbe</param>
        public Player(string color)
        {
            this.Color = color;
            ConstructFigures(color);
        }

        /// <summary>
        /// Konstruiere die Figuren des Spielers. Dies ist die Erstinitialisierung
        /// </summary>
        /// <param name="color">Die Farbe des Spielers, um zu entscheiden wo die Erstkoordinaten der Figuren ist</param>
        private void ConstructFigures(string color)
        {
            for(int i = 1; i <= 4; i++)
            {
                // Füge dem Dictionary jeweils eine ID und eine Figur hinzu.
                figures.Add(i, new Figure(color));
                // Überprüfe ob dies die Erstinitialisierung ist.
                if(!IsInit)
                {
                    // Erhalte die jeweilige Figure für die Zählvariable i, also Figure 1 für i=1 usw.
                    var figure = ReturnFigure(i);
                    switch(i)
                    {
                        // Weise den jeweiligen Figuren ihre Koordinaten zu.
                        // TODO: Koordinaten logischerweise von Farbe abhängig machen.
                        case 1:
                            figure.FigureCoordinate = new Coordinate2D(0, 0);
                            break;
                        case 2:
                            figure.FigureCoordinate = new Coordinate2D(0, 1);
                            break;
                        case 3:
                            figure.FigureCoordinate = new Coordinate2D(1, 0);
                            break;
                        case 4:
                            figure.FigureCoordinate = new Coordinate2D(1, 1);
                            break;
                    }
                }
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
