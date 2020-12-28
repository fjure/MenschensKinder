using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace MenschensKinder
{
    class GameField
    {
        public Coordinate2D Coordinates { get; }
        private bool House
        {
            get => IsHouse(this.Coordinates);
        }


        public GameField(int x, int y)
        {
            Coordinates = new Coordinate2D(x, y);
        }

        private bool IsHouse(Coordinate2D coordinates)
        {
            int x = coordinates.X;
            int y = coordinates.Y;
            if ((x == 0 || x == 1 || x == 9 || x == 10) && (y == 0 || y == 1 || y == 9 || y == 10))
                return true;
            else
                return false;
            
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", Coordinates.ToString(), (House) ? "T" : "F");
        }





    }
}
