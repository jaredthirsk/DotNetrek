// Adapted from:
// http://james.tooraweenah.com/cgi-bin/darcsweb.cgi?r=netrek-server;a=plainblob;f=/Vanilla/include/packets.h
// Retrieved on 2011.10.18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public static class Protocol
    {
        /* 
         * Include file for socket I/O xtrek.
         *
         * Kevin P. Smith 1/29/89
         */
        //#include "copyright2.h"
        //#include "ltd_stats.h"

        public const string STATUS_TOKEN = "\t@@@";		/* ATM */

        /*
         * TCP and UDP use identical packet formats; the only difference is that,
         * when in UDP mode, all packets sent from server to client have a sequence
         * number appended to them.
         *
         * (note: ALL packets, whether sent on the TCP or UDP channel, will have
         * the sequence number.  Thus it's important that client & server agree on
         * when to switch.  This was done to keep critical and non-critical data
         * in sync.)
         */

        /*

        netrek protocol

          prerequisite knowledge; TCP/IP protocols TCP and UDP.

          briefly, "TCP provides reliable, ordered delivery of a stream of
          bytes from a program on one computer to a program on another
          computer" -- Wikipedia.

          UDP on the other hand, provides unreliable, unordered delivery of
          datagrams, from a program on one computer to a program on another
          computer.


          the netrek protocol in TCP mode is a stream of bytes sent by the
          server to the client, and the client to the server.  the stream
          consists of netrek packets, laid end to end.

          in the artificial example below, the stream consists of three
          packets:

          +-----------------------------------------------+
          | stream                                        |
          +---------------+---------------+---------------+
          | netrek packet | netrek packet | netrek packet |
          +---+-----------+---+-----------+---+-----------+
          | t | data data | t | data data | t | data data |
          +---+-----------+---+-----------+---+-----------+

          the length of each netrek packet varies according to the packet
          type.  the first public byte of the netrek packet identifies the packet
          type.

          in TCP mode, a stream may fragment, and client and server are
          responsible for assembling the fragments; in the artificial example
          below, two stream fragments will have to be reassembled:

          +-----------------------+             +-----------------------+
          | stream fragment       |             | stream fragment       |
          +---------------+-------+             +-------+---------------+
          | netrek packet | netrek               packet | netrek packet |
          +---+-----------+---+---              --------+---+-----------+
          | t | data data | t | da              ta data | t | data data |
          +---+-----------+---+---              --------+---+-----------+

          the netrek protocol in UDP mode is a series of netrek packets.  each
          UDP datagram consists of one or more netrek packets, laid end to end.

          +-----------------------------------------------+
          | datagram payload                              |
          +---------------+---------------+---------------+
          | netrek packet | netrek packet | netrek packet |
          +---+-----------+---+-----------+---+-----------+
          | t | data data | t | data data | t | data data |
          +---+-----------+---+-----------+---+-----------+

          communication begins in TCP mode.  once negotiated, communication
          may proceed in both UDP and TCP mode over separate ports.

          timing

            the server sends netrek packets at a rate consistent with the
            chosen update rate, and the amount and type of game activity.  the
            client processes netrek packets and updates the display as fast as
            it is able.

            the server spends most of the time waiting for the next timer
            event or the next packet from a client.  the client spends most of
            the time waiting for packets or user input.

            the netrek packet stream is full of time gaps enforced by game
            update rate and user input.

          netrek packet format

            the first public byte is a netrek packet type.
            bytes that follow vary and depend on the type.

          packet types

            there is a public const string  constant number for each packet type, you will
            find them later in this file.

            SP_ prefix, packets sent by the server to the client
            CP_ prefix, packets sent by the client to the server

          packet contents

            there is a public struct for each packet type, you will find them below.
            the suffix of the public struct name identifies the origin of the packet.

            *_spacket, packets sent by the server to the client
            *_cpacket, packets sent by the client to the server

            every public struct begins with a public sbyte type, which is the packet type.

            public struct members that represent numbers longer than eight bits are
            to be in network public byte order.

            public struct members that represent character arrays are generally not
            NUL terminated.

          general protocol state outline

            starting state, immediately after TCP connection established, the
            client begins by sending CP_SOCKET:

            CP_SOCKET
            CP_FEATURE, optional, to indicate feature packets are known
            SP_MOTD
            SP_FEATURE, only if CP_FEATURE was seen
            SP_QUEUE, optional, repeats until slot is available
            SP_YOU, indicates slot number assigned

            login state, player slot status is POUTFIT, client shows name and
            password prompt and accepts input, when input is complete,
            CP_LOGIN is sent:

            CP_LOGIN
            CP_FEATURE
            SP_LOGIN
            SP_YOU
            SP_PLAYER_INFO
            various other server packets

            outfit state, player slot status is POUTFIT, client shows team
            selection window.

            SP_MASK, sent regularly during outfit

            when client accepts team selection input from user, client sends
            CP_OUTFIT:

            CP_OUTFIT
            SP_PICKOK, signals server acceptance of alive state

            client identifies itself to server (optional):

            CP_MESSAGE (MINDIV|MCONFIG, self, "@clientname clientversion")

            ship alive state, server places ship in game and play begins,
            various packets are sent, until eventually the ship dies:

            SP_PSTATUS, indicates PDEAD state
            client animates explosion

            SP_PSTATUS, indicates POUTFIT state
            clients returns to team selection window

            when user selects quit function:

            CP_QUIT
            CP_BYE
        */

        /* variable length packets */
        public const int VPLAYER_SIZE = 4;
        public const int SP2SHORTVERSION = 11;      /* S_P2 */
        public const int OLDSHORTVERSION = 10;      /* Short packets version 1 */


        public const int SOCKVERSION = 4;
        public const int UDPVERSION = 10;		/* changing this blocks other versions*/

    }
}

