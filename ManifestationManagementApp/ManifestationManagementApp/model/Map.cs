using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestationManagementApp.model
{
    public class Map
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public int Width { get; set; }

        public int Heigth { get; set; }

        public Coordinates ParentMapOffset { get; set; }

        public List<Map> ChildMaps { get; set; }
    }
}
