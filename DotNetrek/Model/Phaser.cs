using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LionFire.Valor;
using LionFire.Assets;
using LionFire.Templating;

namespace LionFire.Netrek
{

    public class Phaser : NetrekEntity<UnitState>
    {
        public override ITEntity Template
        {
            get
            {
                return LionFire.Valor.Packs.Nextrek.Vanilla.VanillaUnits.CA_Phaser;
                //return new HAsset<TUnit>(Team + "/Phaser");
            }
        }

        public Phaser(Galaxy galaxy, int netrekId)
            : base(galaxy, netrekId)
        {
        }

        public override bool CanCreateEntity()
        {
            if (Status == PhaserStatus.Free)
            {
                return false;
            }
            return base.CanCreateEntity();
        }

        public sbyte War { get; set; }

        public PhaserStatus Status
        {
            get { return status; }
            set
            {
                if (status == value) return;
                status = value;
                switch (status)
                {
                    case PhaserStatus.Free:
                        if (Entity != null)
                        {
                            if (Entity.IsAlive) { Entity.Die(DieReason.Expired); }
                            Entity = null;
                        }
                        break;
                    case PhaserStatus.Hit:
                    case PhaserStatus.Hit2:
                    case PhaserStatus.Miss:
                        TryCreateEntity(); // REVIEW - may be multiple unnecessary creates
                        break;
                    default:
                        throw new UnreachableCodeException();
                }
                l.Trace(this.ToString() + " " + status);
            }
        }
        private PhaserStatus status = PhaserStatus.Free;

        private static ILogger l = Log.Get();


        public byte Dir { get; set; }

        public int Target { get; set; }
    }
}
