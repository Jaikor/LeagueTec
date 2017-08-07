using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Damage;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Net;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Camille
{
    public static class Program
    {
        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        private static int LastECast;
        private static bool ShouldW => Game.TickCount - LastECast >= 2000;
        private static bool OnWall => Player.HasBuff("camilleedashtoggle");
        private static bool Ulting => Player.HasBuff("camillerrange");
        private static bool ChargingQ => Player.HasBuff("camilleqprimingstart");
        private static bool QCharged => Player.HasBuff("camilleqprimingcomplete");
        private static bool CastingW => Player.HasBuff("camillewconeslashcharge");
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static void GameEventsOnGameStart()
        {
            if (Player.ChampionName != "Camille")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();

            LastECast = Game.TickCount;

            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += RenderOnOnPresent;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Orbwalker.Implementation.PostAttack += ImplementationOnPostAttack;
            //BuffManager.OnAddBuff += (sender, buff) => Console.WriteLine($"Got {buff.Name}");
            //BuffManager.OnRemoveBuff += (sender, buff) => Console.WriteLine($"Lost {buff.Name}");
        }

        private static void ImplementationOnPostAttack(object o, PostAttackEventArgs e)
        {
            if (!(e.Target is Obj_AI_Hero target))
                return;
            if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
            {
                if (SpellManager.Q.Ready && target.InAutoRange() && !ChargingQ && MenuManager.ComboQ["on"].Enabled)
                    SpellManager.Q.Cast();
            }
            else if (Orbwalker.Implementation.Mode == OrbwalkingMode.Mixed)
            {
                if (SpellManager.Q.Ready && target.InAutoRange() && !ChargingQ && MenuManager.HarassQ["on"].Enabled)
                    SpellManager.Q.Cast();
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMe && e.SpellSlot == SpellSlot.E)
                LastECast = Game.TickCount;
        }

        private static void RenderOnOnPresent()
        {
            if(MenuManager.Drawing["drawW"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.W.Range, 30, Color.White);
            if(MenuManager.Drawing["drawE"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.E.Range, 30, Color.BlueViolet);
        }

        private static void Game_OnUpdate()
        {
            if (MenuManager.Flee["fleeKey"].Enabled)
                Flee();
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
            var target = TargetSelector.GetTarget(SpellManager.E.Range);
            if (target == null)
                return;
            if (SpellManager.E.Ready && !Ulting && !ChargingQ && !QCharged && MenuManager.Combo["comboE"].Enabled)
            {
                if (!OnWall)
                {
                    var walls = Player.ServerPosition.RotateAround(SpellManager.E.Range, 90).GetWalls();
                    var bestWall = walls.Where(x => x.Distance(target) <= SpellManager.E.Range).OrderBy(x => x.Distance(target)).FirstOrDefault();
                    if (bestWall == Vector3.Zero)
                        return;
                    SpellManager.E.Cast(bestWall);
                }
                else
                    SpellManager.E.Cast(target.ServerPosition);
                return;
            }
            if (SpellManager.Q.Ready && target.InAutoRange() && !ChargingQ && (MenuManager.ComboQ["on"].Enabled && !MenuManager.ComboQ["onlyReset"].Enabled))
            {
                SpellManager.Q.Cast();
                return;
            }
            if (SpellManager.W.Ready && target.Distance(Player) <= SpellManager.W.Range && !ChargingQ && !QCharged && ShouldW && MenuManager.Combo["comboW"].Enabled)
                SpellManager.W.Cast(target.ServerPosition);
        }

        private static void Harass()
        {
            var minManaPct = MenuManager.Harass["harassManaPct"].Value;
            var target = TargetSelector.GetTarget(SpellManager.E.Range);
            if (target == null)
                return;
            if (SpellManager.Q.Ready && target.InAutoRange() && !ChargingQ && (MenuManager.HarassQ["on"].Enabled && !MenuManager.HarassQ["onlyReset"].Enabled) && Player.ManaPercent() >= minManaPct)
            {
                SpellManager.Q.Cast();
                return;
            }
            if (SpellManager.W.Ready && target.Distance(Player) <= SpellManager.W.Range && !ChargingQ && !QCharged && ShouldW && MenuManager.Harass["harassW"].Enabled && Player.ManaPercent() >= minManaPct)
                SpellManager.W.Cast(target.ServerPosition);
        }
        private static void Flee()
        {
            var fleePos = Game.CursorPos;
            Orbwalker.Implementation.Move(fleePos);
            if (SpellManager.E.Ready)
            {
                if (!OnWall)
                {
                    var walls = Player.ServerPosition.RotateAround(SpellManager.E.Range, 90).GetWalls();
                    var bestWall = walls.OrderBy(x => x.Distance(fleePos)).ThenByDescending(x => x.Distance(Player)).FirstOrDefault();
                    if (bestWall != Vector3.Zero && SpellManager.E.Ready)
                        SpellManager.E.Cast(bestWall);
                }
                else
                    SpellManager.E.Cast(fleePos);
            }
        }
    }
}
