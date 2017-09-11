using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Kennen
{
    public static class DamageManager
    {
        private static Obj_AI_Hero Player => GameObjects.Player;
        public static double GetUltDamage(this Obj_AI_Hero target, int ticks)
        {
            var tickDamage = Player.GetSpellDamage(target, SpellSlot.R);
            return tickDamage * ticks;
        }
    }
}
