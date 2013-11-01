using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public enum PlasmaStatus : sbyte
    {
        Free = 0,
        Move = 1,
        Explode = 2,
        Det = 3,
    }

    public enum TorpStatus
    {
        Free = 0,
        Move = 1,
        Explode = 2,
        Detonated = 3,
        TOFF = 4,
        TSTRAIGHT = 5,
    }
}
