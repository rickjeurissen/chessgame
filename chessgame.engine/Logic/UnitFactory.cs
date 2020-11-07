using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessgame.engine.Logic
{
    public static class UnitFactory
    {
        public static Unit CreatePawnObject(char v)
        {
            return new Unit()
            {
                Name = "Pawn",
                Type = UnitType.Pawn,
                ShortName = v.ToString()
            };
        }

        public static Unit CreateRookObject(char v)
        {
            return new Unit()
            {
                Name = "Rook",
                Type = UnitType.Rook,
                ShortName = v.ToString()
            };
        }

        public static Unit CreateBishopObject(char v)
        {
            return new Unit()
            {
                Name = "Bishop",
                Type = UnitType.Bishop,
                ShortName = v.ToString()
            };
        }

        public static Unit CreateHorseObject(char v)
        {
            return new Unit()
            {
                Name = "Horse",
                Type = UnitType.Horse,
                ShortName = v.ToString()
            };
        }

        public static Unit CreateKingObject(char v)
        {
            return new Unit()
            {
                Name = "King",
                Type = UnitType.King,
                ShortName = v.ToString()
            };
        }
        public static Unit CreateQueenObject(char v)
        {
            return new Unit()
            {
                Name = "Queen",
                Type = UnitType.Queen,
                ShortName = v.ToString()
            };
        }

        public static Unit CreateWhitespaceObject(char v)
        {
            return new Unit()
            {
                Name = "Air",
                Type = UnitType.Whitespace,
                ShortName = v.ToString()
            };
        }

        public static Unit CreateObjectFromChar(char v)
        {
            if (v.Equals('R') || v.Equals('r'))
            {
                return CreateRookObject(v);
            }
            else if (v.Equals('H') || v.Equals('h'))
            {
                return CreateHorseObject(v);
            }
            else if (v.Equals('B') || v.Equals('b'))
            {
                return CreateBishopObject(v);
            }
            else if (v.Equals('Q') || v.Equals('q'))
            {
                return CreateQueenObject(v);
            }
            else if (v.Equals('K') || v.Equals('k'))
            {
                return CreateKingObject(v);
            }
            else if (v.Equals('P') || v.Equals('p'))
            {
                return CreatePawnObject(v);
            } else if (v.Equals('.'))
            {
                return CreateWhitespaceObject(v);
            } 
            else
            {
                return null;
            }

        }
    }
}
