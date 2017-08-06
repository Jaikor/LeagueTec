using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Pantheon
{
    public static class Utility
    {
        public static Obj_AI_Hero Player = ObjectManager.GetLocalPlayer();
        public static bool CanKillWithQ(Obj_AI_Hero target)
        {
            return target.Health < GetQDamage(target);
        }
        public static double GetQDamage(Obj_AI_Hero target)
        {
            var damage = Player.GetSpellDamage(target, SpellSlot.Q);
            if (IsVulnerable(target))
                damage *= 2;
            return damage;
        }
        public static bool IsVulnerable(Obj_AI_Hero target)
        {
            var maxHealth = target.MaxHealth;
            var vulnThreshold = maxHealth * 0.15;
            return target.Health < vulnThreshold;
        }
        public static List<Obj_AI_Hero> GetVulnerableHeroes()
        {
            return GameObjects.EnemyHeroes.Where(IsVulnerable).ToList();
        }
    }
}
