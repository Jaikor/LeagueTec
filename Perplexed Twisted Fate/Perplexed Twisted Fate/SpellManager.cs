using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Twisted_Fate
{
    public static class SpellManager
    {
        public static Spell Q, W, R;
        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 1450);
            W = new Spell(SpellSlot.W);
            R = new Spell(SpellSlot.R, 5500);

            Q.SetSkillshot(0.25f, 40, 1000, false, SkillshotType.Line);
        }
    }
}
