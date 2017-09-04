using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Ornn
{
    public class Program
    {
        static Obj_AI_Hero Player => GameObjects.Player;
        static int LastRCast;
        static bool UltIsChargingIn => Player.SpellBook.GetSpell(SpellSlot.R).Name == "OrnnRCharge" || Game.TickCount - LastRCast < 5000;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            if (Player.ChampionName != "Ornn")
                return;

            LastRCast = Game.TickCount - 5000;

            MenuManager.Initialize();
            SpellManager.Intialize();

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;

            Render.OnPresent += RenderOnOnPresent;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMelee && e.SpellSlot == SpellSlot.R && e.SpellData.Name == "OrnnR")
                LastRCast = Game.TickCount;
        }

        private static void Game_OnUpdate()
        {
            Killsteal();
            if (MenuManager.Keys["semiUlt"].Enabled)
            {
                Orbwalker.Implementation.Move(Game.CursorPos);
                SemiUlt();
            }
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
            if (MenuManager.Combo["w"].Enabled && target.Distance(Player) <= SpellManager.W.Range && SpellManager.W.Ready)
            {
                if(!target.HasBuff("OrnnVulnerableDebuff")) //Don't W already brittled targets.
                    SpellManager.W.Cast(target.ServerPosition);
                return;
            }
            if (MenuManager.Combo["q"].Enabled && target.Distance(Player) <= SpellManager.Q.Range && SpellManager.Q.Ready)
            {
                if (MenuManager.Combo["qIfHaveE"].Enabled)
                {
                    if (SpellManager.E.Ready && SpellManager.Q.Ready)
                        SpellManager.Q.Cast(target.ServerPosition);
                }
                else if (SpellManager.Q.Ready)
                    SpellManager.Q.Cast(target.ServerPosition);
                return;
            }
            if (MenuManager.Combo["e"].Enabled && SpellManager.E.Ready)
            {
                var pillar = WallManager.GetNearestPillar(target);
                if (pillar != null && pillar.Distance(Player) <= SpellManager.E.Range)
                    SpellManager.E.Cast(pillar.ServerPosition);
                else
                {
                    var wallsAroundTarget = target.ServerPosition.RotateAround(SpellManager.EHitRadius, 90).GetWalls(target);
                    var bestWall = wallsAroundTarget.Where(x => x.Distance(Player) <= SpellManager.E.Range).OrderBy(x => x.Distance(target)).FirstOrDefault();
                    if (bestWall == Vector3.Zero)
                        return;
                    SpellManager.E.Cast(bestWall);
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range);
            if (target == null)
                return;
            var minManaPct = MenuManager.Harass["harassMana"].Value;
            if (Player.ManaPercent() < minManaPct)
                return;
            if(MenuManager.Harass["q"].Enabled && SpellManager.Q.Ready)
            {
                SpellManager.Q.Cast(target.ServerPosition);
                return;
            }
            if (MenuManager.Harass["w"].Enabled && SpellManager.W.Ready)
            {
                if (!target.HasBuff("OrnnVulnerableDebuff")) //Don't W already brittled targets.
                    SpellManager.W.Cast(target.ServerPosition);
            }
        }

        private static void Killsteal()
        {
            if (!MenuManager.Killsteal["q"].Enabled)
                return;
            if (!SpellManager.Q.Ready)
                return;
            var qDamage = SpellManager.GetQDamage();
            var killable = GameObjects.EnemyHeroes.Where(x => x.Health < Player.CalculateDamage(x, DamageType.Physical, qDamage) && x.Distance(Player) <= SpellManager.Q.Range).OrderBy(x => x.Health).FirstOrDefault();
            if (killable == null)
                return;
            SpellManager.Q.Cast(killable.ServerPosition);
        }

        private static void SemiUlt()
        {
            if (SpellManager.R.Ready)
            {
                var target = TargetSelector.GetTarget(SpellManager.R.Range);
                if (target == null)
                    return;
                if (!UltIsChargingIn)
                {
                    SpellManager.R.Cast(target.ServerPosition);
                    return;
                }
                var ornnR = GameObjects.Get<MissileClient>().FirstOrDefault(x => x.SpellData.Name == "OrnnRWave");
                if (ornnR == null)
                    return;
                if (ornnR.ServerPosition.Distance(Player) <= SpellManager.W.Range)
                    SpellManager.R.Cast(target.ServerPosition);
            }
        }

        private static void RenderOnOnPresent()
        {
            if (MenuManager.Drawing["q"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.Q.Range, 50, Color.White);
            if (MenuManager.Drawing["w"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.W.Range, 50, Color.Red);
            if (MenuManager.Drawing["e"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.E.Range, 50, Color.Gold);
        }
    }
}
