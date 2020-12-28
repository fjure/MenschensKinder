using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschensKinder
{
    class Coordinate2D
    {
        public int X { get; }
        public int Y { get; }


        public Coordinate2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}", X.ToString(), Y.ToString());
        }

    }
}
