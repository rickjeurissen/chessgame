using chessgame.engine;
using chessgame.wpf.Model;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessgame.wpf.ViewModel
{
    public class BoardViewModel : Screen
    {
        public ObservableCollection<ChessPiece> Pieces { get; set; } = new ObservableCollection<ChessPiece>();
        
        private Board board;

        public BoardViewModel()
        {
            SetupNewGame();

            board.MoveUnit(new string[] { "B8", "C6" });
            RedrawGameState();
        }

        private void SetupNewGame()
        {
            board = new Board();
            board.InitializeGame();
        }

        private void RedrawGameState()
        {
            Pieces.Clear(); // Cheap way of refreshing the board. Better way would be to check what pieces changed position, and update.

            for (int row = 0; row < board.TileMatrix.GetLength(0); row++)
            {
                for (int column = 0; column < board.TileMatrix.GetLength(1); column++)
                {
                    Unit currentUnit = board.TileMatrix[row, column].Unit;

                    switch (currentUnit.Type)
                    {
                        case UnitType.Horse:
                            Pieces.Add(new ChessPiece() { Row = row, Column = column, Type = ChessPieceTypes.Knight, IsBlack = Char.IsLower(currentUnit.ShortName.ToCharArray()[0]) });
                            break;
                        case UnitType.King:
                            Pieces.Add(new ChessPiece() { Row = row, Column = column, Type = ChessPieceTypes.King, IsBlack = Char.IsLower(currentUnit.ShortName.ToCharArray()[0]) });
                            break;
                        case UnitType.Queen:
                            Pieces.Add(new ChessPiece() { Row = row, Column = column, Type = ChessPieceTypes.Queen, IsBlack = Char.IsLower(currentUnit.ShortName.ToCharArray()[0]) });
                            break;
                        case UnitType.Rook:
                            Pieces.Add(new ChessPiece() { Row = row, Column = column, Type = ChessPieceTypes.Tower, IsBlack = Char.IsLower(currentUnit.ShortName.ToCharArray()[0]) });
                            break;
                        case UnitType.Bishop:
                            Pieces.Add(new ChessPiece() { Row = row, Column = column, Type = ChessPieceTypes.Bishop, IsBlack = Char.IsLower(currentUnit.ShortName.ToCharArray()[0]) });
                            break;
                        case UnitType.Pawn:
                            Pieces.Add(new ChessPiece() { Row = row, Column = column, Type = ChessPieceTypes.Pawn, IsBlack = Char.IsLower(currentUnit.ShortName.ToCharArray()[0]) });
                            break;
                        case UnitType.Whitespace:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
