using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{

    public enum CommunicationRequest : sbyte
    {
        TCP = 0,
        UDP = 1,

        /// <summary>
        /// This request should ONLY come through the UDP connection
        /// </summary>
        Verify = 2,

        Update = 3,
        
        UdpMode = 4,
    }

    public enum Mode : sbyte
    {
        Unspecified = -1,

        CONNMODE_Port = 0,
        CONNMODE_Packet = 1,

        MODE_TCP = 0,
        MODE_SIMPLE = 1,
        MODE_FAT = 2,
        MODE_DOUBLE = 3,	/* put this one last */
    }

    //public enum ConnectionMode : sbyte
    //{
        
    //}

    public enum CommunicationSwitch
    {
        TCP_OK = 0,
        UDP_OK = 1,
        DENIED = 2,
        VERIFY = 3,
    }

    public class NetConstants
    {
        public const int Port = 2592;
        public const int ObserverPort = 2593;
        public const int MetaserverPort = 3521;


        public const int MSG_LEN = 80;
        public const int KEYMAP_LEN = 96;
        public const int NAME_LEN = 16;
        public const int RESERVED_SIZE = 16;
        public const int KEY_SIZE = 32;
        public const int MAXPLAYER = 36;

        public const char LTD_VERSION = 'a'; /* version for SP_LTD packet */

        public const int VPLANET_SIZE = 6;


        #region GENERIC_32


        public const int GENERIC_32_LENGTH = 32;

#if F_sp_generic_32
        public const int COST_GENERIC_32 = GENERIC_32_LENGTH;
#else
        public const int COST_GENERIC_32 = 0;
#endif

        public const int GENERIC_32_VERSION_A = 1;

        public const int GENERIC_32_VERSION_B = 2;
        public const int GENERIC_32_VERSION = 2; //GENERIC_32_VERSION_B /* default */

        #endregion


        public const int SOCKVERSION = 4;
        public const int UDPVERSION = 10;
    }
}
