using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    // Based on Vanilla server code

    [Flags]
    public enum MessageFlags : byte
    {
        MVALID = 0x01,
        MGOD = 0x10, // this is the biggest MFROM flag - can't overlap this one

        // order flags by importance (0x100 - 0x400) 
        // restructuring of message flags to squeeze them all into 1 byte - jmn 

        /* hopefully quasi-back-compatible:
           MVALID, MINDIV, MTEAM, MALL, MGOD use up 5 bits. this leaves us 3 bits.
           since the server only checks for those flags when deciding message
           related things and since each of the above cases only has 1 flag on at
           a time we can overlap the meanings of the flags */

        MINDIV = 0x02,
        // these go with MINDIV flag 
#if STDBG
            MDBG  = 0x20,
#endif
        MCONFIG = 0x40,
        MDIST = 0x60,
        DISTR = 0x40,

        MTEAM = 0x04,
        // these go with MTEAM flag 
        MTAKE = 0x20,
        MDEST = 0x40,
        MBOMB = 0x60,
        MCOUP1 = 0x80,
        MCOUP2 = 0xA0,
        MDISTR = 0xC0,     // flag distress messages

        MALL = 0x08,

        // these go with MALL flag 
        MGENO = 0x20,    // MGENO is not used in INL server but belongs here 
        MCONQ = 0x20,     // not enough bits to distinguish MCONQ from MGENO :( 
        MKILLA = 0x40,
        MKILLP = 0x60,
        MKILL = 0x80,
        MLEAVE = 0xA0,
        MJOIN = 0xC0,
        MGHOST = 0xE0,
        
        MWHOMSK = 0x1f,  // mask with this to find who msg to 
        MWHATMSK = 0xe0,   // mask with this to find what message about 

    }

}
