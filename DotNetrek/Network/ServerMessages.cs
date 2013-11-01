using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using DotNetrek;

namespace LionFire.Netrek
{
    public static class StringExtensions
    {
        /// <summary>
        /// Get a printable 7-bit ASCII string
        /// </summary>
        public static string Clean(this string uncleanString)
        {
            if (uncleanString == null) return null;

            uncleanString = uncleanString.Replace(((char)1).ToString(), "[O]");
            for (int i = 0; i < 32; i++)
            {
                uncleanString = uncleanString.Replace(((char)i).ToString(), "?");
            }
            for (int i = 128; i < 256; i++)
            {
                uncleanString = uncleanString.Replace(((char)i).ToString(), "?");
            }
            return uncleanString;
        }
    }

    public interface IHasWar { sbyte war { get; set; } }
    public interface IHasHostile { sbyte hostile { get; set; } }
    public interface IHasTeam { sbyte team { get; set; } }
    public interface IHasShipType { sbyte shiptype { get; set; } }

    public static class Extensions
    {
        public static NetrekTeam War(this IHasWar x) { return (NetrekTeam)x.war; }
        public static NetrekTeam Hostile(this IHasHostile x) { return (NetrekTeam)x.hostile; }
        public static NetrekTeam Team(this IHasTeam x) { return (NetrekTeam)x.team; }
        public static ShipType ShipType(this IHasShipType x) { return (ShipType)x.shiptype; }
    }

