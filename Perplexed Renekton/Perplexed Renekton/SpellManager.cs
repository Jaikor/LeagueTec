using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Renekton
{
    public static class SpellManager
    {
        public static Spell Q, W, E, ExtendedE, R;

        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 325);
            W = new Spell(SpellSlot.W, 255);
            E = new Spell(SpellSlot.E, 450);
            ExtendedE = new Spell(SpellSlot.E, E.Range * 2);
            R = new Spell(SpellSlot.R);

            E.SetSkillshot(0, 60, 1000, false, SkillshotType.Line);
        }
    }
}
