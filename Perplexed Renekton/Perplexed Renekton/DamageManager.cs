using Aimtec;
using Aimtec.SDK.Damage;

namespace Perplexed_Renekton
{
    public static class DamageManager
    {
        public static double GetQDamage(this Obj_AI_Hero source, Obj_AI_Hero target)
        {
            return source.GetSpellDamage(target, SpellSlot.Q);
        }
        public static double GetWDamage(this Obj_AI_Hero source, Obj_AI_Hero target)
        {
            return source.GetSpellDamage(target, SpellSlot.W);
        }
        public static double GetEDamage(this Obj_AI_Hero source, Obj_AI_Hero target)
        {
            return source.GetSpellDamage(target, SpellSlot.E);
        }
        public static double GetComboDamage(this Obj_AI_Hero source, Obj_AI_Hero target)
        {
            double dmg = 0;
            if (SpellManager.Q.Ready && MenuManager.Combo["q"].Enabled)
                dmg += source.GetQDamage(target);
            if (SpellManager.W.Ready && MenuManager.Combo["w"].Enabled)
                dmg += source.GetWDamage(target);
            if (SpellManager.E.Ready && MenuManager.Combo["e"].Enabled)
                dmg += source.GetEDamage(target);
            if (SpellManager.E.Ready && MenuManager.Combo["r"].Enabled)
            {
                var rLevel = source.SpellBook.GetSpell(SpellSlot.R).Level;
                var estimatedRDamage = (20 * rLevel + source.TotalAbilityDamage * 0.1) * 6;
                dmg += estimatedRDamage;
            }
            var aaDmg = source.GetAutoAttackDamage(target) * 2;
            dmg += aaDmg;
            return dmg;
        }
    }
}
