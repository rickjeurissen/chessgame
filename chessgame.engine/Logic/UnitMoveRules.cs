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

            horseRules.Add("1,1,3,"); horseRules.Add("1,1,7,");
            horseRules.Add("1,3,3,"); horseRules.Add("5,3,3,");
            horseRules.Add("5,5,7,"); horseRules.Add("5,5,3,");
            horseRules.Add("1,7,7,"); horseRules.Add("5,7,7,");

            return horseRules;
        }

        internal static object GetKingRules()
        {
            throw new NotImplementedException();
        }
    }
}
