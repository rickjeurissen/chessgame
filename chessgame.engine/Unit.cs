using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessgame.engine
{
    /*
     * This class is responsible for representing a play-piece on the board.
     * A play-piece has a name, possible moves.
     * The move property is defined by 'direction numbers' relative to its position
     * 1, 3, 5 and 7 are horizontal up, right, down and left SINGLE STEP
     * 2, 4, 6 and 8 are diagonal up-right, bottom-right, bottom-left, up left SINGLE STEP
     * e.g. a horse will have the move property of 1,1,3 AND 1,1,7 AND 3,3,1 3,3,5 ETC
     */
    public class Unit
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public UnitType Type { get; set; }

        public override string ToString()
        {
            return ShortName;
        }
    }

    public enum UnitType
    {
        Horse,
        King,
        Queen,
        Rook,
        Bishop,
        Pawn,
        Whitespace
    }
}
