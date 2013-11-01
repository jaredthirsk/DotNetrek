using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using LionFire.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace LionFire.Netrek
{

    public class Metaclient
    {
        #region Static Accessor

        public static Metaclient Instance { get { return Singleton<Metaclient>.Instance; } }

        #endregion

        #region (Static) Configuration

        public static string[] DefaultMetaservers = new string[]
        {
            "metaserver.us.netrek.org",
        };

        #endregion

        #region Servers

        public SynchronizedObservableCollection<ServerListing> ServerListings = new SynchronizedObservableCollection<ServerListing>();
        protected MultiBindableDictionary<string, ServerListing> Servers
        {
            get { return servers; }
        } private MultiBindableDictionary<string, ServerListing> servers = new MultiBindableDictionary<string, ServerListing>();

        //public ConcurrentDictionary<string, ServerListing> Servers
        //{
        //    get { return servers; }
        //} private ConcurrentDictionary<string, ServerListing> servers = new ConcurrentDictionary<string, ServerListing>();
        
        #endregion

        public void Query(string[] metaservers = null)
        {
            l.Info("[META] Querying metaservers.");

            if (metaservers == null)
            {
                metaservers = DefaultMetaservers;
            }

            foreach (var metaserver in DefaultMetaservers)
            {
                IPHostEntry host = Dns.GetHostEntry(metaserver);

                if (host == null)
                {
                    l.Error("Host DNS lookup failed for metaserver: " + metaserver);
                    continue;
                }

                Socket socket = null;

                try
                {
                    try
                    {
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                        socket.Connect(host.AddressList, NetConstants.MetaserverPort);
                    }
                    catch (Exception ex)
                    {
                        l.Warn("Failed to connect to: " + host.HostName + ".  Exception: " + ex.ToString());
                        continue;
                    }

                    if (!socket.Connected)
                    {
                        socket.Dispose();
                        continue;
                    }

                    byte[] sendBytes = ASCIIEncoding.ASCII.GetBytes("?version=_");

                    try
                    {
                        socket.Send(sendBytes);
                    }
                    catch (Exception ex)
                    {
                        l.Warn("Error sending to metaserver '" + metaserver + "': " + ex.ToString());
                        continue;
                    }

                    int packetsReceived = 0;

                receive:
                    if (packetsReceived > 1) l.Trace("packetsReceived > 1");

                    byte[] recvBuffer = new byte[8192];
                    int bytesRead = 0;
                    try
                    {
                        if (packetsReceived == 0 || socket.Poll(250, SelectMode.SelectRead))
                        {
                            socket.ReceiveTimeout = 5000; // HARDTIME TIMEOUT
                            bytesRead = socket.Receive(recvBuffer);
                        }
                    }
                    catch (Exception ex)
                    {
                        l.Warn("Error receiving from metaserver: " + ex.ToString());
                        continue;
                    }

                    if (bytesRead == 0)
                    {
                        if (packetsReceived == 0)
                        {
                            l.Warn("Received nothing from metaserver'" + metaserver + "'");
                        }
                        continue;
                    }

                    packetsReceived++;

                    string result = ASCIIEncoding.ASCII.GetString(recvBuffer);

                    string[] lines = result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int lineCount = 0;
                    string protocolType = null;
                    string protocolUnknown2;
                    foreach (var line in lines)
                    {
                        if (line.StartsWith(((char)0).ToString())) break;

                        string[] cells = line.Split(',');

                        if (lineCount++ == 0)
                        {
                            protocolType = cells[0];
                            if (protocolType.Equals("r"))
                            {
                                protocolUnknown2 = cells[1];
                            }
                            continue;
                        }

                        ServerListing listing = null;
                        if (protocolType.Equals("r"))
                        {
                            listing = new ServerListing(cells[0]);
                            listing.Source = protocolType;
                            listing.Port = Convert.ToInt32(cells[1]);
                            listing.Status = cells[2];
                            listing.Age = Convert.ToInt32(cells[3]);
                            listing.Players = Convert.ToInt32(cells[4]);
                            listing.Queue = Convert.ToInt32(cells[5]);
                            listing.Type = cells[6];
                            listing.Comment = String.Empty;

                        }
                        else if (protocolType.Equals("s"))
                        {
                            listing = new ServerListing(cells[0]);
                            listing.Source = protocolType;
                            listing.Type = cells[1];
                            listing.Comment = cells[2];
                            listing.Port = Convert.ToInt32(cells[4]);
                            listing.Players = Convert.ToInt32(cells[5]);
                            listing.Queue = Convert.ToInt32(cells[6].Trim());
                            listing.Status = "2"; // TODO  REVIEW
                            if (listing.Type.Equals("unknown")) listing.Status = "3"; // TODO  REVIEW
                            listing.Age = 0;
                        }
                        else
                        {
                            l.Warn("Unknown metaserver protocol: " + protocolType);
                        }

                        if (listing != null && listing.IsSupported)
                        {
                            l.Debug("[meta] " + listing.ToString());
                            if (ServerListings.Contains(listing))
                            {
                                ServerListings.Remove(listing);
                            }
                            ServerListings.Add(listing);
                            servers.Set(listing.Key, listing);
                        }
                    }
                    goto receive;
                }
                finally
                {
                    try
                    {
                        if (socket != null)
                        {
                            socket.Dispose();
                            socket = null;
                        }
                    }
                    catch (Exception ex) { l.Trace(ex.ToString()); }
                }
            }
        }

        private static ILogger l = Log.Get();

    }
}