    //// LionRing style API:
    //public interface SNetrekClient
    //{
    //    void Motd(sbyte pad1, sbyte pad2, sbyte pad3,
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.MSG_LEN)]
    //    string line);
    //}

    //
    // These are server --> client packets
    //

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class mesg_spacket
    { /* SP_MESSAGE py-public class "!bBBB80s" #1 */
        public sbyte type;

        public byte flags;
        public MessageFlags Flags { get { return (MessageFlags)flags; } }
        public byte recipient;
        public byte from;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.MSG_LEN)]
        public string message;
        public string Message { get { return message.Clean(); } }
        //public unsafe fixed byte mesg[Constants.MSG_LEN];
        //public unsafe fixed sbyte mesg[Constants.MSG_LEN];

        public override string ToString()
        {
            return ">SP_MESSAGE< " + Message + " [" + Flags + "]";
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class plyr_info_spacket : IHasTeam, IHasShipType
    { /* SP_PLAYER_INFO py-public class "!bbbb" #2 */
        public sbyte type;
        public sbyte pnum;
        public sbyte shiptype { get; set; }
        public sbyte team { get; set; }

        public override string ToString()
        {
            return ">SP_PLAYER_INFO< " + this.Team().ToInitial() + PlayerId.GetPlayerLetter(pnum) + ": " + this.ShipType().ToString();
        }
    };

    //public class Int32Marshaller : ICustomMarshaler
    //{
    //    public void CleanUpManagedData(object ManagedObj)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CleanUpNativeData(IntPtr pNativeData)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public int GetNativeDataSize()
    //    {
    //        return 4;
    //    }

    //    public IntPtr MarshalManagedToNative(object ManagedObj)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object MarshalNativeToManaged(IntPtr pNativeData)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class kills_spacket
    { /* SP_KILLS py-public class "!bbxxI" #3 */
        public sbyte type;
        public sbyte pnum;
        public sbyte pad1;
        public sbyte pad2;


        public float Kills { get { return ((float)/*IPAddress.NetworkToHostOrder*/(kills)) / 100f; } }
        //[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef =typeof(IntMarshaller))]
        public Int32 kills;	/* where 1234=12.34 kills and 0=0.00 kills */

        public override string ToString()
        {
            return ">SP_KILLS< Slot " + PlayerId.GetPlayerLetter(pnum) + " - kills: " + Kills;
            //+ " - kills2: " + System.Net./*IPAddress.NetworkToHostOrder*/(kills)
            //+ " - kills3: " + System.Net.IPAddress.HostToNetworkOrder(kills);
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class player_spacket
    { /* SP_PLAYER py-public class "!bbBbll" #4 */
        public sbyte type;
        public sbyte pnum;
        public byte dir;
        public sbyte speed;
        public Int32 x, y;
        public Int32 X { get { return /*IPAddress.NetworkToHostOrder*/(x); } }
        public Int32 Y { get { return /*IPAddress.NetworkToHostOrder*/(y); } }

        public override string ToString()
        {
            return ">SP_PLAYER< Slot " + pnum + " - speed: " + speed + ", dir: " + dir + " pos: " + X + "," + Y;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class torp_info_spacket : IHasWar
    { /* SP_TORP_INFO py-public class "!bbbxhxx" #5 */
        public sbyte type;
        public sbyte war { get; set; }		/* mask of teams the torp is hostile toward */
        public sbyte status;	/* new status of this torp, TFREE, TDET, etc... */
        public sbyte pad1;		/* pad needed for cross cpu compatibility */
        public short tnum;		/* the torpedo number */
        public short pad2;

        public NetrekTeam War { get { return (NetrekTeam)war; } }

        public TorpStatus Status
        {
            get { return (TorpStatus)status; }
        }

        public override string ToString()
        {
            string msg = ">SP_TORP_INFO< Torp " + tnum + " - Status: " + Status;
            if (Status != TorpStatus.Free) msg += ", war: " + War;
            return msg;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class torp_spacket
    { /* SP_TORP py-public class "!bBhll" #6 */
        public sbyte type;
        public byte dir;
        public short tnum;
        public Int32 x, y;
        public Int32 X { get { return /*IPAddress.NetworkToHostOrder*/(x); } }
        public Int32 Y { get { return /*IPAddress.NetworkToHostOrder*/(y); } }

        public override string ToString()
        {
            return ">SP_TORP< Torp " + tnum + " - Dir: " + dir + ", Location: " + X + ", " + Y;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class phaser_spacket
    { /* SP_PHASER py-public class "!bbbBlll" #7 */
        public sbyte type;
        public sbyte pnum;
        public sbyte status;	/* PH_HIT, etc... */
        public PhaserStatus Status { get { return (PhaserStatus)status; } }
        public byte dir;
        public Int32 x, y;
        public Int32 X { get { return /*IPAddress.NetworkToHostOrder*/(x); } }
        public Int32 Y { get { return /*IPAddress.NetworkToHostOrder*/(y); } }

        public Int32 target;

        public override string ToString()
        {
            string str = ">SP_PHASER< " + PlayerId.GetPlayerLetter(pnum) + "'s phaser: [" + Status + "]";

            if (Status != PhaserStatus.Free)
            {
                str += " - Dir: " + dir + ", Location: " + X + ", " + Y + ", target: " + target;
            }
            return str;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class plasma_info_spacket : IHasWar
    { /* SP_PLASMA_INFO py-public class "!bbbxhxx" #8 */
        public sbyte type;
        public sbyte war { get; set; }
        public sbyte status;	/* TFREE, TDET, etc... */
        public PlasmaStatus Status { get { return (PlasmaStatus)status; } }
        public sbyte pad1;		/* pad needed for cross cpu compatibility */
        public short pnum;
        public short pad2;

        public NetrekTeam War
        {
            get { return (NetrekTeam)war; }
        }

        public override string ToString()
        {
            string str = ">SP_PLASMA_INFO< pnum: " + pnum + " [" + Status + "]";

            if (Status != PlasmaStatus.Free)
            {
                str += ", war: " + this.War();
                //str += " - Dir: " + dir + ", Location: " + x + ", " + y + ", target: " + target;
            }
            return str;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class plasma_spacket
    { /* SP_PLASMA py-public class "!bxhll" #9 */
        public sbyte type;
        public sbyte pad1;
        public short pnum;
        public Int32 x, y;
        public Int32 X { get { return /*IPAddress.NetworkToHostOrder*/(x); } }
        public Int32 Y { get { return /*IPAddress.NetworkToHostOrder*/(y); } }

    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class warning_spacket
    { /* SP_WARNING py-public class "!bxxx80s" #10 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.MSG_LEN)]
        public string mesg;
        public string Message { get { return mesg.Clean(); } }
        //public unsafe fixed sbyte mesg[Constants.MSG_LEN];

        public override string ToString()
        {
            //return ">SP_WARNING< " + Message;
            return "Server->Player " + Message;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class motd_spacket
    { /* SP_MOTD py-public class "!bxxx80s" #11 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.MSG_LEN)]
        public string line;
        //public unsafe fixed sbyte line[Constants.MSG_LEN];

        public override string ToString()
        {
            return ">SP_MOTD< " + line;
        }
    };

    #region (TEMP NOTE: From client file)

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class you_short_spacket : IHasHostile, IHasWar
    {       /* SP_S_YOU */
        public sbyte type;

        public sbyte pnum;
        public sbyte hostile { get; set; }
        public sbyte war { get; set; } // Jared note: field name was swar

        public sbyte armies;
        public sbyte whydead;
        public sbyte whodead;

        public sbyte pad1;

        public UInt32 flags;
        public PlayerFlags Flags { get { return (PlayerFlags)flags; } }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class youss_spacket
    {          /* SP_S_YOU_SS */
        public sbyte type;
        public sbyte pad1;

        public UInt16 damage;
        public UInt16 shield;
        public UInt16 fuel;
        public UInt16 etemp;
        public UInt16 wtemp;
    };

    #endregion

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class you_spacket : IHasWar, IHasHostile
    { /* SP_YOU py-public class "!bbbbbbxxIlllhhhh" #12 */
        public sbyte type;
        public sbyte pnum;		/* Guy needs to know this... */
        public string PlayerSlot { get { return PlayerId.GetPlayerLetter(pnum); } }
        public sbyte hostile { get; set; }
        public NetrekTeam Hostile { get { return (NetrekTeam)hostile; } }
        public sbyte war { get; set; } // Jared note: field name was swar
        public NetrekTeam War { get { return (NetrekTeam)war; } }
        public sbyte armies;
        public sbyte tractor;	/* ATM - visible tractor (was pad1) */
        public sbyte pad2;
        public sbyte pad3;
        public UInt32 flags;
        public PlayerFlags Flags { get { return (PlayerFlags)flags; } }
        public Int32 damage;
        public Int32 shield;
        public Int32 fuel;
        public short etemp;
        public short wtemp;
        public short whydead;
        public short whodead;

        public override string ToString()
        {
            return ">SP_YOU< Slot " + PlayerSlot + " - [Hostile: " + Hostile + "] [War: " + War + "] [Flags: " + Flags + "]";
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class queue_spacket
    { /* SP_QUEUE py-public class "!bxh" #13 */
        public sbyte type;
        public sbyte pad1;
        public short pos;

        public override string ToString()
        {
            return ">SP_QUEUE< Queue " + pos;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class status_spacket
    { /* SP_STATUS py-public class "!bbxxIIIIIL" #14 */
        public sbyte type;
        public sbyte tourn;
        public sbyte pad1;
        public sbyte pad2;
        public UInt32 armsbomb;
        public UInt32 planets;
        public UInt32 kills;
        public UInt32 losses;
        public UInt32 time;
        public /* UInt64 */ UInt32 timeprod;

        public override string ToString()
        {
            return ">SP_STATUS< timeprod = " + timeprod + " ... ";
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class planet_spacket
    { /* SP_PLANET py-public class "!bbbbhxxl" #15 */
        public sbyte type;
        public sbyte pnum;
        public sbyte owner;
        public sbyte info;
        public short flags;
        public PlanetFlags Flags { get { return (PlanetFlags)flags; } }
        public short pad2;
        public Int32 armies;

        public override string ToString()
        {
            return ">SP_PLANET< Planet " + pnum + " - Flags: " + Flags + ", armies: " + armies + " info: " + info + " owner: " + owner;

        }

    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ping_cpacket
    { /* CP_PING_RESPONSE py-public class "!bBbxll" #42 */
        public sbyte type;
        public byte number;         /* id */
        public sbyte pingme;         /* if client wants server to ping */
        public sbyte pad1;

        public Int32 cp_sent;        /* # packets sent to server */
        public Int32 cp_recv;        /* # packets recv from server */

        public ping_cpacket(byte number, int packetsSent, int packetsReceived, sbyte pingme = 0)
        {
            type = (sbyte)ClientMessageType.CP_PING_RESPONSE;
            pad1 = 0;
            this.number = number;
            this.cp_sent = packetsSent;
            this.cp_recv = packetsReceived;
            this.pingme = pingme;
        }

    };      /* 12 bytes */

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ping_spacket
    { /* SP_PING py-public class "!bBHBBBB" #46 */
        public sbyte type;
        public byte number;         /* id */
        public UInt16 lag;            /* delay in ms */

        public byte tloss_sc;       /* total loss server-client 0-100% */
        public byte tloss_cs;       /* total loss client-server 0-100% */

        public byte iloss_sc;       /* inc. loss server-client 0-100% */
        public byte iloss_cs;       /* inc. loss client-server 0-100% */

    };      /* 8 bytes */

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class pickok_spacket
    { /* SP_PICKOK py-public class "!bbxx" #16 */
        public sbyte type;
        public sbyte state;         /* 0=no, 1=yes */
        public string State
        {
            get
            {
                switch (state)
                {
                    case 0: return "No";
                    case 1: return "Yes";
                    default: return "Unknown";
                }
            }
        }
        public sbyte pad2;
        public sbyte pad3;

        public override string ToString()
        {
            return ">SP_PICKOK< " + State;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)] // , Size = 104
    public class login_spacket
    { /* SP_LOGIN py-public class "!bbxxl96s" #17 */
        public sbyte type;
        public sbyte accept;	/* 1/0 */
        public sbyte pad2;
        public sbyte pad3;
        public Int32 flags;
        public STFlags Flags { get { return (STFlags)flags; } }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.KEYMAP_LEN)]
        public string keymap;
        //public unsafe fixed sbyte keymap[NetConstants.KEYMAP_LEN];

    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class flags_spacket
    { /* SP_FLAGS py-public class "!bbbxI" #18 */
        public sbyte type;
        public sbyte pnum;		/* whose flags are they? */
        public sbyte tractor;	/* ATM - visible tractors */
        public sbyte pad2;
        public UInt32 flags;

        public string PlayerSlot
        {
            get { return PlayerId.GetPlayerLetter(pnum); }
        }
        public PlayerFlags Flags
        {
            get { return (PlayerFlags)flags; }
        }
        public override string ToString()
        {
            return ">SP_FLAGS< Slot " + PlayerSlot + " - flags: " + Flags + ", tractor: " + tractor;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class mask_spacket
    { /* SP_MASK py-public class "!bbxx" #19 */
        public sbyte type;
        public sbyte mask;
        public sbyte pad1;
        public sbyte pad2;

        public NetrekTeam Mask { get { return (NetrekTeam)mask; } }

        public override string ToString()
        {
            return ">SP_MASK< Joinable teams: " + Mask;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class pstatus_spacket
    { /* SP_PSTATUS py-public class "!bbbx" #20 */
        public sbyte type;
        public sbyte pnum;
        public sbyte status;
        public sbyte pad1;

        public PlayerStatus Status
        {
            get { return (PlayerStatus)status; }
        }
        public override string ToString()
        {
            return ">SP_PSTATUS< Slot " + PlayerId.GetPlayerLetter(pnum) + " - status: " + Status;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class badversion_spacket
    { /* SP_BADVERSION py-public class "!bbxx" #21 */
        public sbyte type;
        public sbyte why;
        public sbyte pad2;
        public sbyte pad3;

        public override string ToString()
        {
            return ">SP_BADVERSION< Why: " + why;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class hostile_spacket : IHasWar
    { /* SP_HOSTILE py-public class "!bbbb" #22 */
        public sbyte type;
        public sbyte pnum;
        public sbyte war { get; set; }
        public sbyte hostile { get; set; }

        public string PlayerSlot
        {
            get { return PlayerId.GetPlayerLetter(pnum); }
        }
        public NetrekTeam War
        {
            get { return (NetrekTeam)war; }
        }
        public NetrekTeam Hostile
        {
            get { return (NetrekTeam)hostile; }
        }

        public override string ToString()
        {
            return ">SP_HOSTILE< Slot " + PlayerSlot + " - hostile: " + Hostile + ", war: " + War;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class stats_spacket
    { /* SP_STATS py-public class "!bbxx13l" #23 */
        public sbyte type;
        public sbyte pnum;
        public sbyte pad1;
        public sbyte pad2;
        public Int32 tkills;	/* Tournament kills */
        public Int32 tlosses;	/* Tournament losses */
        public Int32 kills;		/* overall */
        public Int32 losses;	/* overall */
        public Int32 tticks;	/* ticks of tournament play time */
        public Int32 tplanets;	/* Tournament planets */
        public Int32 tarmies;	/* Tournament armies */
        public Int32 sbkills;	/* Starbase kills */
        public Int32 sblosses;	/* Starbase losses */
        public Int32 armies;	/* non-tourn armies */
        public Int32 planets;	/* non-tourn planets */
        public Int32 maxkills;	/* max kills as player * 100 */
        public Int32 sbmaxkills;	/* max kills as sb * 100 */

        public string PlayerSlot
        {
            get { return PlayerId.GetPlayerLetter(pnum); }
        }

        public override string ToString()
        {
            return ">SP_STATS< Slot " + PlayerSlot + " - stats: ...";
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class plyr_login_spacket
    { /* SP_PL_LOGIN py-public class "!bbbx16s16s16s" #24 */
        public sbyte type;
        public sbyte pnum;
        public sbyte rank;
        public sbyte pad1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string name;
        public string Name { get { return name.Clean(); } }
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string monitor;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string login;
        //public unsafe fixed sbyte name[Constants.NAME_LEN];
        //public unsafe fixed sbyte monitor[Constants.NAME_LEN];
        //public unsafe fixed sbyte login[Constants.NAME_LEN];

        public override string ToString()
        {
            //return ">SP_PL_LOGIN< Slot " + pnum + " " + name + " - rank: " + rank + ", login: " + login;
            return ">SP_PL_LOGIN< Slot " + pnum + " " + Name + " - rank: " + rank + ", login: " + login;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class reserved_spacket
    { /* SP_RESERVED py-public class "!bxxx16s" #25 */
        public sbyte type;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        //public unsafe fixed sbyte data[NetConstants.RESERVED_SIZE];
        public string data;

        public override string ToString()
        {
            return ">SP_RESERVED<";
            //return ">SP_RESERVED< " + data.Clean();
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class planet_loc_spacket
    { /* SP_PLANET_LOC py-public class "!bbxxll16s" #26 */
        public sbyte type;
        public sbyte pnum;
        public sbyte pad2;
        public sbyte pad3;
        public Int32 x;
        public Int32 y;
        public Int32 X { get { return /*IPAddress.NetworkToHostOrder*/(x); } }
        public Int32 Y { get { return /*IPAddress.NetworkToHostOrder*/(y); } }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string name;
        public string Name { get { return name.Clean(); } }

        public override string ToString()
        {
            return ">SP_PLANET_LOC< Planet " + Name + " (" + pnum + ") is at " + X + "," + Y;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class scan_spacket
    {		/* ATM */
        public sbyte type;		/* SP_SCAN */
        public sbyte pnum;
        public sbyte success;
        public sbyte pad1;
        public Int32 p_fuel;
        public Int32 p_armies;
        public Int32 p_shield;
        public Int32 p_damage;
        public Int32 p_etemp;
        public Int32 p_wtemp;

        public string PlayerSlot
        {
            get { return PlayerId.GetPlayerLetter(pnum); }
        }

        public override string ToString()
        {
            return ">SP_SCAN< Player " + PlayerSlot + " Scan success: " + success + " ...";
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class udp_reply_spacket
    { /* SP_UDP_REPLY py-public class '!bbxxi' #28 */
        public sbyte type;
        public sbyte reply;
        public CommunicationSwitch Reply { get { return (CommunicationSwitch)reply; } }
        public sbyte pad1;
        public sbyte pad2;
        public int port;

        public override string ToString()
        {
            return ">SP_UDP_REPLY< Port " + port + " reply: " + Reply;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class sequence_spacket
    { /* SP_SEQUENCE py-public class '!bBH' #29 */
        public sbyte type;
        public byte flag8;
        public UInt16 sequence;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class sc_sequence_spacket
    {	/* UDP */
        public sbyte type;		/* SP_CP_SEQUENCE */
        public sbyte pad1;
        public UInt16 sequence;
    };

#if RSA
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct rsa_key_spacket
    {
        public sbyte type;          /* SP_RSA_KEY */
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
        public unsafe fixed byte data[NetConstants.KEY_SIZE];
    };
#endif
    //}


    #region Generic

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct generic_32_spacket
    {
        public sbyte type;
        public unsafe fixed sbyte pad[31];
    };



    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct generic_32_spacket_a
    { /* SP_GENERIC_32 py-public class "b1sHH26x" #32 */
        public sbyte type;
        public sbyte version;        /* alphabetic, 0x60 + version */
        public UInt16 repair_time;    /* server estimate of repair time in seconds */
        public UInt16 pl_orbit;       /* what planet player orbiting, -1 if none */
        public unsafe fixed sbyte pad1[26];
        /* NOTE: this version didn't use network public byte order for the shorts */
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct generic_32_spacket_b
    { /* SP_GENERIC_32 py-public class "!b1sHbHBBsBsBB18x" #32 */
        public sbyte type;
        public sbyte version;        /* alphabetic, 0x60 + version */
        public UInt16 repair_time;    /* server estimate of repair time, seconds  */
        public sbyte pl_orbit;       /* what planet player orbiting, -1 if none  */
        public UInt16 gameup;                  /* server status flags             */
        public byte tournament_teams;        /* what teams are involved         */
        public byte tournament_age;          /* time since last t-mode start    */
        public sbyte tournament_age_units;    /* units for above, see s2du       */
        public byte tournament_remain;       /* remaining INL game time         */
        public sbyte tournament_remain_units; /* units for above, see s2du       */
        public byte starbase_remain;         /* starbase reconstruction, mins   */
        public byte team_remain;             /* team surrender time, seconds    */
        public unsafe fixed sbyte pad1[18];
    } // __attribute__ ((packed));  // TODO


    /* SP_GENERIC_32 versioning instructions:

       we start with version 'a', and each time a structure is changed
       increment the version and reduce the pad size, keeping the packet
       the same size ...

       client is entitled to trust fields in public class that were defined at a
       particular version ...

       client is to send CP_FEATURE with SP_GENERIC_32 value 1 for version
       'a', value 2 for version 'b', etc ...

       server is to reply with SP_FEATURE with SP_GENERIC_32 value set to
       the maximum version it supports (not the version requested by the
       client), ...

       server is to send SP_GENERIC_32 packets of the highest version it
       knows about, but no higher than the version the client asks for.
    */

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class flags_all_spacket
    {
        public sbyte type;           /* SP_FLAGS_ALL */
        public sbyte offset;         /* slot number of first flag */
        public int flags;          /* two bits per slot */
        public const int FLAGS_ALL_DEAD = 0;
        public const int FLAGS_ALL_CLOAK_ON = 1;
        public const int FLAGS_ALL_CLOAK_OFF_SHIELDS_UP = 2;
        public const int FLAGS_ALL_CLOAK_OFF_SHIELDS_DOWN = 3;
    };

    #endregion


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    /* Server configuration of client */
    public class ship_cap_spacket
    { /* SP_SHIP_CAP py-public class "!bbHHHiiiiiiHHH1sx16s2sH" #39 */
        public sbyte type;		/* screw motd method */
        public sbyte operation;	/* 0 = add/change a ship, 1 = remove a ship */
        public UInt16 s_type;
        public UInt16 s_torpspeed;
        public UInt16 s_phaserrange;
        public int s_maxspeed;
        public int s_maxfuel;
        public int s_maxshield;
        public int s_maxdamage;
        public int s_maxwpntemp;
        public int s_maxegntemp;
        public UInt16 s_width;
        public UInt16 s_height;
        public UInt16 s_maxarmies;
        public sbyte s_letter;
        public sbyte pad2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string s_name;
        public sbyte s_desig1;
        public sbyte s_desig2;
        public UInt16 s_bitmap;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class feature_spacket
    { /* SP_FEATURE py-public class "!bcbbi80s" #60 */
        public sbyte type;
        public sbyte feature_type;   /* either 'C' or 'S' */
        public sbyte arg1,
                             arg2;
        public int value;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string name;
        public string Name { get { return name.Clean(); } }
        //public unsafe fixed sbyte name[80];

        public override string ToString()
        {
            return ">SP_FEATURE< " + (char)feature_type + " " + Name + " = " + value;
        }

    };

    #region Short

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class shortreply_spacket
    {     /* SP_S_REPLY */
        public sbyte type;
        public sbyte repl;
        public UInt16 winside;
        public Int32 gwidth;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class planet_s_spacket
    {       /* body of SP_S_PLANET  */
        public sbyte pnum;
        public sbyte owner;
        public sbyte info;
        public byte armies;       /* more than 255 Armies ? ...  */
        public short flags;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class warning_s_spacket
    {              /* SP_S_WARNING */
        public sbyte type;
        public byte whichmessage;
        public sbyte argument, argument2; /* for phaser  etc ... */
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class player_s_spacket
    {
        public sbyte type;          /* SP_S_PLAYER Header */

        public sbyte packets; /* How many player-packets are in this packet  ( only the firs
t 6 bits are relevant ) */
        public byte dir;
        public sbyte speed;
        public Int32 x, y;   /* To get the absolute Position */
        public Int32 X { get { return /*IPAddress.NetworkToHostOrder*/(x); } }
        public Int32 Y { get { return /*IPAddress.NetworkToHostOrder*/(y); } }

    };

    /* S_P2 */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class player_s2_spacket
    {
        public sbyte type;          /* SP_S_PLAYER Header */

        public sbyte packets; /* How many player-packets are in this packet  ( only the first 6 bits are relevant ) */
        public byte dir;
        public sbyte speed;
        public short x, y;  /* absolute position / 40 */
        public Int32 flags;   /* 16 playerflags */
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct torp_s_spacket
    {
        public sbyte type; /* SP_S_TORP */
        public byte bitset;      /* bit=1 that torp is in packet */
        public byte whichtorps; /* Torpnumber of first torp / 8 */
        public unsafe fixed byte data[21]; /* For every torp 2*9 bit coordinates */
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class mesg_s_spacket
    {
        public sbyte type;          /* SP_S_MESSAGE */
        public byte m_flags;
        public byte m_recpt;
        public byte m_from;
        public byte length;       /* Length of whole packet */
        public sbyte mesg;
        public sbyte pad2;
        public sbyte pad3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 76)]
        public string pad;
    };

    /* S_P2 */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct kills_s_spacket
    {
        public sbyte type;                  /* SP_S_KILLS */
        public sbyte pnum;                  /* How many kills in packet */
        public UInt16 kills;       /* 6 bit player numer   */
        /* 10 bit kills*100     */
        public unsafe fixed UInt16 mkills[NetConstants.MAXPLAYER];
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class phaser_s_spacket
    {
        public sbyte type;                  /* SP_S_PHASER */
        public byte status;                /* PH_HIT, etc... */
        public byte pnum;                 /* both bytes are used for more */
        public byte target;               /* look into the code   */
        public short x;                    /* x coord /40 */
        public short y;                    /* y coord /40 */
        public byte dir;
        public sbyte pad1;
        public sbyte pad2;
        public sbyte pad3;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class stats_s_spacket
    {
        public sbyte type;                  /* SP_S_STATS */
        public sbyte pnum;
        public UInt16 tplanets;              /* Tournament planets */
        public UInt16 tkills;                /* Tournament kills */
        public UInt16 tlosses;               /* Tournament losses */
        public UInt16 kills;                 /* overall */
        public UInt16 losses;                /* overall */
        public UInt32 tticks;                /* ticks of tournament play time */
        public UInt32 tarmies;               /* Tournament armies */
        public UInt32 maxkills;
        public UInt16 sbkills;               /* Starbase kills */
        public UInt16 sblosses;              /* Starbase losses */
        public UInt16 armies;                /* non-tourn armies */
        public UInt16 planets;               /* non-tourn planets */
        public UInt32 sbmaxkills;            /* max kills as sb * 100 */
    };

    #endregion

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class rank_spacket
    { /* SP_RANK py-public class pending #61 */
        public sbyte type;
        public sbyte rnum;           /* rank number */
        public sbyte rmax;           /* rank number maximum */
        public sbyte pad;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetConstants.NAME_LEN)]
        public string name; /* full rank name */
        public string Name { get { return name.Clean(); } }
        public int hours;          /* hundredths of hours required */
        public int ratings;        /* hundredths of ratings required */
        public int offense;        /* hundredths of offense required */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string cname;       /* public short 'curt' rank name */
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ltd_spacket
    { /* SP_LTD py-public class pending #62 */
        public sbyte type;
        public sbyte version;
        //public unsafe fixed sbyte pad[2];
        public sbyte pad1;
        public sbyte pad2;
        public UInt32 kt;    /* kills total, kills.total */
        public UInt32 kmax;  /* kills max, kills.max */
        public UInt32 k1;    /* kills first, kills.first */
        public UInt32 k1p;   /* kills first potential, kills.first_potential */
        public UInt32 k1c;   /* kills first converted, kills.first_converted */
        public UInt32 k2;    /* kills second, kills.second */
        public UInt32 k2p;   /* kills second potential, kills.second_potential */
        public UInt32 k2c;   /* kills second converted, kills.second_converted */
        public UInt32 kbp;   /* kills by phaser, kills.phasered */
        public UInt32 kbt;   /* kills by torp, kills.torped */
        public UInt32 kbs;   /* kills by smack, kills.plasmaed */
        public UInt32 dt;    /* deaths total, deaths.total */
        public UInt32 dpc;   /* deaths as potential carrier, deaths.potential */
        public UInt32 dcc;   /* deaths as converted carrier, deaths.converted */
        public UInt32 ddc;   /* deaths as dooshed carrier, deaths.dooshed */
        public UInt32 dbp;   /* deaths by phaser, deaths.phasered */
        public UInt32 dbt;   /* deaths by torp, deaths.torped */
        public UInt32 dbs;   /* deaths by smack, deaths.plasmaed */
        public UInt32 acc;   /* actual carriers created, deaths.acc */
        public UInt32 ptt;   /* planets taken total, planets.taken */
        public UInt32 pdt;   /* planets destroyed total, planets.destroyed */
        public UInt32 bpt;   /* bombed planets total, bomb.planets */
        public UInt32 bp8;   /* bombed planets <=8, bomb.planets_8 */
        public UInt32 bpc;   /* bombed planets core, bomb.planets_core */
        public UInt32 bat;   /* bombed armies total, bomb.armies */
        public UInt32 ba8;   /* bombed_armies <= 8, bomb.armies_8 */
        public UInt32 bac;   /* bombed armies core, bomb.armies_core */
        public UInt32 oat;   /* ogged armies total, ogged.armies */
        public UInt32 odc;   /* ogged dooshed carrier, ogged.dooshed */
        public UInt32 occ;   /* ogged converted carrier, ogged.converted */
        public UInt32 opc;   /* ogged potential carrier, ogged.potential */
        public UInt32 ogc;   /* ogged bigger carrier, ogged.bigger_ship */
        public UInt32 oec;   /* ogged same carrier, ogged.same_ship */
        public UInt32 olc;   /* ogger smaller carrier, ogged.smaller_ship */
        public UInt32 osba;  /* ogged sb armies, ogged.sb_armies */
        public UInt32 ofc;   /* ogged friendly carrier, ogged.friendly */
        public UInt32 ofa;   /* ogged friendly armies, ogged.friendly_armies */
        public UInt32 at;    /* armies carried total, armies.total */
        public UInt32 aa;    /* armies used to attack, armies.attack */
        public UInt32 ar;    /* armies used to reinforce, armies.reinforce */
        public UInt32 af;    /* armies ferried, armies.ferries */
        public UInt32 ak;    /* armies killed, armies.killed */
        public UInt32 ct;    /* carries total, carries.total */
        public UInt32 cp;    /* carries partial, carries.partial */
        public UInt32 cc;    /* carries completed, carries.completed */
        public UInt32 ca;    /* carries to attack, carries.attack */
        public UInt32 cr;    /* carries to reinforce, carries.reinforce */
        public UInt32 cf;    /* carries to ferry, carries.ferries */
        public UInt32 tt;    /* ticks total, ticks.total */
        public UInt32 tyel;  /* ticks in yellow, ticks.yellow */
        public UInt32 tred;  /* ticks in red, ticks.red */
        public UInt32 tz0;   /* ticks in zone 0, ticks.zone[0] */
        public UInt32 tz1;   /* ticks in zone 1, ticks.zone[1] */
        public UInt32 tz2;   /* ticks in zone 2, ticks.zone[2] */
        public UInt32 tz3;   /* ticks in zone 3, ticks.zone[3] */
        public UInt32 tz4;   /* ticks in zone 4, ticks.zone[4] */
        public UInt32 tz5;   /* ticks in zone 5, ticks.zone[5] */
        public UInt32 tz6;   /* ticks in zone 6, ticks.zone[6] */
        public UInt32 tz7;   /* ticks in zone 7, ticks.zone[7] */
        public UInt32 tpc;   /* ticks as potential carrier, ticks.potential */
        public UInt32 tcc;   /* ticks as carrier++, ticks.carrier */
        public UInt32 tr;    /* ticks in repair, ticks.repair */
        public UInt32 dr;    /* damage repaired, damage_repaired */
        public UInt32 wpf;   /* weap phaser fired, weapons.phaser.fired */
        public UInt32 wph;   /* weap phaser hit, weapons.phaser.hit */
        public UInt32 wpdi;  /* weap phaser damage inflicted, weapons.phaser.damage.inflicted */
        public UInt32 wpdt;  /* weap phaser damage taken, weapons.phaser.damage.taken */
        public UInt32 wtf;   /* weap torp fired, weapons.torps.fired */
        public UInt32 wth;   /* weap torp hit, weapons.torps.hit */
        public UInt32 wtd;   /* weap torp detted, weapons.torps.detted */
        public UInt32 wts;   /* weap torp self detted, weapons.torps.selfdetted */
        public UInt32 wtw;   /* weap torp hit wall, weapons.torps.wall */
        public UInt32 wtdi;  /* weap torp damage inflicted, weapons.torps.damage.inflicted */
        public UInt32 wtdt;  /* weap torp damage taken, weapons.torps.damage.taken */
        public UInt32 wsf;   /* weap smack fired, weapons.plasma.fired */
        public UInt32 wsh;   /* weap smack hit, weapons.plasma.hit */
        public UInt32 wsp;   /* weap smack phasered, weapons.plasma.phasered */
        public UInt32 wsw;   /* weap smack hit wall, weapons.plasma.wall */
        public UInt32 wsdi;  /* weap smack damage inflicted, weapons.plasma.damage.inflicted */
        public UInt32 wsdt;  /* weap smack damage taken, weapons.plasma.damage.taken */
    } // __attribute__ ((packed)); // TODO

}
