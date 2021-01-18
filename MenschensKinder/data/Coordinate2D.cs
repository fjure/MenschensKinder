using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschensKinder
{

    /// <summary>
    /// Util-Klasse um die Position des GameField besser verwalten zu können.
    /// </summary>
    public class Coordinate2D
    {
        public int X { get; }
        public int Y { get; }

        /// <summary>
        /// Konstruktor für die Coordinate2D Klasse. Erzeugt aus zwei Integer ein Objekt der Klasse Coordinate2D
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-Position</param>
        public Coordinate2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Override der ToString Methode um Debug-Informationen besser anzeigen zu können.
        /// </summary>
        /// <returns>Die 2D Koordinate als String.</returns>
        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}", X.ToString(), Y.ToString());
        }

        public override bool Equals(object obj)
        {
            Coordinate2D coordinate = (Coordinate2D)obj;
            if (coordinate != null)
            {
                if (this.X != coordinate.X || this.Y != coordinate.Y)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
