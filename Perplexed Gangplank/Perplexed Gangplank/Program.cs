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
using Aimtec.SDK.Util.Cache;

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
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;
            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
        }

        private static void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMe && e.SpellSlot == SpellSlot.Q && e.Target.Name == "Barrel")
            {
                var barrel = BarrelManager.GetBarrelsThatWillHit().FirstOrDefault();
                if (barrel != null)
                {
                    var enemies = BarrelManager.GetEnemiesInChainRadius(barrel);
                    var bestEnemy = enemies.Where(x => x.IsValidTarget()).OrderBy(x => x.Health).FirstOrDefault();
                    if (bestEnemy != null)
                    {
                        if (bestEnemy.IsInRange(SpellManager.E.Range))
                        {
                            Console.WriteLine("CAST DAT TING");
                            SpellManager.E.Cast(bestEnemy.ServerPosition);
                        }
                    }
                }
            }
        }

        private static void GameObject_OnCreate(GameObject sender)
        {
            if (sender.Name == "Barrel")
                BarrelManager.Barrels.Add(new Barrel(sender as Obj_AI_Base, Game.TickCount));
        }

        private static void Game_OnUpdate()
        {
            BarrelManager.Barrels.RemoveAll(x => x.Object.IsDead);
            RemoveCC();
            Killsteal();

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

            if(MenuManager.Combo["comboToMouse"].Enabled)
                ComboToMouse();

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
                case OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case OrbwalkingMode.Lasthit:
                    LastHit();
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

        private static void ComboToMouse()
        {
            Orbwalker.Implementation.Move(Game.CursorPos);
            var nearestBarrelToCursor = BarrelManager.GetNearestBarrel(Game.CursorPos);
            if (nearestBarrelToCursor != null)
            {
                var chainedBarrels = BarrelManager.GetChainedBarrels(nearestBarrelToCursor);
                if (chainedBarrels.Count > 1)
                {
                    var barrelToQ = BarrelManager.GetBestBarrelToQ(chainedBarrels);
                    if (barrelToQ != null && SpellManager.Q.Ready)
                        SpellManager.Q.Cast(barrelToQ.Object);
                }
                else
                {
                    if (nearestBarrelToCursor.Object.Distance(Game.CursorPos) <= SpellManager.ChainRadius)
                    {
                        if (SpellManager.E.Ready && nearestBarrelToCursor.CanChain)
                            SpellManager.E.Cast(Game.CursorPos);
                    }
                    else
                    {
                        var bestPos = nearestBarrelToCursor.ServerPosition.Extend(Game.CursorPos, (SpellManager.ChainRadius - 5));
                        if (SpellManager.E.Ready && nearestBarrelToCursor.CanChain)
                            SpellManager.E.Cast(bestPos);
                    }
                }
            }
            else
            {
                if (Player.Distance(Game.CursorPos) <= SpellManager.E.Range && SpellManager.E.Ready)
                    SpellManager.E.Cast(Game.CursorPos);
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range);
            var minManaPct = MenuManager.Harass["harassManaPct"].Value;
            if (target.IsValidTarget() && SpellManager.Q.Ready && Player.ManaPercent() >= minManaPct)
                SpellManager.Q.Cast(target);
        }

        private static void LastHit()
        {
            if (MenuManager.LastHit["lasthitQ"].Enabled && SpellManager.Q.Ready)
            {
                var minManaPct = MenuManager.LastHit["lasthitManaPct"].Value;
                if (Player.ManaPercent() >= minManaPct)
                {
                    var minion = GameObjects.EnemyMinions.Where(x => x.Distance(Player) <= SpellManager.Q.Range && x.Distance(Player) > Player.AttackRange && Utility.CanKillWithQ(x) && x.UnitSkinName.Contains("Minion")).OrderBy(x => x.Health).FirstOrDefault();
                    if (minion != null)
                        SpellManager.Q.Cast(minion);
                }
            }
        }
        
        private static void RemoveCC()
        {
            if((Player.HasBuffOfType(BuffType.Charm) || Player.HasBuffOfType(BuffType.Fear) || Player.HasBuffOfType(BuffType.Flee) || Player.HasBuffOfType(BuffType.Slow) ||
                Player.HasBuffOfType(BuffType.Snare) || Player.HasBuffOfType(BuffType.Stun) || Player.HasBuffOfType(BuffType.Taunt) || Player.HasBuffOfType(BuffType.Suppression)) && SpellManager.W.Ready)
                SpellManager.W.Cast();
        }

        private static void Killsteal()
        {
            if (SpellManager.Q.Ready && MenuManager.Killsteal["killstealQ"].Enabled)
            {
                var target = Utility.GetAllEnemiesInRange(SpellManager.Q.Range).Where(x => Utility.CanKillWithQ(x)).OrderBy(x => x.Health).FirstOrDefault();
                if (target != null)
                {
                    SpellManager.Q.Cast(target);
                    return;
                }
            }
            if (SpellManager.R.Ready && MenuManager.Killsteal["killstealR"].Enabled)
            {
                var waves = MenuManager.Killsteal["killstealRWaves"].Value;
                var target = Utility.GetAllEnemiesInRange(SpellManager.R.Range).Where(x => Utility.CanKillWithR(x, waves) && x.Distance(Player) > SpellManager.Q.Range).OrderBy(x => x.Health).FirstOrDefault();
                if (target != null)
                {
                    SpellManager.R.Cast(target.ServerPosition);
                    return;
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
            if (MenuManager.Drawing["drawQ"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.Q.Range, 30, Color.White);
            if (MenuManager.Drawing["drawE"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.E.Range, 30, Color.Orange);
            //Render.Text(Player.ServerPosition.ToScreenPosition(), Color.Red, $"Barrels will hit: {BarrelManager.GetBarrelsThatWillHit().Count}");
            foreach (var barrel in BarrelManager.Barrels)
            {
                //Render.Text(barrel.ServerPosition.ToScreenPosition(), Color.Red, $"Chain: {BarrelManager.GetChainedBarrels(barrel).Count}");
                if (MenuManager.Drawing["drawBarrelExplode"].Enabled)
                    Render.Circle(barrel.ServerPosition, SpellManager.ExplosionRadius, 30, Color.Gold);
                if (MenuManager.Drawing["drawBarrelChain"].Enabled)
                    Render.Circle(barrel.ServerPosition, SpellManager.ChainRadius, 30, Color.Orange);
                //Render.Text(barrel.ServerPosition.ToScreenPosition(), Color.Red, $"Chain: {BarrelManager.GetChainedBarrels(barrel).Count}");
            }
        }
    }
}
