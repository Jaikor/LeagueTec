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

namespace Perplexed_Gangplank
{
    public class Program
    {
        private static Obj_AI_Hero Player;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            Player = ObjectManager.GetLocalPlayer();
            if (Player.ChampionName != "Gangplank")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();

            BarrelManager.Barrels = new List<Barrel>();
            GameObject.OnCreate += GameObject_OnCreate;
            BuffManager.OnAddBuff += BuffManager_OnAddBuff;
            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
        }

        private static void BuffManager_OnAddBuff(Obj_AI_Base sender, Buff buff)
        {
        }

        private static void GameObject_OnCreate(GameObject sender)
        {
            if (sender.Name == "Barrel")
                BarrelManager.Barrels.Add(new Barrel(sender as Obj_AI_Base, Game.TickCount));
        }

        private static void Game_OnUpdate()
        {
            BarrelManager.Barrels.RemoveAll(x => x.Object.IsDead);
            //var closestHittingBarrel = BarrelManager.GetBarrelsThatWillHit().FirstOrDefault();
            //if (closestHittingBarrel != null)
            //{
            //    var chainedBarrels = BarrelManager.GetChainedBarrels(closestHittingBarrel);
            //    var barrelToQ = BarrelManager.GetBestBarrelToQ(chainedBarrels);
            //    if (barrelToQ != null && SpellManager.Q.Ready)
            //    {
            //        SpellManager.Q.Cast(barrelToQ.Object);
            //        return;
            //    }
            //}
            if ((Orbwalker.Implementation.Mode == OrbwalkingMode.Combo || Orbwalker.Implementation.Mode == OrbwalkingMode.Mixed || Orbwalker.Implementation.Mode == OrbwalkingMode.Lasthit ||
               Orbwalker.Implementation.Mode == OrbwalkingMode.Laneclear) && MenuManager.Misc["aaBarrel"].Enabled)
            {
                AttackNearestBarrel();
            }
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo();
                    break;
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(SpellManager.E2.Range);
            if (target.IsValidTarget())
            {
                var nearestBarrel = BarrelManager.GetNearestBarrel();
                if (nearestBarrel != null)
                {
                    var chainedBarrels = BarrelManager.GetChainedBarrels(nearestBarrel);
                    if (chainedBarrels.Any(x => BarrelManager.BarrelWillHit(x, target)))
                    {
                        //If any of the chained barrels will hit, cast Q on best barrel.
                        var barrelToQ = BarrelManager.GetBestBarrelToQ(chainedBarrels);
                        if (barrelToQ != null)
                        {
                            if (SpellManager.Q.Ready)
                                SpellManager.Q.Cast(barrelToQ.Object);
                            else if (barrelToQ.Object.Distance(Player) <= Player.AttackRange && Orbwalker.Implementation.CanAttack() && MenuManager.Combo["explodeQCooldown"].Enabled)
                                Orbwalker.Implementation.Attack(barrelToQ.Object);
                            return;
                        }
                    }
                    else
                    {
                        //No chained barrels will hit, so let's chain them.
                        if (nearestBarrel.Object.Distance(target) <= SpellManager.ChainRadius)
                        {
                            if (target.IsInRange(SpellManager.E.Range) && SpellManager.E.Ready && nearestBarrel.CanChain)
                            {
                                SpellManager.E.Cast(target.ServerPosition);
                                return;
                            }
                        }
                        else if (nearestBarrel.Object.Distance(target) <= SpellManager.ChainRadius + SpellManager.ExplosionRadius)
                        {
                            var bestCastPos = nearestBarrel.ServerPosition.Extend(target.ServerPosition, (SpellManager.ChainRadius - 5));
                            Render.Line(nearestBarrel.ServerPosition.ToScreenPosition(), bestCastPos.ToScreenPosition(), 5, true, Color.Red);
                            if (target.IsInRange(SpellManager.E.Range) && SpellManager.E.Ready && Player.Distance(bestCastPos) <= SpellManager.E.Range && nearestBarrel.CanChain)
                            {
                                SpellManager.E.Cast(bestCastPos);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private static void AttackNearestBarrel()
        {
            var nearestBarrel = BarrelManager.GetNearestBarrel();
            if (nearestBarrel != null)
            {
                if (nearestBarrel.Object.Distance(Player) <= Player.AttackRange && nearestBarrel.Health >= 2 && Orbwalker.Implementation.CanAttack())
                    Orbwalker.Implementation.Attack(nearestBarrel.Object);
            }
        }

        private static void Render_OnPresent()
        {
            var target = TargetSelector.GetTarget(SpellManager.E2.Range);
            if(target != null)
                Render.Circle(target.ServerPosition, SpellManager.ExplosionRadius, 30, Color.Green);
            Render.Circle(Player.ServerPosition, SpellManager.Q.Range, 30, Color.White);
            Render.Circle(Player.ServerPosition, SpellManager.E.Range, 30, Color.Orange);
            //Render.Text(Player.ServerPosition.ToScreenPosition(), Color.Red, $"Barrels will hit: {BarrelManager.GetBarrelsThatWillHit().Count}");
            foreach (var barrel in BarrelManager.Barrels)
            {
                Render.Circle(barrel.ServerPosition, SpellManager.ExplosionRadius, 30, Color.Gold);
                Render.Circle(barrel.ServerPosition, SpellManager.ChainRadius, 30, Color.Orange);
                //Render.Text(barrel.ServerPosition.ToScreenPosition(), Color.Red, $"Chain: {BarrelManager.GetChainedBarrels(barrel).Count}");
            }
        }
    }
}
