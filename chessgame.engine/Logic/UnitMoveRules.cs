using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessgame.engine.Logic
{
    /*
     * This class is responsible for the move rules for every playable Unit
     * The move string gets calculated where UP and DOWN are done FIRST. So a horse does 1,3,3 and NOT 3,3,1.
     * If the rule is not correctly set, the horse cannot move
     */
    public static class UnitMoveRules
    {
        public static List<string> GetHorseRules()
        {
            List<string> horseRules = new List<string>();

            horseRules.Add("2,1,"); horseRules.Add("8,1,");
            horseRules.Add("2,2,5,"); horseRules.Add("4,4,1,");
            horseRules.Add("4,5,"); horseRules.Add("6,5,");
            horseRules.Add("6,6,1,"); horseRules.Add("8,8,5,");

            return horseRules;
        }

        public static List<string> GetKingRules()
        {
            List<string> kingRules = new List<string>();

            kingRules.Add("1,"); kingRules.Add("2,");
            kingRules.Add("3,"); kingRules.Add("4,");
            kingRules.Add("5,"); kingRules.Add("6,");
            kingRules.Add("7,"); kingRules.Add("8,");

            return kingRules;
        }

        public static List<string> GetQueenRules()
        {
            List<string> queenRules = new List<string>();

            queenRules.Add("1,*"); queenRules.Add("2,*");
            queenRules.Add("3,*"); queenRules.Add("4,*");
            queenRules.Add("5,*"); queenRules.Add("6,*");
            queenRules.Add("7,*"); queenRules.Add("8,*");

            return queenRules;
        }

        public static List<string> GetRookRules()
        {
            List<string> rookRules = new List<string>();

            rookRules.Add("1,*"); rookRules.Add("3,*");
            rookRules.Add("5,*"); rookRules.Add("7,*");

            return rookRules;
        }

        public static List<string> GetBishopRules()
        {
            List<string> bishopRules = new List<string>();

            bishopRules.Add("2,*"); bishopRules.Add("4,*");
            bishopRules.Add("6,*"); bishopRules.Add("8,*");

            return bishopRules;
        }

        public static List<string> GetPawnRules()
        {
            List<string> pawnRules = new List<string>();

            pawnRules.Add("1,");

            return pawnRules;
        }
    }
}
