using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek.Model
{
    public enum KillReason : sbyte // REVIEW confirm type 
    {
        KQUIT = 0x01,            /* Player quit */
        KTORP = 0x02,            /* killed by torp */
        KPHASER = 0x03,            /* killed by phaser */
        KPLANET = 0x04,            /* killed by planet */
        KSHIP = 0x05,            /* killed by other ship */
        KDAEMON = 0x06,            /* killed by dying daemon */
        KWINNER = 0x07,            /* killed by a winner */
        KGHOST = 0x08,            /* killed because a ghost */
        KGENOCIDE = 0x09,            /* killed by genocide */
        KPROVIDENCE = 0x0a,            /* killed by a hacker */
        KPLASMA = 0x0b,            /* killed by a plasma torpedo */
    }
}
