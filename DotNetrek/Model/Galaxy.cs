using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LionFire.Collections;
using LionFire.Valor.Controllers;
using System.Threading;

namespace LionFire.Netrek
{
    public class Galaxy
    {
        public NetrekClient Client { get; private set; }
        internal EntityController EntityController { get { return this.Client.EntityController; } }

        public Galaxy(NetrekClient client)
        {
            this.Client = client;


        }

        #region Planets

        public MultiBindableDictionary<int, Planet> Planets = new MultiBindableDictionary<int, Planet>();

        public Planet GetPlanet(int planetId)
        {
            //if (id > Constants.MAXPLAYER) return null;
            return Planets.GetOrAddDefault(planetId, () => new Planet(this, planetId));
        }

        public Planet GetHomePlanetForTeam(int teamId)
        {
            return Planets.Values.Where(p => p.TeamId == teamId && p.Flags.HasFlag(PlanetFlags.Home)).FirstOrDefault();
        }
        public IEnumerable<Planet> GetHomePlanetsForTeam(int teamId)
        {
            return Planets.Values.Where(p => p.TeamId == teamId && p.Flags.HasFlag(PlanetFlags.Home));
        }

        #endregion

        #region Players

        protected MultiBindableDictionary<int,Player> Players = new MultiBindableDictionary<int, Player>();

        public Player GetPlayer(int playerId)
        {
            //if (playerId > NetConstants.MAXPLAYER) return null;
            return Players.GetOrAddDefault(playerId, () => 
                {
                    l.Debug("Creating Player " + playerId + " from thread " + Thread.CurrentThread.ManagedThreadId);
                    return new Player(this, playerId);
                });
        }

        #endregion

        #region Torpedos

        protected MultiBindableDictionary<short, Torpedo> Torpedos = new MultiBindableDictionary<short, Torpedo>();

        internal Torpedo GetTorpedo(short torpedoNumber)
        {
            return Torpedos.GetOrAddDefault(torpedoNumber, () => new Torpedo(this, torpedoNumber));
        }

        #endregion

        #region Phasers

        protected MultiBindableDictionary<short, Phaser> Phasers = new MultiBindableDictionary<short, Phaser>();

        internal Phaser GetPhaser(short phaserNumber)
        {
            return Phasers.GetOrAddDefault(phaserNumber, () => new Phaser(this, phaserNumber));
        }

        #endregion


        private static ILogger l = Log.Get();
		
    }

    //public class NetrekObjectManager<T>
    // where T : class, new()
    //{
    //    public readonly int BaseId;
    //    public readonly int MaxId;

    //    public NetrekObjectManager(int baseId, int maxId)
    //    {
    //        this.BaseId = baseId;
    //        this.MaxId = maxId;
    //    }

    //    public T this[int id]
    //    {
    //        get
    //        {

    //        }
    //    }

    //    public Func<int, T> CreateMethod;

    //    private T DefaultCreate(int id)
    //    {
    //        var obj = new T();
    //        return obj;
    //    }
    //}


}
