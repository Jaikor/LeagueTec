using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Zoe
{
    public static class SpellManager
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        public static Spell Q, Q2, W, E, R;

        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 800f);
            Q2 = new Spell(SpellSlot.Q, 900f);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 800f);
            R = new Spell(SpellSlot.R, 575f);

            Q.SetSkillshot(0.25f, 70f, 1200f, true, SkillshotType.Line);
            Q2.SetSkillshot(0f, 70f, 1800f, true, SkillshotType.Line);
            E.SetSkillshot(0.25f, 40f, 1700f, true, SkillshotType.Line);
        }

        public static Vector3 GetBestQCastPosition(Obj_AI_Base target)
        {
            var points = Player.ServerPosition.RotateAround(Q.Range, 90);
            var unitsInRange = GameObjects.Get<Obj_AI_Base>().Where(x => (x.IsMinion || x.IsHero) && x.IsEnemy && x.IsValidTarget()).ToList();
            var validCastPoints = points.Select(x =>
            {
                return unitsInRange.Any(t =>
                {
                    var rect = new Vector2Geometry.Rectangle((Vector2)Player.ServerPosition, (Vector2)x, Q.Width + t.BoundingRadius);
                    return rect.IsInside((Vector2)t.ServerPosition);
                }) ? Vector3.Zero : x;
            }).Where(x => x != Vector3.Zero);

            var validHitPoints = validCastPoints.Select(x =>
            {
                return unitsInRange.Any(t =>
                {
                    var rect = new Vector2Geometry.Rectangle((Vector2)x, (Vector2)target.ServerPosition, Q.Width + t.BoundingRadius);
                    return rect.IsInside((Vector2)t.ServerPosition);
                }) ? Vector3.Zero : x;
            });

            return validHitPoints.Where(x => x != Vector3.Zero).OrderByDescending(x => x.Distance(target)).FirstOrDefault();
        }
    }
}
