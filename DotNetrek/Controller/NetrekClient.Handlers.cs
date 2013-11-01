using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LionFire.Collections;
using LionFire.Valor.Controllers;
using LionFire.Valor;
using Mogre;
using LionFire.Assets;
using LionFire.Valor.Messenger;
using Sprache;
using System.Threading;
using System.Threading.Tasks;
using DotNetrek;

namespace LionFire.Netrek
{
    public partial class NetrekClient
    {
        private static ILogger allPackets = Log.Get("LionFire.Netrek.AllPackets");
        private static ILogger lKills = Log.Get("LionFire.Netrek.Handlers.Kills");
        private static ILogger lPlayerPositions = Log.Get("LionFire.Netrek.Handlers.PlayerPositions");
        private static ILogger lPlanetPositions = Log.Get("LionFire.Netrek.Handlers.PlanetPositions");

        private int PingsSent = 0;
        private int PingsReceived = 0;
        bool sentFeatures = false;

        #region Initialization

        private void InitHandlers()
        {
            motdTimer = new System.Timers.Timer()
            {
                Interval = 500,
                AutoReset = false,
            };
            motdTimer.Elapsed += new System.Timers.ElapsedEventHandler(RaiseMotdChanged);

            PingsSent = 0;
            PingsReceived = 0;
            sentFeatures = false;
        }

        #endregion

        #region Dispatch


