using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessgame.engine
{
    /*
     * This class represents a tile holding the following properties:
     * --- Color (white/black)
     * --- Unit object currently holding the tile
     */
    public class Tile
    {
        public Unit Unit { get; set; }
    }

}
