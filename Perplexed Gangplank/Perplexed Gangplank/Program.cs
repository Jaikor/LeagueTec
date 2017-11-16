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
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Gangplank
{
    public class Program
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        private static int Ammo => Player.SpellBook.GetSpell(SpellSlot.E).Ammo;
        private static int LastQCast;
        private static int LastECast;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            if (Player.ChampionName != "Gangplank")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();

            LastQCast = Game.TickCount;
            LastECast = Game.TickCount;

            BarrelManager.Barrels = new List<Barrel>();
            GameObject.OnCreate += GameObject_OnCreate;
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;
            Obj_AI_Base.OnProcessAutoAttack += ObjAiBaseOnOnProcessAutoAttack;
            Orbwalker.Implementation.PostAttack += ImplementationOnPostAttack;
            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
        }

        private static void ImplementationOnPostAttack(object o, PostAttackEventArgs e)
        {
            if (e.Target == null || !e.Target.IsValid)
                return;
            if (e.Target.Name == "Barrel")
            {
                var barrel = (Barrel)e.Target;
                if (barrel.Health > 1)
                    barrel.Decay(Game.Ping);
            }
        }

        private static void ObjAiBaseOnOnProcessAutoAttack(Obj_AI_Base obj, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (e.Sender.IsMe)
                return;
            if (e.Target.Name == "Barrel")
            {
                var barrel = (Barrel)e.Target;
                if (barrel != null)
                {
                    if (barrel.Health > 1)
                    {
                        if (e.Sender.IsMelee)
                            barrel.Decay(Game.Ping);
                        else
                            barrel.Decay((int) (e.Start.Distance(e.End) / e.SpellData.MissileSpeed) + Game.Ping);
                    }
                }
            }
        }

        private static void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMe && e.SpellSlot == SpellSlot.Q && e.Target.Name == "Barrel")
            {
                var barrel = (Barrel)e.Target;
                LastQCast = Game.TickCount;
                //Console.WriteLine($"[Cast] Distance from barrel: {Player.Distance(barrel.Object)}");
                //1 part
                if (barrel.Object.Distance(Player) >= 585 && barrel.Object.Distance(Player) <= 660 && BarrelManager.GetChainedBarrels(barrel).Count == 1) //1 part combo only works at max range.
                {
                    var enemies = BarrelManager.GetEnemiesInChainRadius(barrel);
                    var bestEnemy = enemies.Where(x => x.IsValidTarget()).OrderBy(x => x.Distance(barrel.Object)).ThenBy(x => x.Health).FirstOrDefault();
                    if (bestEnemy != null)
                    {
                        var bestChainPosition = BarrelManager.GetBestChainPosition(bestEnemy, barrel);
                        if (bestChainPosition != Vector3.Zero && bestEnemy.IsInRange(SpellManager.E.Range) && Player.Distance(bestChainPosition) <= SpellManager.E.Range && SpellManager.E.Ready)
                            SpellManager.E.Cast(bestChainPosition);
                    }
                }
                else if (MenuManager.Combo["triple"].Enabled)
                {
                    //Triple barrel
                    var chainedBarrels = BarrelManager.GetChainedBarrels(barrel);
                    if (chainedBarrels.Count > 1)
                    {
                        var barrelsCanChain = chainedBarrels.Where(x => BarrelManager.GetEnemiesInChainRadius(x).Count > 0 && x.NetworkId != barrel.NetworkId).ToList();
                        if (barrelsCanChain.Count == 0)
                            barrelsCanChain = chainedBarrels.Where(x => BarrelManager.GetEnemiesInChainRadius(x, false).Count > 0 && x.NetworkId != barrel.NetworkId).ToList();
                        var bestBarrel = barrelsCanChain.OrderBy(x => x.Object.Distance(Player)).FirstOrDefault();
                        if (bestBarrel == null)
                            return;
                        var enemiesCanChainTo = BarrelManager.GetEnemiesInChainRadius(bestBarrel);
                        if (enemiesCanChainTo.Count == 0)
                            enemiesCanChainTo = BarrelManager.GetEnemiesInChainRadius(bestBarrel, false);
                        if (enemiesCanChainTo.Count > 0)
                        {
                            var bestEnemy = enemiesCanChainTo.OrderByDescending(x => x.Distance(Player)).FirstOrDefault();
                            if (bestEnemy != null)
                            {
                                var bestChainPosition = BarrelManager.GetBestChainPosition(bestEnemy, bestBarrel);
                                if (bestChainPosition != Vector3.Zero && bestChainPosition.Distance(bestBarrel.Object) > SpellManager.ExplosionRadius && bestEnemy.IsInRange(SpellManager.E.Range) && Player.Distance(bestChainPosition) <= SpellManager.E.Range)
                                {
                                    DelayAction.Queue(250, () =>
                                    {
                                        if (SpellManager.E.Ready)
                                            SpellManager.E.Cast(bestChainPosition);
                                    });
                                }
                            }
                        }
                    }
                }
            }
            else if (sender.IsMe && e.SpellSlot == SpellSlot.E)
                LastECast = Game.TickCount;
        }

        private static void GameObject_OnCreate(GameObject sender)
        {
            if (sender.Name == "Barrel")
                BarrelManager.Barrels.Add(new Barrel(sender as Obj_AI_Base, Game.TickCount));
        }

        private static void Game_OnUpdate()
        {
            BarrelManager.Barrels.RemoveAll(x => x.Object.IsDead || x.Object.Name != "Barrel");
            RemoveCC();
            Killsteal();

            //Auto 1 part
            if (MenuManager.Misc["autoOnePart"].Enabled)
            {
                var onePartBarrels = BarrelManager.Barrels.Where(x => x.Object.Distance(Player) >= 585 && x.Object.Distance(Player) <= 660 && BarrelManager.GetChainedBarrels(x).Count == 1 && x.CanQ);
                var bestBarrel = onePartBarrels.FirstOrDefault(x => BarrelManager.GetEnemiesInChainRadius(x).Count >= 1);

                if (bestBarrel != null && SpellManager.Q.Ready && Ammo >= 1 && Game.TickCount - LastQCast >= 2000)
                {
                    //Console.WriteLine($"[Check] Distance from barrel: {Player.Distance(bestBarrel.Object)}");
                    LastQCast = Game.TickCount;
                    SpellManager.Q.Cast(bestBarrel.Object);
                }
            }
            AutoExplode();

            if ((Orbwalker.Implementation.Mode == OrbwalkingMode.Combo || Orbwalker.Implementation.Mode == OrbwalkingMode.Mixed || Orbwalker.Implementation.Mode == OrbwalkingMode.Lasthit ||
               Orbwalker.Implementation.Mode == OrbwalkingMode.Laneclear || MenuManager.ComboToMouse.Active || MenuManager.ExplodeNearestBarrel.Active) && MenuManager.Misc["aaBarrel"].Enabled)
            {
                AttackNearestBarrel();
            }

            if (MenuManager.ComboToMouse.Active)
            {
                ComboToMouse();
            }

            if (MenuManager.ExplodeNearestBarrel.Active)
            {
                ExplodeNearestBarrel();
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

        private static void AutoExplode()
        {
            var closestHittingBarrel = BarrelManager.GetBarrelsThatWillHit().FirstOrDefault();
            if (closestHittingBarrel != null)
            {
                var chainedBarrels = BarrelManager.GetChainedBarrels(closestHittingBarrel);
                var barrelToQ = BarrelManager.GetBestBarrelToQ(chainedBarrels);
                if (barrelToQ != null)
                {
                    if (SpellManager.Q.Ready)
                    {
                        var timeWhenCanE = LastECast + 500;
                        var delay = timeWhenCanE - Game.TickCount;
                        delay = Ammo < 1 ? 0 : delay <= 0 ? 0 : delay;
                        var castDelay = MenuManager.Combo["triple"].Enabled ? delay : 0;
                        DelayAction.Queue(castDelay, () => SpellManager.Q.Cast(barrelToQ.Object));
                        return;
                    }
                    if (barrelToQ.Object.IsInAutoAttackRange() && Orbwalker.Implementation.CanAttack() && MenuManager.Combo["explodeQCooldown"].Enabled)
                    {
                        Orbwalker.Implementation.ForceTarget(barrelToQ.Object);
                        Orbwalker.Implementation.Attack(barrelToQ.Object);
                    }
                }
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
                            {
                                var timeWhenCanE = LastECast + 500;
                                var delay = timeWhenCanE - Game.TickCount;
                                delay = Ammo < 1 ? 0 : delay <= 0 ? 0 : delay;
                                var castDelay = MenuManager.Combo["triple"].Enabled ? delay : 0;
                                DelayAction.Queue(castDelay, () => SpellManager.Q.Cast(barrelToQ.Object));
                                return;
                            }
                            if (barrelToQ.Object.IsInAutoAttackRange() && Orbwalker.Implementation.CanAttack() && MenuManager.Combo["explodeQCooldown"].Enabled)
                            {
                                Orbwalker.Implementation.ForceTarget(barrelToQ.Object);
                                Orbwalker.Implementation.Attack(barrelToQ.Object);
                            }
                        }
                    }
                    else
                    {
                        //No chained barrels will hit, so let's chain them OR try and triple barrel.
                        if (chainedBarrels.Count >= 2)
                        {
                            //There are chained barrels, so let's see if any are in range to be triple comboed.
                            var barrelToComboFrom = chainedBarrels.FirstOrDefault(x => BarrelManager.GetEnemiesInChainRadius(x).Count >= 1);
                            if (barrelToComboFrom == null)
                                return;
                            var barrelToQ = BarrelManager.GetBestBarrelToQ(chainedBarrels);
                            if (barrelToQ != null)
                            {
                                if (SpellManager.Q.Ready)
                                {
                                    var timeWhenCanE = LastECast + 500;
                                    var delay = timeWhenCanE - Game.TickCount;
                                    delay = delay <= 0 ? 0 : delay;
                                    var castDelay = MenuManager.Combo["triple"].Enabled ? delay : 0;
                                    DelayAction.Queue(castDelay, () => SpellManager.Q.Cast(barrelToQ.Object));
                                    return;
                                }
                                //if (barrelToQ.Object.IsInAutoAttackRange() && Orbwalker.Implementation.CanAttack() && MenuManager.Combo["explodeQCooldown"].Enabled)
                                //{
                                //    Orbwalker.Implementation.ForceTarget(barrelToQ.Object);
                                //    Orbwalker.Implementation.Attack(barrelToQ.Object);
                                //}
                            }
                        }
                        else
                        {
                            var bestChainPosition = BarrelManager.GetBestChainPosition(target, nearestBarrel);
                            if (bestChainPosition != Vector3.Zero && target.IsInRange(SpellManager.E.Range) && Player.Distance(bestChainPosition) <= SpellManager.E.Range && SpellManager.E.Ready && nearestBarrel.CanChain)
                            {
                                Render.Line(nearestBarrel.ServerPosition.ToScreenPosition(), bestChainPosition.ToScreenPosition(), 5, true, Color.Red);
                                SpellManager.E.Cast(bestChainPosition);

                            }
                        }
                    }
                }
            }
        }

        private static void ComboToMouse()
        {
            var nearestBarrelToCursor = BarrelManager.GetNearestBarrel(Game.CursorPos);
            if (nearestBarrelToCursor != null)
            {
                var chainedBarrels = BarrelManager.GetChainedBarrels(nearestBarrelToCursor);
                if (chainedBarrels.Count > 1)
                {
                    var barrelToQ = BarrelManager.GetBestBarrelToQ(chainedBarrels);
                    if (barrelToQ != null)
                    {
                        if (SpellManager.Q.Ready)
                        {
                            SpellManager.Q.Cast(barrelToQ.Object);
                            return;
                        }
                        if (barrelToQ.Object.IsInAutoAttackRange() && Orbwalker.Implementation.CanAttack() && MenuManager.Combo["explodeQCooldown"].Enabled)
                        {
                            Orbwalker.Implementation.ForceTarget(barrelToQ.Object);
                            Orbwalker.Implementation.Attack(barrelToQ.Object);
                        }
                    }
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

        private static void ExplodeNearestBarrel()
        {
            var barrel = BarrelManager.GetNearestBarrel();
            if (barrel != null)
            {
                if (barrel.CanQ)
                {
                    if (SpellManager.Q.Ready)
                    {
                        SpellManager.Q.Cast(barrel.Object);
                        return;
                    }
                    if (barrel.Object.IsInAutoAttackRange() && Orbwalker.Implementation.CanAttack() && MenuManager.Combo["explodeQCooldown"].Enabled)
                    {
                        Orbwalker.Implementation.ForceTarget(barrel.Object);
                        Orbwalker.Implementation.Attack(barrel.Object);
                    }
                }
            }
        }

        private static void Harass()
        {
            if (!MenuManager.Harass["harassQ"].Enabled)
                return;
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
                    var minion = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.Distance(Player) <= SpellManager.Q.Range && !x.IsInAutoAttackRange() && Utility.CanKillWithQ(x) && x.UnitSkinName.Contains("Minion")).OrderBy(x => x.Health).FirstOrDefault();
                    if (minion != null)
                        SpellManager.Q.Cast(minion);
                }
            }
        }

        private static void RemoveCC()
        {
            if (MenuManager.W["wEnable"].Enabled && SpellManager.W.Ready)
            {
                if (Player.HasBuffOfType(BuffType.Blind) && MenuManager.CCTypes["blind"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Charm) && MenuManager.CCTypes["charm"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Fear) && MenuManager.CCTypes["fear"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Flee) && MenuManager.CCTypes["flee"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Polymorph) && MenuManager.CCTypes["polymorph"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Silence) && MenuManager.CCTypes["silence"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Slow) && MenuManager.CCTypes["slow"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Snare) && MenuManager.CCTypes["snare"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Stun) && MenuManager.CCTypes["stun"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Suppression) && MenuManager.CCTypes["suppression"].Enabled)
                    SpellManager.W.Cast();
                else if (Player.HasBuffOfType(BuffType.Taunt) && MenuManager.CCTypes["taunt"].Enabled)
                    SpellManager.W.Cast();
            }
        }

        private static void Killsteal()
        {
            if (SpellManager.Q.Ready && MenuManager.Killsteal["killstealQ"].Enabled)
            {
                var target = Utility.GetAllEnemiesInRange(SpellManager.Q.Range).Where(Utility.CanKillWithQ).OrderBy(x => x.Health).FirstOrDefault();
                if (target != null)
                {
                    SpellManager.Q.Cast(target);
                    return;
                }
            }
            if (SpellManager.R.Ready && MenuManager.Killsteal["killstealR"].Enabled)
            {
                var waves = MenuManager.Killsteal["killstealRWaves"].Value;
                var target = Utility.GetAllEnemiesInRange(SpellManager.R.Range).Where(x => Utility.CanKillWithR(x, waves) && x.Distance(Player) > SpellManager.E2.Range).OrderBy(x => x.Health).FirstOrDefault();
                if (target != null)
                    SpellManager.R.Cast(target.ServerPosition);
            }
        }

        private static void AttackNearestBarrel()
        {
            var nearestBarrel = BarrelManager.GetNearestBarrel();
            if (nearestBarrel != null)
            {
                if (nearestBarrel.Object.IsInAutoAttackRange() && nearestBarrel.Health >= 2 && Orbwalker.Implementation.CanAttack() && !Orbwalker.Implementation.IsWindingUp)
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
                //Render.Text(barrel.ServerPosition.ToScreenPosition(), Color.Red, $"Can Q: {barrel.CanQTime} | {barrel.CanQ}");
                if (MenuManager.Drawing["drawBarrelExplode"].Enabled)
                    Render.Circle(barrel.ServerPosition, SpellManager.ExplosionRadius, 30, Color.Gold);
                if (MenuManager.Drawing["drawBarrelChain"].Enabled)
                    Render.Circle(barrel.ServerPosition, SpellManager.ChainRadius, 30, Color.Orange);
            }
        }
    }
}
