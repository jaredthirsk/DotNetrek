using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public static class GameType
    {
        public static Dictionary<string, string> LongTypeNames;

        public static bool IsSupported(string longName)
        {
            switch (longName)
            {
                // TODO: Try loading all the assets
                case "Bronco":
                    return true;
                default:
                    return false;
            }
        }

        public static string GetLongName(string shortName)
        {
            switch (shortName)
            {
                case "B":
                    return "Bronco";
                case "H":
                    return "Hockey";
                case "P":
                    return "Paradise";
                case "S":
                    return "Sturgeon";
                default:
                    return null;
            }
        }
    }
}
