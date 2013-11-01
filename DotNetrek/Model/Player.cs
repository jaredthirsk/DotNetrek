using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LionFire.Assets;
using LionFire.Netrek.Packs;
using LionFire.Valor;
using LionFire.Valor.Packs;
using LionFire.Valor.Packs.Nextrek;

namespace LionFire.Netrek
{
    /// <summary>
    /// Note: Player represents the player's ship
    /// </summary>
    public class Player : NetrekEntity<UnitState>
    {

        #region Ontology

        public sbyte PlayerSlot { get { return (sbyte)NetrekId; } }

        public string SlotLetter
        {
            get { return PlayerId.GetPlayerLetter(PlayerSlot); }
        }
        public string SlotName
        {
            get { return this.Team.ToInitial() + SlotLetter; }
        }
        public string SlotNameAndName
        {
            get { return  this.SlotName+ " " + this.Name; }
        }

        #endregion

        #region Valor Bridge

        public IUnit Unit { get { return Entity as IUnit; } }

        public override ITEntity Template
        {
            get
            {
                //return new HAsset<TUnit>(Team + "/Ships/" + ShipType.GetAbbreviation());
                //return new HAsset<TUnit>(TUnit.GetPath("") Team + "/Ships/" + ShipType.GetAbbreviation());

                var result = BroncoPack.AllUnits.Where(u => (u.Object as TNetrekUnit).Name.Equals(this.ShipType.GetAbbreviation())).SingleOrDefault();
                //var result = LionFire.Valor.Packs.Nextrek.Vanilla.VanillaUnits.Units.Units.Where(u => u.Asset.Abbreviation.Equals(this.ShipType.GetAbbreviation())).SingleOrDefault();
                if (result != null) return result.Object as TNetrekUnit;
                else
                {
                    l.Warn("Warning: failed to find unit type for ship: " + ShipType.ToString() + ". Falling back to default.");

                    //return LionFire.Valor.Packs.Nextrek.Vanilla.VanillaUnits.CA; 
                    return BroncoPack.CA;
                }
            }
        }

        #endregion

        #region Construction

        public Player(Galaxy galaxy, int playerId)
            : base(galaxy, playerId)
        {
            ShipTypeId = sbyte.MinValue;
        }

        #endregion

        public bool IsObserver
        {
            //get { return this.NetrekId > 15; }
            get { return this.Name.Contains((char)1); }
        }

        public override bool CanCreateEntity()
        {
            if (IsObserver) return false;

            if (ShipTypeId == sbyte.MinValue)
            {
                return false;
            }
            if (!Status.HasFlag(PlayerStatus.Alive)) return false;
            //if (!Status.HasFlag(PlayerStatus.)) return false;

            return base.CanCreateEntity();
        }

        #region Player Info

        #region Name

        public string Name
        {
            get { return name.Clean(); }
            set { name = value; }
        } private string name;

        #endregion

        public string Login { get; set; }
        public sbyte Rank { get; set; }

        public string Monitor { get; set; }

        #endregion

        #region State

        public ShipType ShipType { get { return (ShipType)ShipTypeId; } }
        public sbyte ShipTypeId { get; set; }
        public sbyte Speed { get; set; }

        #region Flags

        public PlayerFlags Flags
        {
            get { return flags; }
            set
            {
                if (flags == value) return;
                flags = value;
                lFlags.Trace(this.Name + " flags: " + Flags);
            }
        } private PlayerFlags flags;

        #endregion

        public PlayerStatus Status
        {
            get { return status; }
            set
            {
                if (status == value) return;
                var oldStatus = status;
                status = value;
                l.Trace(SlotNameAndName + " status: " + status);

                if (Entity != null)
                {
                    if (status == PlayerStatus.Explode)
                    {
                        Entity.Die(DieReason.Killed); // REVIEW - Make sure it explodes
                    }
                    else if (status == PlayerStatus.Dead)
                    {
                        if (Entity.IsAlive)
                        {
                            Entity.Die(DieReason.Expired); // REVIEW
                        }
                        //this.Galaxy.EntityController.Remove(this.Entity.Id);
                        Entity = null;
                    }
                }
                else
                {
                    TryCreateEntity();
                }
            }
        } private PlayerStatus status;

        public float Kills { get; set; }

        public NetrekTeam HostileTeams { get { return (NetrekTeam)Hostile; } }
        public sbyte Hostile { get; set; }
        public NetrekTeam WarTeams { get { return (NetrekTeam)War; } }
        public sbyte War { get; set; }
        public sbyte Tractor { get; set; }

        public short ETemp { get; set; }
        public short WTemp { get; set; }
        public int Shield { get; set; }
        public int Damage { get; set; }
        public int Fuel { get; set; }
        public sbyte Armies { get; set; }
        public short WhoDead { get; set; }
        public short WhyDead { get; set; }

        #endregion

        private static ILogger lFlags = Log.Get("LionFire.Netrek.Player.Flags");
        private static ILogger l = Log.Get();

    }

}
