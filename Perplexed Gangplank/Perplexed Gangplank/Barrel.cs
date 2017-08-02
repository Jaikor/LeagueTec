using Aimtec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perplexed_Gangplank
{
    public class Barrel
    {
        public Obj_AI_Base Object;
        public float Created;
        public float Health => Object.Health;
        public Vector3 Position => Object.Position;
        public Vector3 ServerPosition => Object.ServerPosition;
        public float DecayRate;
        public float TimeAt1HP;
        public float CanQTime => TimeAt1HP - 250f - ((Utility.DistanceFrom(Object) / 2000f) * 1000) + Game.Ping + 10;
        public bool CanQ => Game.TickCount >= CanQTime || Health == 1;
        public float CanChainTime => TimeAt1HP - (250f * 2);
        public bool CanChain => Game.TickCount >= CanChainTime || Health == 1;
        public Barrel(Obj_AI_Base barrel, float created)
        {
            Object = barrel;
            Created = created;
            DecayRate = Utility.GetDecayRate();
            TimeAt1HP = Created + ((1000 * DecayRate) * 2);
        }
        public static implicit operator Barrel(GameObject barrel)
        {
            return BarrelManager.Barrels.First(x => x.ServerPosition == barrel.ServerPosition);
        }
    }
}
