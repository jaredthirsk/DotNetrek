using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    [Flags]
    public enum PlayerFlags : uint
    {
        PFSHIELD = 0x0001,    // displayed on tactical
        PFREPAIR = 0x0002,    // displayed in ReportSprite
        PFBOMB = 0x0004,    // displayed in ReportSprite
        PFORBIT = 0x0008,    // displayed in ReportSprite
        PFCLOAK = 0x0010,    // displayed in ReportSprite
        PFWEP = 0x0020,    // displayed in ReportSprite
        PFENG = 0x0040,    // displayed in ReportSprite
        PFROBOT = 0x0080,    // displayed in ReportSprite
        PFBEAMUP = 0x0100,    // displayed in ReportSprite
        PFBEAMDOWN = 0x0200,    // displayed in ReportSprite
        PFSELFDEST = 0x0400,    // displayed in ReportSprite
        PFGREEN = 0x0800,    // displayed as background colour
        PFYELLOW = 0x1000,    // displayed as background colour
        PFRED = 0x2000,    // displayed as background colour
        PFPLOCK = 0x4000,    // displayed in ReportSprite
        PFPLLOCK = 0x8000,    // displayed in ReportSprite
        PFCOPILOT = 0x10000,    // not to be displayed
        PFWAR = 0x20000,    // displayed in ReportSprite
        PFPRACTR = 0x40000,    // displayed in ReportSprite
        PFDOCK = 0x80000,    // displayed in ReportSprite
        PFREFIT = 0x100000,    // not to be displayed
        PFREFITTING = 0x200000,    // displayed in ReportSprite
        PFTRACT = 0x400000,    // displayed in ReportSprite
        PFPRESS = 0x800000,    // displayed in ReportSprite
        PFDOCKOK = 0x1000000,    // displayed in ReportSprite
        PFSEEN = 0x2000000,    // displayed in ReportSprite
        PFOBSERV = 0x8000000,    // not to be displayed
        PFTWARP = 0x40000000,    // displayed in ReportSprite
        PFBPROBOT = 0x80000000,    // displayed in ReportSprite
    }

}
