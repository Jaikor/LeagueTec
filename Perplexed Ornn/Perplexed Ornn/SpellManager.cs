using System.Security.AccessControl;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Ornn
{
    public static class SpellManager
    {
        public const int EHitRadius = 350;
        public static Spell Q, W, E, R;
        public static void Intialize()
        {
            Q = new Spell(SpellSlot.Q, 800);
            W = new Spell(SpellSlot.W, 500);
            E = new Spell(SpellSlot.E, 750);
            R = new Spell(SpellSlot.R, 3500);

            Q.SetSkillshot(0.25f, 65, 1800, false, SkillshotType.Line);
            E.SetSkillshot(0.25f, 130, 1600, false, SkillshotType.Line);
            R.SetSkillshot(0.25f, 250, 450, false, SkillshotType.Line);
        }

        public static int GetQDamage()
        {
            var damages = new[] { 20, 50, 80, 110, 140 };
            var player = GameObjects.Player;
            var qLevel = player.SpellBook.GetSpell(SpellSlot.Q).Level;
            var baseDamage = damages[qLevel - 1];
            return (int) (baseDamage + player.TotalAttackDamage);
        }
    }
}
