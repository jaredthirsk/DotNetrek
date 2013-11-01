using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public enum PhaserStatus : sbyte
    {
        Free = 0x00,
        Hit = 0x01, // ship
        Miss = 0x02, // whiff
        Hit2 = 0x04, // plasma
    }
}
