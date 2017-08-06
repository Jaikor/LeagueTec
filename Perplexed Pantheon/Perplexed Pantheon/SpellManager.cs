using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;
namespace Perplexed_Pantheon
{
    public static class SpellManager
    {
        public static Spell Q, W, E;
        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 600);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 600);

            E.SetSkillshot(0.25f, 80, 2000f, false, SkillshotType.Cone);
        }
    }
}
