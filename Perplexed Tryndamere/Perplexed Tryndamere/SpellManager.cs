using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Tryndamere
{
    public static class SpellManager
    {
        public static Spell Q, W, E, R;
        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W, 850);
            E = new Spell(SpellSlot.E, 660);
            R = new Spell(SpellSlot.R);

            E.SetSkillshot(0, 160, 550, false, SkillshotType.Line);
        }
    }
}
