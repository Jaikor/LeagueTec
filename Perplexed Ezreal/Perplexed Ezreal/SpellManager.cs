using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Ezreal
{
    public static class SpellManager
    {
        public static Spell Q, W, R;
        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 1150);
            W = new Spell(SpellSlot.W, 1000);
            R = new Spell(SpellSlot.R, float.MaxValue);

            Q.SetSkillshot(0.25f, 60, 2000, true, SkillshotType.Line);
            W.SetSkillshot(0.25f, 80, 1550, false, SkillshotType.Line);
            R.SetSkillshot(1, 160, 2000, false, SkillshotType.Line);
        }
    }
}
