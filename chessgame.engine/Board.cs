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
     * - Matrix of 8x8 for holding states of every tile. Tile state example:
     * --- White tile, black horse. Or black tile, white king. Or black tile, black queen.
     * - Initialisation of position play-pieces at the start of a game (for player white)
     * --- R - rook
     * --- H - horse
     * --- B - bishop
     * --- Q - queen
     * --- K - king
     * --- P - pawn
     * - Be able to move a play-piece
     * --- With commands: A1 (tower) A5 to move 4 tiles up (as white player)
     * - Calculate if the move-action is allowed
     * --- e.g. a horse can go from A1 to B3. But A1 to A3 is not allowed because the
     * --- play-piece on A1 is a horse and has specific move pattern
     * - Check if a player is standing 'check'
     * - Check if a player got check-mated
     */
    public class Board
    {
        /*
         * TileMatrix contains a matrix of Tile objects that hold states for every tile
         */
        public Tile[,] TileMatrix { get; set; } = new Tile[Constants.BoardHeight, Constants.BoardWidth];

        public void InitializeGame()
        {
            InitializeTileMatrix();

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
        public bool MoveUnit(string fromTilePos, string toTilePos)
        {
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < Constants.BoardHeight; y++)
            {
                sb.Append("            ");

                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    sb.Append(TileMatrix[y, x].Unit.ToString());
                    sb.Append("  ");
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}
