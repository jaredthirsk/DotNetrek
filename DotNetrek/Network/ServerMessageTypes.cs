using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LionFire.Netrek
{
    //    public static class SMessageTypes
    //    {
    //        /* packets sent from xtrek server to remote client */
    //        public const int SP_MESSAGE = 1;
    //        public const int SP_PLAYER_INFO = 2;		/* general player info not elsewhere */
    //        public const int SP_KILLS = 3; 		/* # kills a player has */
    //        public const int SP_PLAYER = 4;		/* x,y for player */
    //        public const int SP_TORP_INFO = 5;		/* torp status */
    //        public const int SP_TORP = 6;		/* torp location */
    //        public const int SP_PHASER = 7;		/* phaser status and direction */
    //        public const int SP_PLASMA_INFO = 8;		/* player login information */
    //        public const int SP_PLASMA = 9;		/* like SP_TORP */
    //        public const int SP_WARNING = 10;		/* like SP_MESG */
    //        public const int SP_MOTD = 11;		/* line from .motd screen */
    //        public const int SP_YOU = 12;		/* info on you? */
    //        public const int SP_QUEUE = 13;		/* estimated loc in queue? */
    //        public const int SP_STATUS = 14;		/* galaxy status numbers */
    //        public const int SP_PLANET = 15;		/* planet armies & facilities */
    //        public const int SP_PICKOK = 16;		/* your team & ship was accepted */
    //        public const int SP_LOGIN = 17;		/* login response */
    //        public const int SP_FLAGS = 18;		/* give flags for a player */
    //        public const int SP_MASK = 19;		/* tournament mode mask */
    //        public const int SP_PSTATUS = 20;		/* give status for a player */
    //        public const int SP_BADVERSION = 21;		/* invalid version number */
    //        public const int SP_HOSTILE = 22;		/* hostility settings for a player */
    //        public const int SP_STATS = 23;		/* a player's statistics */
    //        public const int SP_PL_LOGIN = 24;		/* new player logs in */
    //        public const int SP_RESERVED = 25;		/* for future use */
    //        public const int SP_PLANET_LOC = 26;		/* planet name, x, y */

    //        public const int SP_UDP_REPLY = 28;		/* notify client of UDP status */
    //        public const int SP_SEQUENCE = 29;		/* sequence # packet */
    //        public const int SP_SC_SEQUENCE = 30;		/* this trans is semi-critical info */
    //        public const int SP_RSA_KEY = 31;		/* handles binary verification */
    //        public const int SP_GENERIC_32 = 32;		/* 32 public byte generic, see public struct */
    //        public const int SP_FLAGS_ALL = 33;		/* abbreviated flags for all players */

    //        public const int SP_SHIP_CAP = 39;		/* Handles server ship mods */
    //        public const int SP_S_REPLY = 40;              /* reply to send-public short request */
    //        public const int SP_S_MESSAGE = 41;              /* var. Message Packet */
    //        public const int SP_S_WARNING = 42;              /* Warnings with 4  Bytes */
    //        public const int SP_S_YOU = 43;              /* hostile,armies,whydead,etc .. */
    //        public const int SP_S_YOU_SS = 44;              /* your ship status */
    //        public const int SP_S_PLAYER = 45;              /* variable length player packet */
    //        public const int SP_PING = 46;              /* ping packet */
    //        public const int SP_S_TORP = 47;              /* variable length torp packet */
    //        public const int SP_S_TORP_INFO = 48;              /* SP_S_TORP with TorpInfo */
    //        public const int SP_S_8_TORP = 49;              /* optimized SP_S_TORP */
    //        public const int SP_S_PLANET = 50;             /* see SP_PLANET */
    //        /* S_P2 */

    //        public const int SP_S_SEQUENCE = 56;	/* SP_SEQUENCE for compressed packets */
    //        public const int SP_S_PHASER = 57;      /* see public struct */
    //        public const int SP_S_KILLS = 58;      /* # of kills player have */
    //        public const int SP_S_STATS = 59;      /* see SP_STATS */
    //        public const int SP_FEATURE = 60;
    //        public const int SP_RANK = 61;
    //#if LTD_STATS
    //            public const int SP_LTD = 62;      /* LTD stats for character */
    //#endif

    //        public const int TOTAL_SPACKETS = SP_LTD + 1 + 1; /* length of packet sizes array */
    //    }

    public static class ServerMessages
    {

        public static int GetServerMessageSize(int serverMessageId)
        {
            if (serverMessageId <= 0) return -1;

            ServerMessageType messageType = (ServerMessageType)serverMessageId;

            switch (messageType)
            {
                case ServerMessageType.SP_MESSAGE:
                    return Marshal.SizeOf(typeof(mesg_spacket));
                case ServerMessageType.SP_PLAYER_INFO:
                    return Marshal.SizeOf(typeof(plyr_info_spacket));
                case ServerMessageType.SP_KILLS:
                    return Marshal.SizeOf(typeof(kills_spacket));
                case ServerMessageType.SP_PLAYER:
                    return Marshal.SizeOf(typeof(player_spacket));
                case ServerMessageType.SP_TORP_INFO:
                    return Marshal.SizeOf(typeof(torp_info_spacket));
                case ServerMessageType.SP_TORP:
                    return Marshal.SizeOf(typeof(torp_spacket));
                case ServerMessageType.SP_PHASER:
                    return Marshal.SizeOf(typeof(phaser_spacket));
                case ServerMessageType.SP_PLASMA_INFO:
                    return Marshal.SizeOf(typeof(plasma_info_spacket));
                case ServerMessageType.SP_PLASMA:
                    return Marshal.SizeOf(typeof(plasma_spacket));
                case ServerMessageType.SP_WARNING:
                    return Marshal.SizeOf(typeof(warning_spacket));
                case ServerMessageType.SP_MOTD:
                    return Marshal.SizeOf(typeof(motd_spacket));
                case ServerMessageType.SP_YOU:
                    return Marshal.SizeOf(typeof(you_spacket));
                case ServerMessageType.SP_QUEUE:
                    return Marshal.SizeOf(typeof(queue_spacket));
                case ServerMessageType.SP_STATUS:
                    return Marshal.SizeOf(typeof(status_spacket));
                case ServerMessageType.SP_PLANET:
                    return Marshal.SizeOf(typeof(planet_spacket));
                case ServerMessageType.SP_PICKOK:
                    return Marshal.SizeOf(typeof(pickok_spacket));
                case ServerMessageType.SP_LOGIN:
                    return Marshal.SizeOf(typeof(login_spacket));
                case ServerMessageType.SP_FLAGS:
                    return Marshal.SizeOf(typeof(flags_spacket));
                case ServerMessageType.SP_MASK:
                    return Marshal.SizeOf(typeof(mask_spacket));
                case ServerMessageType.SP_PSTATUS:
                    return Marshal.SizeOf(typeof(pstatus_spacket));
                case ServerMessageType.SP_BADVERSION:
                    return Marshal.SizeOf(typeof(badversion_spacket));
                case ServerMessageType.SP_HOSTILE:
                    return Marshal.SizeOf(typeof(hostile_spacket));
                case ServerMessageType.SP_STATS:
                    return Marshal.SizeOf(typeof(stats_spacket));
                case ServerMessageType.SP_PL_LOGIN:
                    return Marshal.SizeOf(typeof(plyr_login_spacket));
                case ServerMessageType.SP_RESERVED:
                    return Marshal.SizeOf(typeof(reserved_spacket));
                case ServerMessageType.SP_PLANET_LOC:
                    return Marshal.SizeOf(typeof(planet_loc_spacket));
                case ServerMessageType.SP_UDP_REPLY:
                    return Marshal.SizeOf(typeof(udp_reply_spacket));
                case ServerMessageType.SP_SEQUENCE:
                    return Marshal.SizeOf(typeof(sequence_spacket));
                case ServerMessageType.SP_SC_SEQUENCE:
                    return Marshal.SizeOf(typeof(sc_sequence_spacket));
                case ServerMessageType.SP_RSA_KEY:
                    return Marshal.SizeOf(typeof(rsa_key_spacket));
                case ServerMessageType.SP_GENERIC_32:
                    return Marshal.SizeOf(typeof(generic_32_spacket));
                case ServerMessageType.SP_FLAGS_ALL:
                    return Marshal.SizeOf(typeof(flags_all_spacket));
                case ServerMessageType.SP_SHIP_CAP:
                    return Marshal.SizeOf(typeof(ship_cap_spacket));
                case ServerMessageType.SP_S_REPLY:
                    return Marshal.SizeOf(typeof(shortreply_spacket));
                case ServerMessageType.SP_S_MESSAGE:
                    return Marshal.SizeOf(typeof(mesg_s_spacket));
                case ServerMessageType.SP_S_WARNING:
                    return Marshal.SizeOf(typeof(warning_s_spacket));
                case ServerMessageType.SP_S_YOU:
                    return Marshal.SizeOf(typeof(you_short_spacket));
                case ServerMessageType.SP_S_YOU_SS:
                    return Marshal.SizeOf(typeof(youss_spacket));
                case ServerMessageType.SP_S_PLAYER:
                    return Marshal.SizeOf(typeof(player_s_spacket));
                case ServerMessageType.SP_PING:
                    return Marshal.SizeOf(typeof(ping_spacket));
                case ServerMessageType.SP_S_TORP:
                    return Marshal.SizeOf(typeof(torp_s_spacket));
                case ServerMessageType.SP_S_TORP_INFO:
                    return Marshal.SizeOf(typeof(torp_s_spacket));
                case ServerMessageType.SP_S_8_TORP:
                    return Marshal.SizeOf(typeof(torp_s_spacket));
                case ServerMessageType.SP_S_PLANET:
                    return Marshal.SizeOf(typeof(planet_s_spacket));
                case ServerMessageType.SP_S_SEQUENCE:
                    return Marshal.SizeOf(typeof(sequence_spacket));
                case ServerMessageType.SP_S_PHASER:
                    return Marshal.SizeOf(typeof(phaser_s_spacket));
                case ServerMessageType.SP_S_KILLS:
                    return Marshal.SizeOf(typeof(kills_s_spacket));
                case ServerMessageType.SP_S_STATS:
                    return Marshal.SizeOf(typeof(stats_s_spacket));
                case ServerMessageType.SP_FEATURE:
                    return Marshal.SizeOf(typeof(feature_spacket));
                case ServerMessageType.SP_RANK:
                    return Marshal.SizeOf(typeof(rank_spacket));
                case ServerMessageType.SP_LTD:
                    return Marshal.SizeOf(typeof(ltd_spacket));
                case ServerMessageType.TOTAL_SPACKETS:
                    break;
                default:
                    break;
            }
            return -1;
        }

    
    }

    public enum ServerMessageType
    {
        /* packets sent from xtrek server to remote client */
        SP_MESSAGE = 1,
        SP_PLAYER_INFO = 2,		/* general player info not elsewhere */
        SP_KILLS = 3, 		/* # kills a player has */
        SP_PLAYER = 4,		/* x,y for player */
        SP_TORP_INFO = 5,		/* torp status */
        SP_TORP = 6,		/* torp location */
        SP_PHASER = 7,		/* phaser status and direction */
        SP_PLASMA_INFO = 8,		/* player login information */
        SP_PLASMA = 9,		/* like SP_TORP */
        SP_WARNING = 10,		/* like SP_MESG */
        SP_MOTD = 11,		/* line from .motd screen */
        SP_YOU = 12,		/* info on you? */
        SP_QUEUE = 13,		/* estimated loc in queue? */
        SP_STATUS = 14,		/* galaxy status numbers */
        SP_PLANET = 15,		/* planet armies & facilities */
        SP_PICKOK = 16,		/* your team & ship was accepted */
        SP_LOGIN = 17,		/* login response */
        SP_FLAGS = 18,		/* give flags for a player */
        SP_MASK = 19,		/* tournament mode mask */
        SP_PSTATUS = 20,		/* give status for a player */
        SP_BADVERSION = 21,		/* invalid version number */
        SP_HOSTILE = 22,		/* hostility settings for a player */
        SP_STATS = 23,		/* a player's statistics */
        SP_PL_LOGIN = 24,		/* new player logs in */
        SP_RESERVED = 25,		/* for future use */
        SP_PLANET_LOC = 26,		/* planet name, x, y */

        SP_UDP_REPLY = 28,		/* notify client of UDP status */
        SP_SEQUENCE = 29,		/* sequence # packet */
        SP_SC_SEQUENCE = 30,		/* this trans is semi-critical info */
        SP_RSA_KEY = 31,		/* handles binary verification */
        SP_GENERIC_32 = 32,		/* 32 public byte generic, see public struct */
        SP_FLAGS_ALL = 33,		/* abbreviated flags for all players */

        SP_SHIP_CAP = 39,		/* Handles server ship mods */
        SP_S_REPLY = 40,              /* reply to send-public short request */
        SP_S_MESSAGE = 41,              /* var. Message Packet */
        SP_S_WARNING = 42,              /* Warnings with 4  Bytes */
        SP_S_YOU = 43,              /* hostile,armies,whydead,etc .. */
        SP_S_YOU_SS = 44,              /* your ship status */
        SP_S_PLAYER = 45,              /* variable length player packet */
        SP_PING = 46,              /* ping packet */
        SP_S_TORP = 47,              /* variable length torp packet */
        SP_S_TORP_INFO = 48,              /* SP_S_TORP with TorpInfo */
        SP_S_8_TORP = 49,              /* optimized SP_S_TORP */
        SP_S_PLANET = 50,             /* see SP_PLANET */
        /* S_P2 */

        SP_S_SEQUENCE = 56,	/* SP_SEQUENCE for compressed packets */
        SP_S_PHASER = 57,      /* see public struct */
        SP_S_KILLS = 58,      /* # of kills player have */
        SP_S_STATS = 59,      /* see SP_STATS */
        SP_FEATURE = 60,
        SP_RANK = 61,
#if LTD_STATS
        SP_LTD = 62,      /* LTD stats for character */
#endif

        TOTAL_SPACKETS = SP_LTD + 1 + 1, /* length of packet sizes array */
    }
}