        public void DeserializeAndDispatch(byte[] buffer, int bufferSize, int messageId)
        {
            ServerMessageType messageType = (ServerMessageType)messageId;

            allPackets.Trace("[IN] " + messageType.ToString());

            #region Deserialize and handle based on messageType

            {
                switch (messageType)
                {
                    case ServerMessageType.SP_MESSAGE:
                        { var msg = MarshalSerialization.Deserialize<mesg_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PLAYER_INFO:
                        { var msg = MarshalSerialization.Deserialize<plyr_info_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_KILLS:
                        { var msg = MarshalSerialization.Deserialize<kills_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PLAYER:
                        { var msg = MarshalSerialization.Deserialize<player_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_TORP_INFO:
                        { var msg = MarshalSerialization.Deserialize<torp_info_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_TORP:
                        { var msg = MarshalSerialization.Deserialize<torp_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PHASER:
                        { var msg = MarshalSerialization.Deserialize<phaser_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PLASMA_INFO:
                        { var msg = MarshalSerialization.Deserialize<plasma_info_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PLASMA:
                        { var msg = MarshalSerialization.Deserialize<plasma_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_WARNING:
                        { var msg = MarshalSerialization.Deserialize<warning_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_MOTD:
                        { var msg = MarshalSerialization.Deserialize<motd_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_YOU:
                        { var msg = MarshalSerialization.Deserialize<you_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_QUEUE:
                        { var msg = MarshalSerialization.Deserialize<queue_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_STATUS:
                        { var msg = MarshalSerialization.Deserialize<status_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PLANET:
                        { var msg = MarshalSerialization.Deserialize<planet_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PICKOK:
                        { var msg = MarshalSerialization.Deserialize<pickok_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_LOGIN:
                        { var msg = MarshalSerialization.Deserialize<login_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_FLAGS:
                        { var msg = MarshalSerialization.Deserialize<flags_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_MASK:
                        { var msg = MarshalSerialization.Deserialize<mask_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PSTATUS:
                        { var msg = MarshalSerialization.Deserialize<pstatus_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_BADVERSION:
                        { var msg = MarshalSerialization.Deserialize<badversion_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_HOSTILE:
                        { var msg = MarshalSerialization.Deserialize<hostile_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_STATS:
                        { var msg = MarshalSerialization.Deserialize<stats_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PL_LOGIN:
                        { var msg = MarshalSerialization.Deserialize<plyr_login_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_RESERVED:
                        { var msg = MarshalSerialization.Deserialize<reserved_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PLANET_LOC:
                        { var msg = MarshalSerialization.Deserialize<planet_loc_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_UDP_REPLY:
                        { var msg = MarshalSerialization.Deserialize<udp_reply_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_SEQUENCE:
                        { var msg = MarshalSerialization.Deserialize<sequence_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_SC_SEQUENCE:
                        { var msg = MarshalSerialization.Deserialize<sc_sequence_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_RSA_KEY:
                        { var msg = MarshalSerialization.Deserialize<rsa_key_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_GENERIC_32:
                        { var msg = MarshalSerialization.Deserialize<generic_32_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_FLAGS_ALL:
                        { var msg = MarshalSerialization.Deserialize<flags_all_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_SHIP_CAP:
                        { var msg = MarshalSerialization.Deserialize<ship_cap_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_REPLY:
                        { var msg = MarshalSerialization.Deserialize<shortreply_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_MESSAGE:
                        { var msg = MarshalSerialization.Deserialize<mesg_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_WARNING:
                        { var msg = MarshalSerialization.Deserialize<warning_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_YOU:
                        { var msg = MarshalSerialization.Deserialize<you_short_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_YOU_SS:
                        { var msg = MarshalSerialization.Deserialize<youss_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_PLAYER:
                        { var msg = MarshalSerialization.Deserialize<player_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_PING:
                        { var msg = MarshalSerialization.Deserialize<ping_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_TORP:
                        { var msg = MarshalSerialization.Deserialize<torp_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_TORP_INFO:
                        { var msg = MarshalSerialization.Deserialize<torp_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; } // !
                    case ServerMessageType.SP_S_8_TORP:
                        { var msg = MarshalSerialization.Deserialize<torp_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; } // !
                    case ServerMessageType.SP_S_PLANET:
                        { var msg = MarshalSerialization.Deserialize<planet_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_SEQUENCE:
                        { var msg = MarshalSerialization.Deserialize<sequence_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; } // !
                    case ServerMessageType.SP_S_PHASER:
                        { var msg = MarshalSerialization.Deserialize<phaser_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_KILLS:
                        { var msg = MarshalSerialization.Deserialize<kills_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_S_STATS:
                        { var msg = MarshalSerialization.Deserialize<stats_s_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_FEATURE:
                        { var msg = MarshalSerialization.Deserialize<feature_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_RANK:
                        { var msg = MarshalSerialization.Deserialize<rank_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.SP_LTD:
                        { var msg = MarshalSerialization.Deserialize<ltd_spacket>(buffer, bufferSize); ClientExecutor.TickDispatcher.Enqueue(() => Handle(msg)); break; }
                    case ServerMessageType.TOTAL_SPACKETS:
                    default:
                        try
                        {
                            l.Warn("[NET] Unknown message type: " + messageType.ToString());
                        }
                        catch
                        {
                            l.Warn("[NET] Unknown message id: " + messageId);
                        }
                        break;
                }
            }


            #endregion

        }

        #endregion

        #region Message Handlers

        private void Handle(mesg_spacket msg)
        {
            lMessage.Debug(msg.ToString());

            if (!msg.Flags.HasFlag(MessageFlags.MVALID))
            {
                l.Warn("UNEXPECTED: Got message without MVALID bit set");
                return;
            }

            Message message = new Message()
            {
                Text = msg.Message,
            };

            if (msg.Flags.HasFlag(MessageFlags.MGOD))
            {
                Player player = Galaxy.GetPlayer(msg.from);
                message.Source = "GOD";
            }
            else
            {
                if (msg.from == 255)
                {
                    message.Source = "GOD";
                }
                else
                {
                    Player player = Galaxy.GetPlayer(msg.from);
                    message.Source = player.SlotName;
                }
            }


            if (msg.Flags.HasFlag(MessageFlags.MINDIV))
            {
                Player player = Galaxy.GetPlayer(msg.recipient);
                if (player != null)
                {
                    message.Recipient = player.SlotNameAndName;
                }
            }
            else if (msg.Flags.HasFlag(MessageFlags.MTEAM))
            {
                NetrekTeam team = (NetrekTeam)msg.recipient;
                if (Enum.IsDefined(typeof(NetrekTeam), team))
                {
                    message.Recipient = team.ToShortName();
                }
                else
                {
                    l.Warn("!Enum.IsDefined(team)");
                }

                //Player player = Galaxy.(msg.recipient);
                //if (player != null)
                //{
                //    message.Recipient = player.SlotNameAndName;
                //}
            }
            else if (msg.Flags.HasFlag(MessageFlags.MALL))
            {
                message.Recipient = "ALL";
            }

            Bridge.MessengerServer.Send(message);
        }

        #region MOTD

        System.Timers.Timer motdTimer;

        private void Handle(motd_spacket msg)
        {
            //l.Trace(msg.ToString());

            motdSb.Append(msg.line);
            motdSb.Append(Environment.NewLine);

            motdTimer.Enabled = true;
        }

        private void RaiseMotdChanged(object sender, System.Timers.ElapsedEventArgs e)
        {
            var ev = MotdChanged;
            if (ev != null) ev();
        }

        #endregion

        private void Handle(queue_spacket msg)
        {
            l.Debug(msg.ToString());
        }

        #region Players

        private void Handle(player_spacket msg)
        {
            lPlayerPositions.Debug(msg.ToString());

            Player player = galaxy.GetPlayer(msg.pnum);
            player.Speed = msg.speed;
            player.SetLocationOrientation(msg.x, msg.y, msg.dir);
        }

        private void Handle(plyr_info_spacket msg)
        {
            l.Trace(msg.ToString());
            Player player = galaxy.GetPlayer(msg.pnum);
            player.TeamId = msg.team;
            player.ShipTypeId = msg.shiptype;

            if (player.NetrekId == LockedPlayerId)
            {
                UpdateLockedEntity();
            }
        }

        private void Handle(plyr_login_spacket msg)
        {
            l.Trace(msg.ToString());

            Player player = galaxy.GetPlayer(msg.pnum);
            player.Name = msg.name;
            player.Login = msg.login;
            player.Rank = msg.rank;
            player.Monitor = msg.monitor;
        }

        private void Handle(pstatus_spacket msg)
        {
            lPlayerStatus.Trace(msg.ToString());
            Player player = galaxy.GetPlayer(msg.pnum);
            player.Status = (PlayerStatus)msg.status;

            if (player.Status == PlayerStatus.Alive && player.NetrekId == LockedPlayerId)
            {
                UpdateLockedEntity();
            }

            if (player.NetrekId == MyPlayerNumber)
            {
                if (!player.Status.HasFlag(PlayerStatus.Alive))
                {
                    ViewModel.LPilotedUnit = null;
                    //ViewModel.InterfaceController.MainInterfacePort.
                }
            }
        }

        private void Handle(flags_spacket msg)
        {
            if (msg.tractor != 0)
            {
                l.Debug(msg.ToString() + " (TODO: Tractor)");
            }
            Player player = galaxy.GetPlayer(msg.pnum);
            player.Tractor = msg.tractor;
            player.Flags = (PlayerFlags)msg.flags;
        }

        private void Handle(kills_spacket msg)
        {
            lKills.Debug(msg.ToString());
            Player player = galaxy.GetPlayer(msg.pnum);
            player.Kills = msg.Kills;
        }

        #endregion

        #region Torpedos

        private void Handle(torp_spacket msg)
        {
            Torpedo torp = galaxy.GetTorpedo(msg.tnum);
            torp.SetLocationOrientation(msg.x, msg.y, msg.dir);

            torp.TryCreateEntity();

            lTorp.Trace(msg.ToString());
        }

        private void Handle(torp_info_spacket msg)
        {
            Torpedo torp = galaxy.GetTorpedo(msg.tnum);
            torp.War = msg.war;
            torp.Status = (TorpStatus)msg.status;
            torp.Team = NetrekTeam.Independent; // TEMP HACK
            if (msg.Status != TorpStatus.Free)
            {
                lTorp.Trace(msg.ToString());
            }
            torp.TryCreateEntity();
        }

        private void Handle(torp_s_spacket msg)
        {
            l.Warn("Not supported: torp_s_spacket");

            //switch (msg.type)
            //{
            //                case ServerMessageType.SP_S_TORP:
            //    case ServerMessageType.SP_S_TORP_INFO:
            //    case ServerMessageType.SP_S_8_TORP:
            //}
            //Torpedo torp = galaxy.GetTorpedo(msg.);
            //torp.Bitset = msg.bitset;
            //torp.War = msg.whichtorps
        }

        #endregion

        #region Plasma

        private void Handle(plasma_info_spacket msg)
        {
            if (msg.Status != PlasmaStatus.Free)
            {
                l.Debug("Unimplemented: " + msg.ToString());
            }
        }

        #endregion

        #region Phasers

        private void Handle(phaser_spacket msg)
        {
            Phaser phaser = galaxy.GetPhaser(msg.pnum);
            phaser.Dir = msg.dir;
            phaser.Status = (PhaserStatus)msg.status;
            phaser.Target = msg.target;
            phaser.SetLocationOrientation(msg.x, msg.y, msg.dir);

            if (msg.Status != PhaserStatus.Free)
            {
                l.Debug("Unimplemented: " + msg.ToString());
            }
        }

        #endregion

        #region Planets

        private void Handle(planet_loc_spacket msg)
        {
            lPlanetPositions.Trace(msg.ToString());

            Planet planet = galaxy.GetPlanet(msg.pnum);
            planet.Name = msg.name;

            planet.SetLocationOrientation(msg.x, msg.y, 0);
            planet.TryCreateEntity(); // REVIEW
        }

        private void Handle(planet_spacket msg)
        {
            lPlanet.Trace(msg.ToString());

            Planet planet = galaxy.GetPlanet(msg.pnum);
            planet.TeamId = msg.owner;
            planet.Info = msg.info;
            planet.Armies = msg.armies;
            planet.Flags = (PlanetFlags)msg.flags;

            planet.TryCreateEntity(); // REVIEW
        }

        #endregion

        #region Pings

        int lastPingReceived = 0;
        //HashSet<byte> futurePings // FUTURE?  Packet loss count may be invalid (too high) until we detect older pings.

        private void Handle(ping_spacket msg)
        {
            if (msg.number <= lastPingReceived)
            {
                l.Debug("Duplicate (or out of order) ping received: " + msg.number);
                return;
            }

            if (msg.number != lastPingReceived + 1)
            {
                l.Trace("Missed a ping?  lastPingReceived:" + lastPingReceived + ", ping from server: " + msg.number);
            }

            lastPingReceived = msg.number;
            if (lastPingReceived == 255)
            {
                l.Warn("Wrapping lastPingReceived around.  VERIFY");
                lastPingReceived = -1;
            }

            PingsReceived++;
            //l.Trace("Sending ping in response to server ping " + msg.number);
            SendPing(msg.number, 1);
        }

        private void SendPing(byte number, sbyte pingme)
        {
            var response = new ping_cpacket(number, PingsSent, PingsReceived, pingme);
            net.SendTcp(response);
            PingsSent++;

            lPing.Debug("Sent ping " + number);
        }

        #endregion

        private void Handle(hostile_spacket msg)
        {
            lHostile.Trace(msg.ToString());

            Player player = galaxy.GetPlayer(msg.pnum);
            player.Hostile = msg.hostile;
            player.War = msg.war;
        }

        private void Handle(feature_spacket msg)
        {
            l.Trace(msg.ToString());

            if (!sentFeatures)
            {
                l.Trace("Sending features... ");
                sentFeatures = true;
                SendFeatures();
            }
        }

        private void Handle(mask_spacket msg)
        {
            var mask = msg.Mask;

            l.Debug(msg.ToString());

            if (Bridge != null)
            {
                Bridge.SetJoinableTeams(mask);
                //l.Debug("Set mask -- got bridge!");
            }
            else
            {
                SpinWait.SpinUntil(() =>
                    {
                        if (Bridge != null)
                        {
                            //l.Debug("Set mask -- got bridge!");
                            Bridge.SetJoinableTeams(mask);
                            return true;
                        }
                        else
                        {
                            l.Trace("Set mask -- no bridge yet");
                            return false;
                        }
                    });
            }
        }

        #region You

        private void Handle(you_spacket msg)
        {
            MyPlayerNumber = msg.pnum;

            Player player = galaxy.GetPlayer(msg.pnum);
            player.Flags = msg.Flags;
            player.ETemp = msg.etemp;
            player.WTemp = msg.wtemp;
            player.Shield = msg.shield;
            player.Damage = msg.damage;
            player.Armies = msg.armies;
            player.Fuel = msg.fuel;
            player.Hostile = msg.hostile;
            player.Tractor = msg.tractor;
            player.War = msg.war;
            player.WhoDead = msg.whodead;
            player.WhyDead = msg.whydead;

            lYou.Trace(msg.ToString());

        }

        private void Handle(you_short_spacket msg)
        {
            l.Debug("UNHANDLED" + msg.ToString());
        }

        private void Handle(youss_spacket msg)
        {
            l.Debug("UNHANDLED" + msg.ToString());
        }


        #region MOVE

        sbyte LockedPlanetId = -1;
        sbyte LockedPlayerId = -1;
        public bool IsPlayerLocked { get { return LockedPlayerId != -1; } }

        private void UpdateLockedEntity()
        {
            if (IsPlayerLocked)
            {
                try
                {
                    var player = Galaxy.GetPlayer(LockedPlayerId);
                    if (player != null && player.Unit != null)
                    {
                        ViewModel.LPilotedUnit = player.Unit;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    l.Error(ex.ToString());
                }

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(3000);
                    LockOnAnyPlayer(); // TEMP
                });
            }
        }

        private bool LockOnAnyPlayer()
        {
            for (sbyte i = 0; i < 16; i++)
            {
                try
                {
                    Player player = Galaxy.GetPlayer(i);
                    if (player.Team == MyTeam && player.Status == PlayerStatus.Alive)
                    {
                        TryPlayerLock(i);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    l.Warn("LockOnAnyPlayer(): " + ex.ToString());
                    continue;
                }
            }
            l.Warn("LockOnAnyPlayer(): failed to lock on");
            return false;
        }

        public void TryPlanetLock(sbyte planetNumber)
        {
            LockedPlanetId = planetNumber;
            LockedPlayerId = -1;
            {
                l.Trace("TEMP - trying to lock on planet");
                var lockMsg = new planlock_cpacket(planetNumber);
                net.SendTcp(lockMsg);
            }

            //var planet = Galaxy.GetPlanet(0);
            //if (planet != null && planet.Entity != null)
            //{
            //    ViewModel.LPilotedUnit = planet.Entity;
            //}
        }

        public void TryPlayerLock(sbyte playerNumber)
        {
            LockedPlanetId = -1;
            LockedPlayerId = playerNumber;

            {
                l.Trace("TEMP - trying to lock on player");
                var lockMsg = new playlock_cpacket(playerNumber);
                net.SendTcp(lockMsg);
            }

            UpdateLockedEntity();


        }

        #endregion

        private void Handle(pickok_spacket msg)
        {
            l.Trace(msg.ToString());

            if (this.IsObserver)
            {
                LockOnAnyPlayer();  // TEMP TEST HACK 
            }
        }

        private void Handle(status_spacket msg)
        {
            l.Trace(msg.ToString());
        }
        #endregion

        #region Warning

        private void Handle(warning_spacket msg)
        {
            l.Info(msg.ToString());
        }

        private void Handle(warning_s_spacket msg)
        {
            l.Warn(msg.ToString());
        }

        #endregion

        #region Misc

        private void Handle(reserved_spacket msg)
        {
            //try
            //{
            //    l.Trace(msg.ToString());
            //}
            //catch
            //{
            //    l.Trace(">SP_RESERVED< (non-displayable data)");
            //}
        }

        #endregion

        /// <summary>
        /// Catch-all
        /// </summary>
        /// <param name="msg"></param>
        private void Handle(object msg)
        {
            l.Info("Unhandled: " + msg.ToString());
        }

        #region Login

        private void Handle(login_spacket msg)
        {
            //l.Debug(msg.ToString());

            if (msg.accept == 1)
            {
                IsLoggedIn = true;
                STFlags Flags = (STFlags)msg.flags;
                l.Info("Logged in.  Flags: " + Flags + "  Keymap: " + msg.keymap.Clean());
            }
            else
            {
                IsLoggedIn = false;
                l.Warn("Login rejected");
            }

            SendPing(number: 0, pingme: 1);

            net.IsUdpDesired = true;

        }

        #endregion

        #region Netcode

        #region UDP



        private void Handle(udp_reply_spacket msg)
        {
            l.Info(msg.ToString());

            switch (msg.Reply)
            {
                case CommunicationSwitch.TCP_OK:
                    break;
                case CommunicationSwitch.UDP_OK:
                    net.UdpSocketRemotePort = msg.port;
                    net.IsUdpActive = true;
                    net.RequestFullUpdateUdp();
                    break;
                case CommunicationSwitch.DENIED:
                    l.Warn("CommunicationSwitch.DENIED");
                    break;
                case CommunicationSwitch.VERIFY:


                    break;
                default:
                    break;
            }

        }

        #endregion

        private void Handle(badversion_spacket msg)
        {
            l.Warn(msg.ToString());
        }

        #endregion

        #endregion

    }
}
