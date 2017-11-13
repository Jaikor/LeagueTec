﻿using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Damage;
using System.Collections.Generic;
using System.Linq;
using Aimtec.SDK.Orbwalking;

namespace Perplexed_Gangplank
{
    public static class Utility
    {
        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        public static float GetDecayRate()
        {
            var level = Player.Level;
            return level >= 13 ? 0.5f : Player.Level >= 7 ? 1f : 2f;
        }
        public static float DistanceFrom(Obj_AI_Base target)
        {
            return Player.Distance(target);
        }
        public static List<Obj_AI_Hero> GetAllEnemiesInRange(float range)
        {
            return ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.IsInRange(range) && x.IsValidTarget()).ToList();
        }
        public static bool CanKillWithQ(Obj_AI_Base target)
        {
            var damage = Player.GetSpellDamage(target, SpellSlot.Q);
            var extraDamage = 0f;
            if (Player.HasItem(ItemId.Sheen)  || Player.HasItem(ItemId.IcebornGauntlet))
                damage += Player.BaseAttackDamage;
            else if (Player.HasItem(ItemId.TrinityForce))
                damage += Player.BaseAttackDamage * 2;
            extraDamage = (float) Player.CalculateDamage(target, DamageType.Physical, extraDamage);
            damage += extraDamage;
            return damage >= target.Health;
        }
        public static double GetRDamage(Obj_AI_Base target, int waves)
        {
            var waveDamage = Player.GetSpellDamage(target, SpellSlot.R);
            var totalDamage = waveDamage * waves;
            return totalDamage;
        }
        public static bool CanKillWithR(Obj_AI_Base target, int waves)
        {
            return GetRDamage(target, waves) > target.Health;
        }
    }
}
