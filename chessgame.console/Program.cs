using chessgame.engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace chessgame.console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============ Welcome to chessgame! ============");
            Console.WriteLine("");
            Console.WriteLine("The units of Player 1 are uppercase letters.");
            Console.WriteLine("The units of Player 2 are lowecase letters.");
            Console.WriteLine("Each character represents the following unit:");
            Console.WriteLine("  R: Rook");
            Console.WriteLine("  H: Horse");
            Console.WriteLine("  B: Bishop");
            Console.WriteLine("  Q: Queen");
            Console.WriteLine("  K: King");
            Console.WriteLine("  P: Pawn");
            Console.WriteLine("  .: Nothing");
            Console.WriteLine("");
            Console.WriteLine("===============================================");
            Console.WriteLine("");

            Board board = new Board();
            board.InitializeGame();
            Console.WriteLine(board);
            Console.WriteLine("");

            while (true)
            {
                Console.Write("Move: ");
                board.MoveUnit(Console.ReadLine().Split());
                Console.WriteLine("");
                Console.WriteLine(board);
                Console.WriteLine("");
            }
        }
    }
}
