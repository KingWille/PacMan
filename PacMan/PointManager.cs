using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    internal class PointManager
    {
        Tiles[,] TileArray;
        public PointManager(Tiles[,] tilesArray ) 
        {
            TileArray = new Tiles[TileArray.GetLength(0), TileArray.GetLength(1)];
        }
    }
}
