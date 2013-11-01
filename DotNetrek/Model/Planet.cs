using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LionFire.Valor;
using LionFire.Assets;
using LionFire.Templating;
using LionFire.Valor.Modules;

namespace LionFire.Netrek
{
    public class Planet : NetrekEntity<UnitState>
    {
        #region Construction

        public Planet(Galaxy galaxy, int netrekId)
            : base(galaxy, netrekId)
        {
        }

        #endregion

        public string Name { get; set; }

        #region Flags

        public PlanetFlags Flags
        {
            get { return flags; }
            set
            {
                if (flags == value) return;
                var oldFlags = flags;
                flags = value;

                l.Debug(this.ToString() + " flags: " + Flags);

                if (Entity == null)
                {
                    TryCreateEntity();
                }

                OnFlagsChanged(oldFlags, flags);
                var ev = FlagsChanged;
                if (ev != null) ev();
            }
        } private PlanetFlags flags;

        public event Action FlagsChanged;

        #endregion

        public IEntity SpawnEntity { get; protected set; }
        public ISpawnPoint SpawnPoint { get; protected set; }

        private bool spawnCreationDeferred;

        private void OnFlagsChanged(PlanetFlags oldFlags, PlanetFlags newFlags)
        {
            if (oldFlags.HasFlag(PlanetFlags.Home) && !newFlags.HasFlag(PlanetFlags.Home))
            {
                l.Warn("UNEXPECTED / Not supported: Home planet flag removed.  TODO: Remove spawner for this planet.");
            }

            if (!oldFlags.HasFlag(PlanetFlags.Home) && newFlags.HasFlag(PlanetFlags.Home))
            {
                CreateSpawnPoint();
            }
        }

        private void CreateSpawnPoint()
        {
            if (this.Name == null || this.Location.Equals(UnsetLocation))
            {
                spawnCreationDeferred = true;
                l.Error("TODO: Follow through with deferred spawn creation. Spawn will not be created");
            }

            this.SpawnEntity = this.Galaxy.EntityController.CreateNow(new PEntity("VanillaSpawner")
            {
                EntityName = Name,
                State = new EntityState
                {
                    TeamId = this.TeamId,
                    Physics = new PhysicsBodyState(this.Location),
                    Module = new ModuleStateNode
                    {
                        Children =  
                        {
                            new ModuleStateNode(typeof(Spawner), new SpawnerState()),
                        },
                    },
                }
            });
            this.SpawnPoint = SpawnEntity.RootModule.ModulesOfType<Spawner>().First();

        }

        public override bool CanCreateEntity()
        {
            if (!base.CanCreateEntity()) return false;
            //if (Flags == PlanetFlags.Unspecified)
            //{
            //    return false;
            //}
            if (Name == null) return false;
            return true;
        }

        protected override void OnEntityCreated(IEntity entity)
        {
            base.OnEntityCreated(entity);
        }

        public override ITEntity Template
        {
            get
            {
                //l.Debug("TEMP - Hardcoded planet type to FluxPillar"); TODO
                return LionFire.Valor.Packs.Nextrek.Vanilla.VanillaUnits.FluxPillar;

                //if(TeamId == sbyte.MinValue) return null;
                //return new HAsset<TUnit>(Team + "/Planets/Planet").Asset;
            }
        }

        //public override uint ValorId
        //{
        //    get { return (uint)NetrekId + 200; }
        //}
        //public uint ValorSpawnId
        //{
        //    get { return (uint)NetrekId + 20000; }
        //}

        private static ILogger l = Log.Get();

        public int Armies { get; set; }

        public sbyte Info { get; set; }

        public ISpawnPoint Spawner { get; protected set; }
    }
}
