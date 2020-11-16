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
                return false;
            }
        }

        private bool IsMoveAllowed(int[] fromTile, int[] toTile)
        {
            // Calculate the move string from fromTile and toTile
            string moveString = CalculateMoveString(fromTile, toTile);

            // Get the fromTile
            //Tile fromTileObject = TileMatrix[fromTile[0], fromTile[1]];

            // Check if this move is within the Unit's move set
            bool isValidMove = IsValidMove(fromTile, moveString);

            if (isValidMove)
            {
                
                return true;
            } 
            else
            {
                return false;
            }
        }

        private bool IsValidMove(int[] fromTile, string moveString)
        {
            Tile fromTileObject = TileMatrix[fromTile[0], fromTile[1]];

            switch (fromTileObject.Unit.Type)
            {
                case UnitType.Horse:
                    return IsMoveInRules(UnitMoveRules.GetHorseRules(), moveString) && IsMoveNotColliding(fromTile, moveString);
                case UnitType.King:
                    return IsMoveInStraightLineAndInRules(UnitMoveRules.GetKingRules(), moveString) && IsMoveNotColliding(fromTile, moveString);
                case UnitType.Queen:
                    return IsMoveInStraightLineAndInRules(UnitMoveRules.GetQueenRules(), moveString) && IsMoveNotColliding(fromTile, moveString);
                case UnitType.Rook:
                    return IsMoveInStraightLineAndInRules(UnitMoveRules.GetRookRules(), moveString) && IsMoveNotColliding(fromTile, moveString);
                case UnitType.Bishop:
                    return IsMoveInStraightLineAndInRules(UnitMoveRules.GetBishopRules(), moveString) && IsMoveNotColliding(fromTile, moveString);
                case UnitType.Pawn:
                    return IsMoveInStraightLineAndInRules(UnitMoveRules.GetPawnRules(), moveString) && IsMoveNotColliding(fromTile, moveString);
                case UnitType.Whitespace:
                    return false;
                default:
                    return false;
            }
        }

        /*
         * Check if the move is within its ruleset. This method is probably only for the horse, because it does not have to
         * travel in a straight line.
         */
        private bool IsMoveInRules(List<string> rules, string moveString)
        {
            return rules.Contains(moveString);
        }

        private bool IsMoveNotColliding(int[] fromTile, string moveString)
        {
            bool isHorse = TileMatrix[fromTile[0], fromTile[1]].Unit.Type == UnitType.Horse;

            int[] fromTileCopy = (int[])fromTile.Clone();

            // If not a horse, check for every step if it collides with another unit other that whitespace
            if (!isHorse)
            {
                foreach (string direction in moveString.TrimEnd(',').Split(','))
                {
                    Tile nextTile = GetRelativeTileUsingDirection(fromTileCopy, direction);

                    if (nextTile.Unit.Type == UnitType.Whitespace)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Not a valid move. Move collides with another unit.");
                        return false;
                    }
                }

                return true;
            } 
            else
            {
                // For the horse, move trough the directions but check only at the end if the move is valid
                // Horses can jump over other units
                string[] moveStringSplitted = moveString.TrimEnd(',').Split(',');

                for (int x = 0; x < moveStringSplitted.Length; x++)
                {
                    Tile nextTile = GetRelativeTileUsingDirection(fromTileCopy, moveStringSplitted[x]);

                    if (nextTile.Unit.Type == UnitType.Whitespace && x == moveStringSplitted.Length - 1)
                    {
                        return true;
                    }
                }

                Console.WriteLine("Not a valid move. Move collides with another unit.");
                return false;

            }
        }

        /*
         * This method is responsible for finding the tile using the y and x coordinates and the direction relative to it
         */
        private Tile GetRelativeTileUsingDirection(int[] fromTile, string direction)
        {
            // Add the comma to the direction end to match constant
            direction += ",";

            switch (direction)
            {
                case Constants.DIRECTION_UP:
                    fromTile[0]--; // direction up equals matrix y minus one
                    // Strange observation: fromTile[0]-- inline in the return line is NOT the same as doing -- first, then the return??
                    return TileMatrix[fromTile[0], fromTile[1]];
                case Constants.DIRECTION_UP_RIGHT:
                    fromTile[0]--;
                    fromTile[1]++;
                    return TileMatrix[fromTile[0], fromTile[1]];
                case Constants.DIRECTION_RIGHT:
                    fromTile[1]++;
                    return TileMatrix[fromTile[0], fromTile[1]];
                case Constants.DIRECTION_DOWN_RIGHT:
                    fromTile[0]++;
                    fromTile[1]++;
                    return TileMatrix[fromTile[0], fromTile[1]];
                case Constants.DIRECTION_DOWN:
                    fromTile[0]++;
                    return TileMatrix[fromTile[0], fromTile[1]];
                case Constants.DIRECTION_DOWN_LEFT:
                    fromTile[0]++;
                    fromTile[1]--;
                    return TileMatrix[fromTile[0], fromTile[1]];
                case Constants.DIRECTION_LEFT:
                    fromTile[1]--;
                    return TileMatrix[fromTile[0], fromTile[1]];
                case Constants.DIRECTION_UP_LEFT:
                    fromTile[0]--;
                    fromTile[1]--;
                    return TileMatrix[fromTile[0], fromTile[1]];
                default:
                    return TileMatrix[fromTile[0], fromTile[1]];
            }
        }

        private bool IsMoveInStraightLineAndInRules(List<string> rules, string moveString)
        {
            if (rules.Contains(moveString.Substring(0,2) + "*"))
            {
                string[] moveDirections = moveString.Split(',');

                for (int x = 1; x < moveDirections.Length; x++)
                {
                    if (!moveDirections[x].Equals(moveDirections[x - 1]) && !moveDirections[x].Equals(""))
                    {
                        Console.WriteLine("Move is not a straight line or in rules.");

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
                Console.WriteLine("Move is not a straight line or in rules.");
                return false;
            }
        }

        private string CalculateMoveString(int[] fromTile, int[] toTile)
        {
            int[] localFromTile = (int[])fromTile.Clone();
            int[] localToTile = (int[])toTile.Clone();

            string moveString = String.Empty;

            while(true)
            {
                // If Y pos of fromTile is bigger than Y pos of to Tile AND x pos fromtile equals y pos toTile; direction is UP
                if (localFromTile[0] > localToTile[0] && localFromTile[1] == localToTile[1])
                {
                    for (int y1 = localFromTile[0]; y1 > localToTile[0]; y1--)
                    {
                        moveString += Constants.DIRECTION_UP;
                        localFromTile[0]--;
                    }
                }
                // If y pos of fromTIle is bigger then ypos of toTile AND X pos fromTile  is bigger than X pos toTile, direction is TOP LEFT
                else if (localFromTile[0] > localToTile[0] && localFromTile[1] > localToTile[1])
                {
                    for (int x1 = localFromTile[1]; x1 > localToTile[1]; x1--)
                    {
                        moveString += Constants.DIRECTION_UP_LEFT;
                        localFromTile[0]--;
                        localFromTile[1]--;
                    }
                }
                // If y pos of fromTIle is bigger then ypos of toTile AND X pos fromTile  is less than X pos toTile, direction is TOP RIGHT
                else if (localFromTile[0] > localToTile[0] && localFromTile[1] < localToTile[1])
                {
                    for (int x1 = localFromTile[1]; x1 < localToTile[1]; x1++)
                    {
                        moveString += Constants.DIRECTION_UP_RIGHT;
                        localFromTile[0]--;
                        localFromTile[1]++;
                    }
                }

                // If Y pos of fromTile is less than Y pos of toTile; direction is DOWN
                else if (localFromTile[0] < localToTile[0] && localFromTile[1] == localToTile[1])
                {
                    for (int y1 = localFromTile[0]; y1 < localToTile[0]; y1++)
                    {
                        moveString += Constants.DIRECTION_DOWN;
                        localFromTile[0]++;
                    }
                }
                // If Y pos of fromTile is less than Y pos of toTile AND x pos fromTIle is bigger than X pos toTile, direction is BOTTOM LEFT
                else if (localFromTile[0] < localToTile[0] && localFromTile[1] > localToTile[1])
                {
                    for (int x1 = localFromTile[1]; x1 > localToTile[1]; x1--)
                    {
                        moveString += Constants.DIRECTION_DOWN_LEFT;
                        localFromTile[0]++;
                        localFromTile[1]--;
                    }
                }
                // If y pos of fromTIle is bigger then ypos of toTile AND X pos fromTile  is less than X pos toTile, direction is BOTTOM RIGHT
                else if (localFromTile[0] < localToTile[0] && localFromTile[1] < localToTile[1])
                {
                    for (int x1 = localFromTile[1]; x1 < localToTile[1]; x1++)
                    {
                        moveString += Constants.DIRECTION_DOWN_RIGHT;
                        localFromTile[0]++;
                        localFromTile[1]++;
                    }
                }

                // If X pos of fromTile is bigger than X pos of toTile; direction is RIGHT
                else if (localFromTile[1] < localToTile[1] && localFromTile[0] == localToTile[0])
                {
                    for (int x1 = localFromTile[1]; x1 < localToTile[1]; x1++)
                    {
                        moveString += Constants.DIRECTION_RIGHT;
                        localFromTile[1]++;
                    }
                }

                // If x pos of fromTile is less than X pos of toTile; direction is LEFT
                else if (localFromTile[1] > localToTile[1] && localFromTile[0] == localToTile[0])
                {
                    for (int x1 = localFromTile[1]; x1 > localToTile[1]; x1--)
                    {
                        moveString += Constants.DIRECTION_LEFT;
                        localFromTile[1]--;
                    }
                } 
                else
                {
                    break;
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
