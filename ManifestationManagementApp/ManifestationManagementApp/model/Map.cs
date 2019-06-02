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

        /**
            Adds map coordinates to a manifestation based on the position on current map when dropped upon.
            Manifestation pointer must be deisplayed on all maps showing the area dropped upon.
            The map coordinates are added for the main map first, then for all submaps containing
            the drop area.

            Parameters:
            Manifestation manif - dropped manifestation for which the map coordinates are
            calculated,
            int x - x position of drop action on current map
            int y - y position of drop action on current map
        */
        public void PlaceManifAtPos(Manifestation manif, int x, int y)
        {
            Map parent = ParentMapOffset.ParentMap;
            if (parent != null)
            {
                //pass manifestation to according position on the main map
                parent.PlaceManifAtPos(manif, x + ParentMapOffset.X, y + ParentMapOffset.Y);
            }
            else
            {
                //place or move manif on main (current) map first
                PlaceOrMoveManifOnPos(manif, x, y);
                
                //place or move manif on submaps containing the new position
                foreach(Map submap in ChildMaps)
                {
                    //check if submap contains new position
                    int xOffset = submap.ParentMapOffset.X;
                    int yOffset = submap.ParentMapOffset.Y;
                    if (x >= xOffset && x <= xOffset + submap.Width 
                        && y >= yOffset && y <= yOffset + submap.Heigth)
                    {
                        //place or move manif to according position on submap
                        submap.PlaceOrMoveManifOnPos(manif, x - xOffset, y - yOffset);
                    }
                    else
                    {
                        //if submap does not contain new position remove the submap coordinates
                        //from manifestation if present
                        manif.RemoveCoordinatesForMap(submap.Id);
                    }
                }
            }
        }

        private void PlaceOrMoveManifOnPos(Manifestation manif, int x, int y)
        {
            //current map coordinates to be added or updated
            Coordinates mapCoords = null;
            //check if manif is already placed on current map
            foreach (Coordinates c in manif.MapCoordinates)
            {
                if (c.ParentMap.Id == Id)
                {
                    mapCoords = c;
                    break;
                }
            }
            //if manif is not on map add new coordinates, else update old coordinates
            if (mapCoords == null)
            {
                mapCoords = new Coordinates();
                mapCoords.ParentMap = this;
                manif.MapCoordinates.Add(mapCoords);
            }
            mapCoords.X = x;
            mapCoords.Y = y;
        }
    }
}
