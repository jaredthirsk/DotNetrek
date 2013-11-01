using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LionFire.Valor;
using LionFire.Assets;
using LionFire.Templating;

namespace LionFire.Netrek
{

    public class Torpedo : NetrekEntity<ProjectileState>
    {
        public override ITEntity Template
        {
            get
            {
                return LionFire.Valor.Packs.Nextrek.Vanilla.VanillaUnits.CA_Torp;
                //return new HAsset<TUnit>(Team + "/Torpedo");
            }
        }

        public Torpedo(Galaxy galaxy, int netrekId)
            : base(galaxy, netrekId)
        {
        }

        public override bool CanCreateEntity()
        {
            if (Status == TorpStatus.Free)
            {
                return false;
            }
            return base.CanCreateEntity();
        }

        public sbyte War { get; set; }

        public TorpStatus Status
        {
            get { return status; }
            set
            {
                if (status == value) return;
                status = value;
                switch (status)
                {
                    case TorpStatus.Free:
                        if (Entity != null)
                        {
                            if (Entity.IsAlive) { Entity.Die(DieReason.Expired); }
                            Entity = null;
                        }
                        break;
                    case TorpStatus.Move:
                        TryCreateEntity(); // REVIEW - may be multiple unnecessary creates
                        break;
                    case TorpStatus.Explode:
                        if (Entity != null)
                        {
                            if (Entity.IsAlive) Entity.Die(DieReason.Detonated);
                            //Entity = null;
                        }
                        break;
                    case TorpStatus.Detonated:
                        if (Entity != null)
                        {
                            if (Entity.IsAlive) Entity.Die(DieReason.Abandoned);
                            //Entity = null;
                        }
                        break;
                    case TorpStatus.TOFF:
                        if (Entity != null)
                        {
                            if (Entity.IsAlive) Entity.Die(DieReason.Abandoned);
                            Entity = null;
                        }
                        l.Warn("Not implemented: TOFF torpedoes");
                        break;
                    case TorpStatus.TSTRAIGHT:
                        l.Warn("Not implemented: Straight torpedoes");
                        break;
                    default:
                        throw new UnreachableCodeException();
                }
                l.Trace(this.ToString() + " " + status);
            }
        }
        private TorpStatus status = TorpStatus.Free;

        private static ILogger l = Log.Get();

    }
}
