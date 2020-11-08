using chessgame.engine.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace chessgame.engine
{
    /*
     * This class is responsible for holding state of a single game.
     * Must haves:
     * 
     * DONE Matrix of 8x8 for holding states of every tile. Tile state example:
     *  --- White tile, black horse. Or black tile, white king. Or black tile, black queen.
     * DONE Initialisation of position play-pieces at the start of a game (for player white)
         * --- R - rook
         * --- H - horse
         * --- B - bishop
         * --- Q - queen
         * --- K - king
         * --- P - pawn
     * DONE Be able to move a play-piece
     *  --- With commands: A1 (tower) A5 to move 4 tiles up (as white player)
     * PARTLY-DONE Calculate if the move-action is allowed
     *  --- e.g. a horse can go from A1 to B3. But A1 to A3 is not allowed because the
     *  --- play-piece on A1 is a horse and has specific move pattern
     *  --- And a Rook shouldnt be able to go trough another unit
     * - Check if a player is standing 'check'
     * - Check if a player got check-mated
     */
    public class Board
    {
        /*
         * TileMatrix contains a matrix of Tile objects that hold states for every tile
         */
        public Tile[,] TileMatrix { get; set; } = new Tile[Constants.BoardHeight, Constants.BoardWidth];

        /*
         * This initialization method is responsible for correctly setting up the game to start playing
         */
        public void InitializeGame()
        {
            // Fill the TileMatrix with new objects
            InitializeTileMatrix();

            // Set the unit positions of player 1 and 2
            SetPlayersUnitPositions("PPPPPPPPRHBQKBHR", "rhbqkbhrpppppppp");
        }

        private void InitializeTileMatrix()
        {
            for (int y = 0; y < Constants.BoardHeight; y++)
            {
                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    TileMatrix[y, x] = new Tile();
                }
            }
        }

        private void SetPlayersUnitPositions(string tileStringP1, string tileStringP2)
        {
            // Convert to char array
            char[] p1CharArray = tileStringP1.ToCharArray();
            char[] p2CharArray = tileStringP2.ToCharArray();

            // For each char, place it in the tile matrix.
            for (int c = 0; c < p1CharArray.Length; c++)
            {
                if (c < Constants.BoardWidth)
                {
                    TileMatrix[6, c].Unit = UnitFactory.CreateObjectFromChar(p1CharArray[c]);
                } else if (c >= Constants.BoardWidth)
                {
                    TileMatrix[7, c - Constants.BoardWidth].Unit = UnitFactory.CreateObjectFromChar(p1CharArray[c]);
                }
            }

            for (int c = 0; c < p2CharArray.Length; c++)
            {
                if (c < Constants.BoardWidth)
                {
                    TileMatrix[0, c].Unit = UnitFactory.CreateObjectFromChar(p2CharArray[c]);
                }
                else if (c >= Constants.BoardWidth)
                {
                    TileMatrix[1, c - Constants.BoardWidth].Unit = UnitFactory.CreateObjectFromChar(p2CharArray[c]);
                }
            }

            for (int y = 2; y < 6; y++)
            {
                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    TileMatrix[y, x].Unit = UnitFactory.CreateObjectFromChar('.');
                }
            }

        }


        /*
         * Expected tile pos format: B7, C2, A1 etc
         */

        public bool MoveUnit(string[] fromToTilePos)
        {
            int[] fromTile = ConvertStringToPos(fromToTilePos[0]);
            int[] toTile = ConvertStringToPos(fromToTilePos[1]);
            
            if (IsMoveAllowed(fromTile, toTile))
            {
                // Get the tile object from the TileMatrix using the fromTile array
                Tile fromTileObject = TileMatrix[fromTile[0], fromTile[1]];

                // Copy the fromTile unit to the toTile unit in the TileMatrix
                TileMatrix[toTile[0], toTile[1]].Unit = TileMatrix[fromTile[0], fromTile[1]].Unit;

                Console.WriteLine(fromTileObject.Unit.Type.ToString() + " moved");

                // Make the fromTile unit in the TileMatrix whitespace
                TileMatrix[fromTile[0], fromTile[1]].Unit = UnitFactory.CreateWhitespaceObject('.');

                return true;
            } else
            {
                Console.WriteLine("Move was not allowed");
                return false;
            }
        }

        private bool IsMoveAllowed(int[] fromTile, int[] toTile)
        {
            // Calculate the move string from fromTile and toTile
            string moveString = CalculateMoveString(fromTile, toTile);

            //Console.WriteLine(moveString);

            // Get the fromTile
            Tile tileToCheck = TileMatrix[fromTile[0], fromTile[1]];

            // Check if this move is within the Unit's move set
            // TODO: How to make a rook go only direction for unlimited length? 1,3,3 is not valid because it can only be 1,1,1 or 3,3,3 etc
            switch (tileToCheck.Unit.Type)
            {
                case UnitType.Horse:
                    return UnitMoveRules.GetHorseRules().Contains(moveString);
                case UnitType.King:
                    return IsStraightLineAndInRules(UnitMoveRules.GetKingRules(), moveString);
                case UnitType.Queen:
                    return IsStraightLineAndInRules(UnitMoveRules.GetQueenRules(), moveString);
                case UnitType.Rook:
                    //if (UnitMoveRules.GetRookRules().Contains(moveString.Substring(0, 2) + "*"))
                    //{
                    //    string[] moveDirections = moveString.Split(',');

                    //    for (int x = 1; x < moveDirections.Length; x++)
                    //    {
                    //        if (!moveDirections[x].Equals(moveDirections[x - 1]) && !moveDirections[x].Equals(""))
                    //        {
                    //            return false;
                    //        }
                    //    }

                    //    return true;
                    //}
                    //return UnitMoveRules.GetRookRules().Contains(moveString);

                    return IsStraightLineAndInRules(UnitMoveRules.GetRookRules(), moveString);
                case UnitType.Bishop:

                    return IsStraightLineAndInRules(UnitMoveRules.GetBishopRules(), moveString);
                case UnitType.Pawn:
                    return IsStraightLineAndInRules(UnitMoveRules.GetPawnRules(), moveString);
                case UnitType.Whitespace:
                    return false;
                default:
                    return false;
            }
        }

        private bool IsStraightLineAndInRules(List<string> rules, string moveString)
        {
            if (rules.Contains(moveString.Substring(0,2) + "*"))
            {
                string[] moveDirections = moveString.Split(',');

                for (int x = 1; x < moveDirections.Length; x++)
                {
                    if (!moveDirections[x].Equals(moveDirections[x - 1]) && !moveDirections[x].Equals(""))
                    {
                        return false;
                    }
                }

                return true;
            }  
            else if (rules.Contains(moveString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string CalculateMoveString(int[] fromTile, int[] toTile)
        {
            // Horse is a problem now, because it goes 1,1,3. 3 is not equal to 1.
            string moveString = String.Empty;
            // If Y pos of fromTile is bigger than Y pos of to Tile AND x pos fromtile equals y pos toTile; direction is UP
            if (fromTile[0] > toTile[0] && fromTile[1] == toTile[1])
            {
                for (int y1 = fromTile[0]; y1 > toTile[0]; y1--)
                {
                    moveString += "1,";
                }
            }
            // If y pos of fromTIle is bigger then ypos of toTile AND X pos fromTile  is bigger than X pos toTile, direction is top left
            if (fromTile[0] > toTile[0] && fromTile[1] > toTile[1])
            {
                for (int x1 = fromTile[1]; x1 > toTile[1]; x1--)
                {
                    moveString += "8,";
                }
            }
            // If y pos of fromTIle is bigger then ypos of toTile AND X pos fromTile  is less than X pos toTile, direction is top right
            if (fromTile[0] > toTile[0] && fromTile[1] < toTile[1])
            {
                for (int x1 = fromTile[1]; x1 < toTile[1]; x1++)
                {
                    moveString += "2,";
                }
            }

            // If Y pos of fromTile is less than Y pos of toTile; direction is DOWN
            if (fromTile[0] < toTile[0] && fromTile[1] == toTile[1])
            {
                for (int y1 = fromTile[0]; y1 < toTile[0]; y1++)
                {
                    moveString += "5,";
                }
            }
            // If Y pos of fromTile is less than Y pos of toTile AND x pos fromTIle is bigger than X pos toTile, direction is bottom left
            if (fromTile[0] < toTile[0] && fromTile[1] > toTile[1])
            {
                for (int x1 = fromTile[1]; x1 > toTile[1]; x1--)
                {
                    moveString += "6,";
                }
            }
            // If y pos of fromTIle is bigger then ypos of toTile AND X pos fromTile  is less than X pos toTile, direction is bottom right
            if (fromTile[0] < toTile[0] && fromTile[1] < toTile[1])
            {
                for (int x1 = fromTile[1]; x1 < toTile[1]; x1++)
                {
                    moveString += "4,";
                }
            }

            // If X pos of fromTile is bigger than X pos of toTile; direction is RIGHT
            if (fromTile[1] < toTile[1] && fromTile[0] == toTile[0])
            {
                for (int x1 = fromTile[1]; x1 < toTile[1]; x1++)
                {
                    moveString += "3,";
                }
            }

            // If x pos of fromTile is less than X pos of toTile; direction is LEFT
            if (fromTile[1] > toTile[1] && fromTile[0] == toTile[0])
            {
                for (int x1 = fromTile[1]; x1 > toTile[1]; x1--)
                {
                    moveString += "7,";
                }
            }
            

            return moveString;
        }

        private int[] ConvertStringToPos(string pos)
        {
            int[] position = new int[2];
            char[] chars = pos.ToUpper().ToCharArray();
            if (chars[0].Equals('A'))
            {
                position[1] = 0;
            }
            else if (chars[0].Equals('B'))
            {
                position[1] = 1;
            }
            else if (chars[0].Equals('C'))
            {
                position[1] = 2;
            }
            else if (chars[0].Equals('D'))
            {
                position[1] = 3;
            }
            else if (chars[0].Equals('E'))
            {
                position[1] = 4;
            }
            else if (chars[0].Equals('F'))
            {
                position[1] = 5;
            }
            else if (chars[0].Equals('G'))
            {
                position[1] = 6;
            }
            else if (chars[0].Equals('H'))
            {
                position[1] = 7;
            }

            int parsedPos = 0;
            if (int.TryParse(chars[1].ToString(), out parsedPos))
            {
                if (parsedPos > 0 && parsedPos < 9)
                {
                    position[0] = parsedPos - 1;
                } else
                {
                    return null;
                }
            } else
            {
                return null;
            }

            return position;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();


            for (int y = 0; y < Constants.BoardHeight; y++)
            {
                sb.Append("   " + (y+1) + "|   ");

                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    sb.Append(TileMatrix[y, x].Unit.ToString());
                    sb.Append("  ");
                }

                sb.Append('\n');
            }
            sb.Append("        ______________________");
            sb.Append("\n        A  B  C  D  E  F  G  H");

            return sb.ToString();
        }
    }
}
