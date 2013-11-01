using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public enum ShipType : sbyte
    {
        Scout = 0,
        Destroyer = 1,
        Cruiser = 2,
        Battleship = 3,
        Assault = 4,
        Starbase = 5,
        Galaxy = 6,
        ATT = 7,
        NUM_TYPES = 8,
    }

    public static class ShipTypeExtensions
    {
        public static string GetAbbreviation(this ShipType shipType)
        {
            switch (shipType)
            {
                case ShipType.Scout:
                    return "SC";
                case ShipType.Destroyer:
                    return "DD";
                case ShipType.Cruiser:
                    return "CA";
                case ShipType.Battleship:
                    return "BB";
                case ShipType.Assault:
                    return "AS";
                case ShipType.Starbase:
                    return "SB";
                case ShipType.Galaxy:
                    return "GA";
                case ShipType.ATT:
                    return "AT";
                default:
                    return null;
            }
        }

        public static ShipType ToShipTypeFromAbbreviation(this string shipTypeAbbreviation)
        {
            switch (shipTypeAbbreviation)
            {
                case "SC":
                    return ShipType.Scout;
                case "DD":
                    return ShipType.Destroyer;
                case "CA":
                    return ShipType.Cruiser;
                case "BB":
                    return ShipType.Battleship;
                case "AS":
                    return ShipType.Assault;
                case "SB":
                    return ShipType.Starbase;
                case "GA":
                    return ShipType.Galaxy;
                case "AT":
                    return ShipType.ATT;
                default:
                    throw new ArgumentException("Unknown shipTypeAbbreviation: " + shipTypeAbbreviation);
            }
        }
    }
}
