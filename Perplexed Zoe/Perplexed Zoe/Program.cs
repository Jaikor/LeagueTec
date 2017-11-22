using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Zoe
{
    class Program
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        private static bool QCasted => Player.SpellBook.GetSpell(SpellSlot.Q).Name == "ZoeQRecast";
        private static GameObject QMissile => GameObjects.Get<GameObject>().FirstOrDefault(x => x.Name == "Zoe_Base_Q_Mis_Linger");
        private static GameObject Portal => GameObjects.Get<GameObject>().FirstOrDefault(x => x.Name == "Zoe_Base_R_portal_entrance");

        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static void GameEventsOnGameStart()
        {
            if (Player.ChampionName != "Zoe")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();

            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
        }

        private static void Game_OnUpdate()
        {
            Killsteal();
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass();
                    break;
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range);
            if (Portal != null)
                target = ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(x => x.IsEnemy && !x.IsDead && x.IsValidTarget() && x.Distance(Portal) <= SpellManager.Q.Range);
            if (target != null)
            {
                if (MenuManager.Combo["q"].Enabled && SpellManager.Q.Ready)
                {
                    if (!QCasted)
                    {
                        var bestCastPos = SpellManager.GetBestQCastPosition(target);
                        SpellManager.Q.Cast(bestCastPos);
                    }
                    else
                    {
                        if (QMissile != null)
                        {
                            var qPos = QMissile.ServerPosition;
                            var qPred = SpellManager.Q2.GetPrediction(target, qPos, Player.ServerPosition);
                            if (qPred.HitChance >= HitChance.High)
                                SpellManager.Q.Cast(qPred.CastPosition);
                        }
                    }
                }
                if (MenuManager.Combo["e"].Enabled && SpellManager.E.Ready)
                {
                    SpellManager.E.Cast(target);
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range);
            if (Portal != null)
                target = ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(x => x.IsEnemy && !x.IsDead && x.IsValidTarget() && x.Distance(Portal) <= SpellManager.Q.Range);
            if (target != null)
            {
                if (MenuManager.Combo["q"].Enabled && SpellManager.Q.Ready && Player.ManaPercent() >= MenuManager.Harass["mana"].Value)
                {
                    if (!QCasted)
                    {
                        var bestCastPos = SpellManager.GetBestQCastPosition(target);
                        SpellManager.Q.Cast(bestCastPos);
                    }
                    else
                    {
                        if (QMissile != null)
                        {
                            var qPos = QMissile.ServerPosition;
                            var qPred = SpellManager.Q2.GetPrediction(target, qPos, Player.ServerPosition);
                            if (qPred.HitChance >= HitChance.High)
                                SpellManager.Q.Cast(qPred.CastPosition);
                        }
                    }
                }
                if (MenuManager.Combo["e"].Enabled && SpellManager.E.Ready && Player.ManaPercent() >= MenuManager.Harass["mana"].Value)
                {
                    SpellManager.E.Cast(target);
                }
            }
        }

        private static void Killsteal()
        {
            if (!MenuManager.Killsteal["q"].Enabled)
                return;
            var killables = GameObjects.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && !x.IsDead && Player.GetSpellDamage(x, SpellSlot.Q) > x.Health && x.Distance(Player) <= SpellManager.Q.Range);
            foreach (var target in killables)
            {
                if (SpellManager.Q.GetPrediction(target).HitChance >= HitChance.High && !QCasted)
                    SpellManager.Q.Cast(target);
                else
                {
                    if (!QCasted)
                    {
                        var bestCastPos = SpellManager.GetBestQCastPosition(target);
                        SpellManager.Q.Cast(bestCastPos);
                    }
                    else
                    {
                        if (QMissile != null)
                        {
                            var qPos = QMissile.ServerPosition;
                            var qPred = SpellManager.Q2.GetPrediction(target, qPos, Player.ServerPosition);
                            if (qPred.HitChance >= HitChance.High)
                                SpellManager.Q.Cast(qPred.CastPosition);
                        }
                    }
                }
            }
        }

        private static void Render_OnPresent()
        {
            if(MenuManager.Drawing["q"].Enabled)
                Render.Circle(Player.Position, SpellManager.Q.Range, 100, Color.White);
            if(MenuManager.Drawing["e"].Enabled)
                Render.Circle(Player.Position, SpellManager.E.Range, 100, Color.Blue);
            if (MenuManager.Drawing["r"].Enabled)
                Render.Circle(Player.Position, SpellManager.R.Range, 100, Color.BlueViolet);
        }
    }
}
