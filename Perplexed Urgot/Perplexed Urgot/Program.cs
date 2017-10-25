using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using System.Linq;
using System.Drawing;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Urgot
{
    public class Program
    {
        static Obj_AI_Hero Player => GameObjects.Player;
        private static string[] PassiveNames =
        {
            "Urgot_Base_Passive_Ready_Glow.troy", "Urgot_Base_Passive_Ready_Glow_Flipped.troy",

        };
        private static bool CastingW => Player.HasBuff("UrgotW");
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static void GameEventsOnGameStart()
        {
            if (Player.ChampionName != "Urgot")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();

            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += RenderOnOnPresent;
        }

        private static void Game_OnUpdate()
        {
            Killsteal();
            if (MenuManager.Keys["semiUlt"].Enabled)
                SemiUlt();
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    if (CastingW)
                    {
                        if (MenuManager.ComboWSettings["autoFollow"].Enabled)
                            AutoFollow(SpellManager.W.Range, MenuManager.ComboWSettings["followDistance"].Value);
                    }
                    Combo();
                    break;
                case OrbwalkingMode.Mixed:
                    if (CastingW)
                    {
                        if (MenuManager.HarassWSettings["autoFollow"].Enabled)
                            AutoFollow(SpellManager.W.Range, MenuManager.HarassWSettings["followDistance"].Value);
                    }
                    Harass();
                    break;
            }
        }

        private static void RenderOnOnPresent()
        {
            if(MenuManager.Drawing["q"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.Q.Range, 50, Color.White);
            if (MenuManager.Drawing["we"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.W.Range, 50, Color.Gold);
            if (MenuManager.Drawing["r"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.R.Range, 50, Color.Red);
            if (MenuManager.Drawing["passive"].Enabled)
            {
                var legs = GameObjects.Get<GameObject>().Where(x => PassiveNames.Contains(x.Name)).ToList();
                foreach (var passive in legs)
                    Render.Circle(passive.ServerPosition, 50, 50, Color.Green);
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(SpellManager.R.Range);
            if (target == null)
                return;
            if (MenuManager.Combo["e"].Enabled && SpellManager.E.Ready && !CastingW)
            {
                if (target.Distance(Player) < SpellManager.E.Range)
                {
                    SpellManager.E.Cast(target.ServerPosition);
                    return;
                }
            }
            if (MenuManager.Combo["q"].Enabled && SpellManager.Q.Ready && !CastingW)
            {
                if (target.Distance(Player) < SpellManager.Q.Range && !target.HasBuffOfType(BuffType.Knockup))
                {
                    SpellManager.Q.Cast(target.ServerPosition);
                    return;
                }
            }
            if (MenuManager.ComboWSettings["on"].Enabled && SpellManager.W.Ready && !CastingW)
            {
                var legs = GameObjects.Get<GameObject>().Where(x => PassiveNames.Contains(x.Name)).ToList();
                if (target.Distance(Player) <= MenuManager.ComboWSettings["castDistance"].Value && legs.Count >= MenuManager.ComboWSettings["legs"].Value)
                    SpellManager.W.Cast();
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(SpellManager.R.Range);
            if (target == null)
                return;
            if (MenuManager.Harass["q"].Enabled && SpellManager.Q.Ready && !CastingW)
            {
                if (target.Distance(Player) < SpellManager.Q.Range)
                {
                    SpellManager.Q.Cast(target.ServerPosition);
                    return;
                }
            }
            if (MenuManager.HarassWSettings["on"].Enabled && SpellManager.W.Ready && !CastingW)
            {
                var legs = GameObjects.Get<GameObject>().Where(x => PassiveNames.Contains(x.Name)).ToList();
                if (target.Distance(Player) <= MenuManager.HarassWSettings["castDistance"].Value && legs.Count >= MenuManager.HarassWSettings["legs"].Value)
                    SpellManager.W.Cast();
            }
        }

        private static void SemiUlt()
        {
            Orbwalker.Implementation.Move(Game.CursorPos);
            var target = TargetSelector.GetTarget(SpellManager.R.Range);
            if (target == null)
                return;
            var pred = SpellManager.R.GetPrediction(target);
            if (pred.HitChance >= HitChance.Medium)
                SpellManager.R.Cast(pred.CastPosition);
        }

        private static void Killsteal()
        {
            if (MenuManager.Killsteal["r"].Enabled && SpellManager.R.Ready)
            {
                var target = GameObjects.EnemyHeroes.Where(x => x.HealthPercent() < 25).OrderBy(x => x.Distance(Player)).FirstOrDefault();
                if (target == null)
                    return;
                var pred = SpellManager.R.GetPrediction(target);
                if (pred.HitChance >= HitChance.Medium)
                    SpellManager.R.Cast(pred.CastPosition);
            }
        }

        private static void AutoFollow(float targetRange, int followRange)
        {
            var target = TargetSelector.GetTarget(targetRange);
            if (target == null)
                return;
            var legs = GameObjects.Get<GameObject>().Where(x => PassiveNames.Contains(x.Name)).ToList();
            if (legs.Count == 0)
                return;
            var closestLegToEnemy = legs.OrderBy(x => x.Distance(target)).FirstOrDefault();
            if (closestLegToEnemy == null)
                return;
            var diff = closestLegToEnemy.ServerPosition - Player.ServerPosition - 25;
            var endPos = target.ServerPosition - diff;
            var direction = (endPos - target.ServerPosition).To2D().Normalized();
            endPos = target.ServerPosition + direction.To3D() * followRange;
            Orbwalker.Implementation.Move(endPos);
        }
    }
}
