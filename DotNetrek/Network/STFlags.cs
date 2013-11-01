using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    [Flags]
    public enum STFlags : int
    {
        ST_MAPMODE = 0x001,
        ST_NAMEMODE = 0x002,
        ST_SHOWSHIELDS = 0x004,
        ST_KEEPPEACE = 0x008,
        ST_SHOWLOCAL = 0x010,      // two bits
        ST_SHOWLOCAL2 = 0x020,
        ST_SHOWGLOBAL = 0x040,  // two bits
        ST_SHOWGLOBAL2 = 0x080,
        ST_CYBORG = 0x100,
        ST_INITIAL = ST_MAPMODE + ST_NAMEMODE + ST_SHOWSHIELDS + ST_KEEPPEACE + ST_SHOWLOCAL * 2 + ST_SHOWGLOBAL * 2,
    }
}
