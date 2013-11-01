using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    [Flags]
    public enum PlanetFlags : short
    {
        Unspecified = 0x0,
        Repair = 0x010,
        Fuel = 0x020,
        Agri = 0x040,
        Redraw = 0x080,
        Home = 0x100,
        Coup = 0x200,
        Cheap = 0x400,
        Core = 0x800,
        Clear = 0x1000,
    }
}
