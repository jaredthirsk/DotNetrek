using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public class ServerListing
    {
        public string Key { get { return Address; } }

        public ServerListing(string address, string type = "?", string status = "?", int port = NetConstants.Port, int observerPort = NetConstants.ObserverPort)
        {
            this.Address = address;
            this.Port = port;
            this.ObserverPort = observerPort;
            this.Type = type;
            this.Status = status;
        }
        public string Address { get; set; }
        public int Port { get; set; }
        public int ObserverPort { get; set; }
        public string Type { get; set; }
        public string TypeName
        {
            get
            {
                var result = GameType.GetLongName(Type);
                return result;
            }
        }
        public bool IsSupported
        {
            get
            {
                return GameType.IsSupported(TypeName);
            }
        }

        public string Status { get; set; }
        public int Age { get; set; }


        public int Players { get; set; }
        public int Queue { get; set; }
        public string PlayerStatus
        {
            get
            {
                if (Queue > 0)
                {
                    return "Wait queue: " + Queue;
                }
                else if (Players == 0)
                {
                    return "Empty";
                }
                else
                {
                    return Players + " player" + (Players == 1 ? "" : "s");
                }
            }
        }

        public string Comment { get; set; }
        public string Source { get; set; }

        public override string ToString()
        {
            try
            {
                return Address + ":" + Port + "(:" + ObserverPort + ")-" + Type + ": " + Status;
            }
            catch
            {
                return "(ServerListing)";
            }
        }

        public override bool Equals(object obj)
        {
            ServerListing other = obj as ServerListing;
            if (other == null) return false;
            return this.Key.Equals(other.Key);
        }
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
