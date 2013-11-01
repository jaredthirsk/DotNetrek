using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LionFire.Netrek
{
    //
    // These are the client --> server packets
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class mesg_cpacket
    { /* CP_MESSAGE py-public struct "!bBBx80s" #1 */
        public sbyte type = (sbyte)ClientMessageType.CP_MESSAGE;
        public byte group;
        public byte indiv;	/* does this break anything? -da */
        public sbyte pad1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.MSG_LEN)]
        public string mesg;
        public string Message
        {
            get { return mesg; }
            set
            {
                if (value.Length > NetConstants.MSG_LEN) throw new ArgumentException("Message too long.  Max: " + NetConstants.MSG_LEN);
                mesg = value;
            }
        }
        //public unsafe fixed sbyte mesg[NetConstants.MSG_LEN];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct speed_cpacket
    { /* CP_SPEED py-public struct "!bbxx" #2 */
        public sbyte type;
        public sbyte speed;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct dir_cpacket
    { /* CP_DIRECTION py-public struct "!bBxx" #3 */
        public sbyte type;
        public byte dir;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct phaser_cpacket
    { /* CP_PHASER py-public struct "!bBxx" #4 */
        public sbyte type;
        public byte dir;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct plasma_cpacket
    { /* CP_PLASMA py-public struct "!bBxx" #5 */
        public sbyte type;
        public byte dir;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct torp_cpacket
    { /* CP_TORP py-public struct "!bBxx" #6 */
        public sbyte type;
        public byte dir;		/* direction to fire torp */
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct quit_cpacket
    { /* CP_QUIT py-public struct "!bxxx" #7 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct login_cpacket
    { /* CP_LOGIN py-public struct '!bbxx16s16s16s' #8 */
        public sbyte type;
        public sbyte query;
        public sbyte pad2;
        public sbyte pad3;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string password;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string login;

        //public unsafe fixed sbyte name[Constants.NAME_LEN];
        //public unsafe fixed sbyte password[Constants.NAME_LEN];
        //public unsafe fixed sbyte login[Constants.NAME_LEN];

    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct outfit_cpacket
    { /* CP_OUTFIT py-public struct "!bbbx" #9 */
        public sbyte type;
        public sbyte team;
        //public NetrekTeam Team { set { team = (sbyte)value; } }
        public sbyte ship;
        public sbyte pad1;

        public outfit_cpacket(NetrekTeam team, ShipType shipType)
        {
            TeamsNumeric teamNumeric = (TeamsNumeric)Enum.Parse(typeof(TeamsNumeric), team.ToString());
            type = (sbyte)ClientMessageType.CP_OUTFIT;
            ship = (sbyte)shipType;
            pad1 = 0;
            this.team = (sbyte)teamNumeric;
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct war_cpacket
    { /* CP_WAR py-public struct "!bbxx" #10 */
        public sbyte type;
        public sbyte newmask;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct practr_cpacket
    { /* CP_PRACTR py-public struct "!bxxx" #11 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct shield_cpacket
    { /* CP_SHIELD py-public struct "!bbxx" #12 */
        public sbyte type;
        public sbyte state;		/* up/down */
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct repair_cpacket
    { /* CP_REPAIR py-public struct "!bbxx" #13 */
        public sbyte type;
        public sbyte state;		/* on/off */
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct orbit_cpacket
    { /* CP_ORBIT py-public struct "!bbxx" #14 */
        public sbyte type;
        public sbyte state;		/* on/off */
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct planlock_cpacket
    { /* CP_PLANLOCK py-public struct "!bbxx" #15 */
        public sbyte type;
        public sbyte pnum;
        public sbyte pad1;
        public sbyte pad2;

        public planlock_cpacket(sbyte pnum)
        {
            type = (sbyte)ClientMessageType.CP_PLANLOCK;
            this.pnum = pnum;
            pad1 = 0;
            pad2 = 0;
        }

    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct playlock_cpacket
    { /* CP_PLAYLOCK py-public struct "!bbxx" #16 */
        public sbyte type;
        public sbyte pnum;
        public sbyte pad1;
        public sbyte pad2;

        public playlock_cpacket(sbyte pnum)
        {
            type = (sbyte)ClientMessageType.CP_PLAYLOCK;
            this.pnum = pnum;
            pad1 = 0;
            pad2 = 0;
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct bomb_cpacket
    { /* CP_BOMB py-public struct "!bbxx" #17 */
        public sbyte type;
        public sbyte state;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct beam_cpacket
    { /* CP_BEAM py-public struct "!bbxx" #18 */
        public sbyte type;
        public sbyte state;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct cloak_cpacket
    { /* CP_CLOAK py-public struct "!bbxx" #19 */
        public sbyte type;
        public sbyte state;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct det_torps_cpacket
    { /* CP_DET_TORPS py-public struct "!bxxx" #20 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct det_mytorp_cpacket
    { /* CP_DET_MYTORP py-public struct "!bxh" #21 */
        public sbyte type;
        public sbyte pad1;
        public short tnum;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct copilot_cpacket
    { /* CP_COPILOT py-public struct "!bbxx" #22 */
        public sbyte type;
        public sbyte state;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct refit_cpacket
    { /* CP_REFIT py-public struct "!bbxx" #23 */
        public sbyte type;
        public sbyte ship;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct tractor_cpacket
    { /* CP_TRACTOR py-public struct "!bbbx" #24 */
        public sbyte type;
        public sbyte state;
        public sbyte pnum;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct repress_cpacket
    { /* CP_REPRESS py-public struct "!bbbx" #25 */
        public sbyte type;
        public sbyte state;
        public sbyte pnum;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct coup_cpacket
    { /* CP_COUP py-public struct "!bxxx" #26 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct socket_cpacket
    { /* CP_SOCKET py-public struct "!bbbxI" #27 */
        public sbyte type;
        public sbyte version;
        public sbyte udp_version;	/* was pad2 */
        public sbyte pad3;
        public UInt32 socket;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct options_cpacket
    { /* CP_OPTIONS py-public struct "!bxxxI96s" #28 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
        public UInt32 flags;
        public unsafe fixed sbyte keymap[NetConstants.KEYMAP_LEN];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct bye_cpacket
    { /* CP_BYE py-public struct "!bxxx" #29 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct dockperm_cpacket
    { /* CP_DOCKPERM py-public struct "!bbxx" #30 */
        public sbyte type;
        public sbyte state;
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct updates_cpacket
    { /* CP_UPDATES py-public struct "!bxxxI" #31 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
        public UInt32 usecs;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct resetstats_cpacket
    { /* CP_RESETSTATS py-public struct "!bbxx" #32 */
        public sbyte type;
        public sbyte verify;	/* 'Y' - just to make sure he meant it */
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct reserved_cpacket
    { /* CP_RESERVED py-public struct "!bxxx16s16s" #33 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
        public unsafe fixed sbyte data[NetConstants.RESERVED_SIZE];
        public unsafe fixed sbyte resp[NetConstants.RESERVED_SIZE];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct scan_cpacket
    { /* CP_SCAN py-public struct "!bbxx" #34 */
        public sbyte type;
        public sbyte pnum;
        public sbyte pad1;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class udp_req_cpacket
    { /* CP_UDP_REQ py-public struct "!bbbxi" #35 */
        public readonly sbyte type = (sbyte)ClientMessageType.CP_UDP_REQ;
        public sbyte request;
        public CommunicationRequest Request { set { request = (sbyte)value; } }
        public sbyte connmode;	/* respond with port # or just send UDP packet? */
        public Mode ConnectionMode { get { return (Mode)connmode; } set { connmode = (sbyte)value; } }

        public sbyte pad2;
        public int port;		/* compensate for hosed recvfrom() */

        public udp_req_cpacket(CommunicationRequest request, Mode connectionMode = Mode.Unspecified, int port = -1)
        {
            this.ConnectionMode = connectionMode;
            this.Request = request;
            this.port = port;
        }

        public static readonly udp_req_cpacket Verify = new udp_req_cpacket(CommunicationRequest.Verify);
        public static readonly udp_req_cpacket Update = new udp_req_cpacket(CommunicationRequest.Update);
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct sequence_cpacket
    {	/* UDP */
        public sbyte type;		/* CP_SEQUENCE */
        public sbyte pad1;
        public UInt16 sequence;
    };

#if RSA
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct rsa_key_cpacket
    {
        public sbyte type;          /* CP_RSA_KEY */
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
        public unsafe fixed byte global[NetConstants.KEY_SIZE];
        public unsafe fixed byte @public[NetConstants.KEY_SIZE];
        public unsafe fixed byte resp[NetConstants.KEY_SIZE];
    };
#endif

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct planet_cpacket
    {
        public sbyte type;		/* CP_PLANET */
        public sbyte pnum;
        public sbyte owner;
        public sbyte info;
        public short flags;
        public int armies;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct shortreq_cpacket
    {       /* CP_S_REQ */
        public sbyte type;
        public sbyte req;
        public sbyte version;
        public sbyte pad2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct threshold_cpacket
    {      /* CP_S_THRS */
        public sbyte type;
        public sbyte pad1;
        public UInt16 thresh;
    };



    /* flag header */
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct top_flags
    {
        public sbyte type;
        public byte packets;
        public byte numflags; /* How many flag packets */
        public byte index;   /* from which index on */
        public UInt32 tflags;
        public UInt32 tflags2;
    };

    /* The format of the body:
    public struct player_s_body_spacket {  Body of new Player Packet
            public byte pnum;      0-4 = pnum, 5 local or galactic, 6 = 9. x-bit, 7 9. y-b
    it
            public byte speeddir;  0-3 = speed , 4-7 direction of ship
            public byte x;         low 8 bits from X-Pixelcoordinate
            public byte y;         low 8 bits from Y-Pixelcoordinate
    };
    */



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct mesg_s_cpacket
    {
        public sbyte type;          /* CP_S_MESSAGE */
        public sbyte group;
        public sbyte indiv;
        public sbyte length; /* Size of whole packet   */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.MSG_LEN)]
        public string mesg;
    };


#if BASEPRACTICE || NEWBIESERVER || PRETSERVER
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct oggv_cpacket
    {
        public sbyte type;   /* CP_OGGV */
        public byte def;    /* defense 1-100 */
        public byte targ;   /* target */
    };
#endif

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct feature_cpacket
    { /* CP_FEATURE py-public struct "!bcbbi80s" #60 */

        public feature_cpacket(char featureType, sbyte arg1, sbyte arg2, int value, string name)
        {
            this.type = (sbyte)ClientMessageType.CP_FEATURE;
            this.feature_type = (sbyte)featureType;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.value = value;
            this.name = name;
        }

        public sbyte type;
        public sbyte feature_type;   /* either 'C' or 'S' */
        public sbyte arg1,
                             arg2;
        public int value;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string name;
    };

}
