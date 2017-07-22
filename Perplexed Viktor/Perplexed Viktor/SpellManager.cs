using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Viktor
{
    public static class SpellManager
    {
        public static Spell Q, W, E, E2, E3, R;
        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 600);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 525);
            E2 = new Spell(SpellSlot.E, 500);
            E3 = new Spell(SpellSlot.E, E.Range + E2.Range);
            R = new Spell(SpellSlot.R, 700);

            W.SetSkillshot(0.25f, 300, float.MaxValue, false, SkillshotType.Circle);
            E.SetSkillshot(0.25f, 100, 1350, false, SkillshotType.Line, true);
            R.SetSkillshot(0.25f, 450, int.MaxValue, false, SkillshotType.Circle);
        }
    }
}
