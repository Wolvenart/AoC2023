using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCHelper
{
    public static class DirectionHelper
    {
        public static readonly List<System.Drawing.Point> Modifiers_Diagonal = new List<System.Drawing.Point>()
        {
            new System.Drawing.Point(0, 1), //N
            new System.Drawing.Point(1, 1), //NE
            new System.Drawing.Point(1, 0), //E
            new System.Drawing.Point(1, -1), //SE
            new System.Drawing.Point(0, -1), //S
            new System.Drawing.Point(-1, -1), //SW
            new System.Drawing.Point(-1, 0), //W
            new System.Drawing.Point(-1, 1), //NW
        };
        public static readonly List<System.Drawing.Point> Modifiers = new List<System.Drawing.Point>()
        {
            new System.Drawing.Point(0, 1), //N
            new System.Drawing.Point(1, 0), //E
            new System.Drawing.Point(0, -1), //S
            new System.Drawing.Point(-1, 0), //W
        };
    }
}
