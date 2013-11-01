using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    [Flags]
    public enum PlayerStatus : sbyte
    {
        Free = 0,
        Outfit = 1,
        Alive = 2,
        Explode = 3,
        Dead = 4,
        Observe = 5,
    }
}
