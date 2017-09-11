using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Kennen
{
    public class Program
    {
        private static Obj_AI_Hero Player => GameObjects.Player;
        private static bool AutoIsCharged => Player.HasBuff("kennendoublestrikelive");
        private static bool IsRushing => Player.HasBuff("KennenLightningRush");
        private static bool RushToTarget = true;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static void GameEventsOnGameStart()
        {
            if (Player.ChampionName != "Kennen")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();

            Game.OnUpdate += GameOnOnUpdate;
            Orbwalker.Implementation.PostAttack += Implementation_PostAttack;
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;
            Render.OnPresent += Render_OnPresent;
        }

        private static void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMe && e.SpellSlot == SpellSlot.E)
                RushToTarget = true;
        }

        private static void Implementation_PostAttack(object sender, PostAttackEventArgs e)
        {
            if (sender is Obj_AI_Hero hero && e.Target is Obj_AI_Hero target)
            {
                if (hero.IsMe && target.GetPassiveMarks() == 1 && AutoIsCharged && SpellManager.W.Ready)
                    DelayAction.Queue(25, () => SpellManager.W.Cast());
            }
        }

        private static void GameOnOnUpdate()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range);
            if (target == null)
                return;
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
            if (target == null)
                return;
            var targetsInUltRange = GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsValidTarget() && x.Distance(Player) <= SpellManager.R.Range).ToList();
            if (MenuManager.Combo["r"].Children["on"].Enabled)
            {
                if (MenuManager.Combo["r"].Children["rAmount"].Enabled && targetsInUltRange.Count >= MenuManager.Combo["r"].Children["rAmount"].Value)
                {
                    SpellManager.R.Cast();
                    return;
                }
                if (MenuManager.Combo["r"].Children["killable"].Enabled && target.Health < target.GetUltDamage(6) && target.Distance(Player) <= SpellManager.R.Range)
                    SpellManager.R.Cast();
            }
            if (IsRushing)
            {
                if (RushToTarget)
                {
                    Orbwalker.Implementation.Move(target.ServerPosition);
                    if (target.Distance(Player) <= 150)
                        RushToTarget = false;
                }
                else
                    Orbwalker.Implementation.Move(Game.CursorPos);
            }
            if (MenuManager.Combo["q"].Enabled && SpellManager.Q.Ready && target.Distance(Player) < SpellManager.Q.Range)
                SpellManager.Q.Cast(target);
            if (MenuManager.Combo["w"].Enabled && SpellManager.W.Ready && target.Distance(Player) <= SpellManager.W.Range)
            {
                if (MenuManager.Combo["wStun"].Enabled)
                {
                    if (target.GetPassiveMarks() == 2)
                        SpellManager.W.Cast();
                    return;
                }
                if (target.GetPassiveMarks() >= 1)
                    SpellManager.W.Cast();
            }
            if (MenuManager.Combo["e"].Enabled && SpellManager.E.Ready && !IsRushing)
            {
                if (target.Distance(Player) <= MenuManager.Combo["eDistance"].Value)
                    SpellManager.E.Cast();
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range);
            if (target == null)
                return;
            if (IsRushing)
            {
                if (RushToTarget)
                {
                    Orbwalker.Implementation.Move(target.ServerPosition);
                    if (target.Distance(Player) <= 150)
                        RushToTarget = false;
                }
                else
                    Orbwalker.Implementation.Move(Game.CursorPos);
            }
            if (MenuManager.Harass["q"].Enabled && SpellManager.Q.Ready && target.Distance(Player) < SpellManager.Q.Range)
                SpellManager.Q.Cast(target);
            if (MenuManager.Harass["w"].Enabled && SpellManager.W.Ready && target.Distance(Player) <= SpellManager.W.Range)
            {
                if (target.GetPassiveMarks() >= 1)
                    SpellManager.W.Cast();
            }
            if (MenuManager.Harass["e"].Enabled && SpellManager.E.Ready && !IsRushing)
            {
                if (target.Distance(Player) <= MenuManager.Harass["eDistance"].Value)
                    SpellManager.E.Cast();
            }
        }

        private static void Killsteal()
        {
            if (MenuManager.Killsteal["q"].Enabled && SpellManager.Q.Ready)
            {
                var killable = GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsValidTarget() && x.Health < Player.GetSpellDamage(x, SpellSlot.Q) && x.Distance(Player) <= SpellManager.Q.Range);
                foreach (var target in killable)
                    SpellManager.Q.Cast(target);
            }
        }

        private static void Render_OnPresent()
        {
            if (MenuManager.Drawing["q"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.Q.Range, 50, Color.White);
            if (MenuManager.Drawing["w"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.W.Range, 50, Color.Blue);
            if (MenuManager.Drawing["comboE"].Enabled)
                Render.Circle(Player.ServerPosition, MenuManager.Combo["eDistance"].Value, 50, Color.Chocolate);
            if (MenuManager.Drawing["harassE"].Enabled)
                Render.Circle(Player.ServerPosition, MenuManager.Harass["eDistance"].Value, 50, Color.CadetBlue);
            if (MenuManager.Drawing["r"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.R.Range, 50, Color.BlueViolet);

        }
    }
}
