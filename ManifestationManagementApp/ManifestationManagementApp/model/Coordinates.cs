using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestationManagementApp.model
{
    public class Coordinates
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Map ParentMap { get; set; }

        public Coordinates()
        {
            X = 0;
            Y = 0;
        }

        public Coordinates(int x_coord, int y_coord, Map parent)
        {
            X = x_coord;
            Y = y_coord;
            ParentMap = parent;
        }
    }
}
