using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    //    /// <summary>
    //    /// packets sent from remote client to xtrek server
    //    /// </summary>
    //    public static class CMessageTypes
    //    {
    //        /// <summary>
    //        /// send a message
    //        /// </summary>
    //        public const int CP_MESSAGE = 1;

    //        /// <summary>
    //        /// set speed
    //        /// </summary>
    //        public const int CP_SPEED = 2;

    //        /// <summary>
    //        /// change direction
    //        /// </summary>
    //        public const int CP_DIRECTION = 3;

    //        public const int CP_PHASER = 4;		/* phaser in a direction */
    //        public const int CP_PLASMA = 5;		/* plasma (in a direction) */
    //        public const int CP_TORP = 6;		/* fire torp in a direction */
    //        public const int CP_QUIT = 7;		/* self depublic struct */
    //        public const int CP_LOGIN = 8;		/* log in (name, password) */
    //        public const int CP_OUTFIT = 9;		/* outfit to new ship */
    //        public const int CP_WAR = 10;		/* change war status */
    //        public const int CP_PRACTR = 11;		/* create practice robot, transwarp */
    //        public const int CP_SHIELD = 12;		/* raise/lower sheilds */
    //        public const int CP_REPAIR = 13;		/* enter repair mode */
    //        public const int CP_ORBIT = 14;		/* orbit planet/starbase */
    //        public const int CP_PLANLOCK = 15;		/* lock on planet */
    //        public const int CP_PLAYLOCK = 16;		/* lock on player */
    //        public const int CP_BOMB = 17;	/* bomb a planet */
    //        public const int CP_BEAM = 18;		/* beam armies up/down */
    //        public const int CP_CLOAK = 19;		/* cloak on/off */
    //        public const int CP_DET_TORPS = 20;		/* detonate enemy torps */
    //        public const int CP_DET_MYTORP = 21;		/* detonate one of my torps */
    //        public const int CP_COPILOT = 22;		/* toggle copilot mode */
    //        public const int CP_REFIT = 23;		/* refit to different ship type */
    //        public const int CP_TRACTOR = 24;		/* tractor on/off */
    //        public const int CP_REPRESS = 25;		/* pressor on/off */
    //        public const int CP_COUP = 26;		/* coup home planet */
    //        public const int CP_SOCKET = 27;		/* new socket for reconnection */
    //        public const int CP_OPTIONS = 28;		/* send my options to be saved */
    //        public const int CP_BYE = 29;		/* I'm done! */
    //        public const int CP_DOCKPERM = 30;		/* set docking permissions */
    //        public const int CP_UPDATES = 31;		/* set number of usecs per update */
    //        public const int CP_RESETSTATS = 32;		/* reset my stats packet */
    //        public const int CP_RESERVED = 33;		/* for future use */
    //        public const int CP_SCAN = 34;		/* ATM: request for player scan */
    //        public const int CP_UDP_REQ = 35;		/* request UDP on/off */
    //        public const int CP_SEQUENCE = 36;		/* sequence # packet */
    //        public const int CP_RSA_KEY = 37;		/* handles binary verification */
    //        public const int CP_PLANET = 38;		/* cross-check planet info */
    //        public const int CP_PING_RESPONSE = 42;              /* client response */
    //        public const int CP_S_REQ = 43;
    //        public const int CP_S_THRS = 44;
    //        public const int CP_S_MESSAGE = 45;              /* vari. Message Packet */
    //        public const int CP_S_RESERVED = 46;
    //        public const int CP_S_DUMMY = 47;

    //#if BASEPRACTICE || NEWBIESERVER || PRETSERVER
    //        public const int CP_OGGV = 50;
    //#endif

    //        /* 51-54 */

    //        public const int CP_FEATURE = 60;

    //        public const int MAX_CP_PACKETS = 60;
    //    }

    /// <summary>
    /// packets sent from remote client to xtrek server
    /// </summary>
    public enum ClientMessageType : sbyte
    {
        /// <summary>
        /// send a message
        /// </summary>
        CP_MESSAGE = 1,

        /// <summary>
        /// set speed
        /// </summary>
        CP_SPEED = 2,

        /// <summary>
        /// change direction
        /// </summary>
        CP_DIRECTION = 3,

        CP_PHASER = 4,		/* phaser in a direction */
        CP_PLASMA = 5,		/* plasma (in a direction) */
        CP_TORP = 6,		/* fire torp in a direction */
        CP_QUIT = 7,		/* self depublic struct */
        CP_LOGIN = 8,		/* log in (name, password) */
        CP_OUTFIT = 9,		/* outfit to new ship */
        CP_WAR = 10,		/* change war status */
        CP_PRACTR = 11,		/* create practice robot, transwarp */
        CP_SHIELD = 12,		/* raise/lower sheilds */
        CP_REPAIR = 13,		/* enter repair mode */
        CP_ORBIT = 14,		/* orbit planet/starbase */
        CP_PLANLOCK = 15,		/* lock on planet */
        CP_PLAYLOCK = 16,		/* lock on player */
        CP_BOMB = 17,	/* bomb a planet */
        CP_BEAM = 18,		/* beam armies up/down */
        CP_CLOAK = 19,		/* cloak on/off */
        CP_DET_TORPS = 20,		/* detonate enemy torps */
        CP_DET_MYTORP = 21,		/* detonate one of my torps */
        CP_COPILOT = 22,		/* toggle copilot mode */
        CP_REFIT = 23,		/* refit to different ship type */
        CP_TRACTOR = 24,		/* tractor on/off */
        CP_REPRESS = 25,		/* pressor on/off */
        CP_COUP = 26,		/* coup home planet */
        CP_SOCKET = 27,		/* new socket for reconnection */
        CP_OPTIONS = 28,		/* send my options to be saved */
        CP_BYE = 29,		/* I'm done! */
        CP_DOCKPERM = 30,		/* set docking permissions */
        CP_UPDATES = 31,		/* set number of usecs per update */
        CP_RESETSTATS = 32,		/* reset my stats packet */
        CP_RESERVED = 33,		/* for future use */
        CP_SCAN = 34,		/* ATM: request for player scan */
        CP_UDP_REQ = 35,		/* request UDP on/off */
        CP_SEQUENCE = 36,		/* sequence # packet */
        CP_RSA_KEY = 37,		/* handles binary verification */
        CP_PLANET = 38,		/* cross-check planet info */
        CP_PING_RESPONSE = 42,              /* client response */
        CP_S_REQ = 43,
        CP_S_THRS = 44,
        CP_S_MESSAGE = 45,              /* vari. Message Packet */
        CP_S_RESERVED = 46,
        CP_S_DUMMY = 47,

#if BASEPRACTICE || NEWBIESERVER || PRETSERVER
        CP_OGGV = 50,
#endif

        /* 51-54 */

        CP_FEATURE = 60,

        MAX_CP_PACKETS = 60
    }
}
