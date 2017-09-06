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

namespace Perplexed_Renekton
{
    public class Program
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        private static bool EIsEmpowered => Player.SpellBook.GetSpell(SpellSlot.E).Name == "RenektonDice";
        private static bool HasWBuff => Player.HasBuff("RenektonPreExecute");
        private static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            if (Player.ChampionName != "Renekton")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();

            Orbwalker.Implementation.PostAttack += ImplementationOnPostAttack;
            BuffManager.OnRemoveBuff += BuffManagerOnOnRemoveBuff;
            Game.OnUpdate += GameOnOnUpdate;
            Render.OnPresent += RenderOnOnPresent;
        }

        private static void ImplementationOnPostAttack(object o, PostAttackEventArgs e)
        {
            if (e.Target is Obj_AI_Hero)
            {
                if (!SpellManager.W.Ready && !HasWBuff && (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo || Orbwalker.Implementation.Mode == OrbwalkingMode.Mixed))
                    UseItems();
                else if (MenuManager.Combo["w"].Enabled && Orbwalker.Implementation.Mode == OrbwalkingMode.Combo || MenuManager.Harass["w"].Enabled && Orbwalker.Implementation.Mode == OrbwalkingMode.Mixed)
                {
                    if (e.Target.Distance(Player) <= SpellManager.W.Range)
                        SpellManager.W.Cast();
                }
            }
            else if (e.Target is Obj_AI_Minion minion)
            {
                var isJungleMinion = GameObjects.Jungle.Contains(minion);
                if (!SpellManager.W.Ready && !HasWBuff && Orbwalker.Implementation.Mode == OrbwalkingMode.Laneclear)
                {
                    UseItems();
                    return;
                }
                if (isJungleMinion && MenuManager.Jungleclear["w"].Enabled && SpellManager.W.Ready && Orbwalker.Implementation.Mode == OrbwalkingMode.Laneclear)
                {
                    if (e.Target.Distance(Player) <= SpellManager.W.Range)
                    {
                        if (MenuManager.Jungleclear["saveFury"].Enabled && Player.Mana >= 50)
                            return;
                        SpellManager.W.Cast();
                        return;
                    }
                }
                if (!isJungleMinion && MenuManager.Laneclear["w"].Enabled && SpellManager.W.Ready && Orbwalker.Implementation.Mode == OrbwalkingMode.Laneclear)
                {
                    if (e.Target.Distance(Player) <= SpellManager.W.Range)
                    {
                        if (MenuManager.Laneclear["saveFury"].Enabled && Player.Mana >= 50)
                            return;
                        SpellManager.W.Cast();
                    }
                }
            }
        }

        private static void BuffManagerOnOnRemoveBuff(Obj_AI_Base target, Buff buff)
        {
            if (target.IsMe && buff.Name == "RenektonPreExecute" && (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo || Orbwalker.Implementation.Mode == OrbwalkingMode.Mixed || Orbwalker.Implementation.Mode == OrbwalkingMode.Laneclear))
            {
                if (SpellManager.R.Ready && Orbwalker.Implementation.Mode == OrbwalkingMode.Combo && MenuManager.Combo["r"].Enabled && MenuManager.Combo["rMode"].Value == 0)
                {
                    //Only cast R after W if R mode is always
                    SpellManager.R.Cast();
                    DelayAction.Queue(250, UseItems);
                    return;
                } 
                UseItems();
            }
        }

        private static void GameOnOnUpdate()
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
                case OrbwalkingMode.Laneclear:
                    Laneclear();
                    Jungleclear();
                    break;
            }
        }
        private static void RenderOnOnPresent()
        {
            if(MenuManager.Drawing["q"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.Q.Range, 50, Color.Red);
            if (MenuManager.Drawing["w"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.W.Range, 50, Color.Gold);
            if (MenuManager.Drawing["e"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.E.Range, 50, Color.White);
            if (MenuManager.Drawing["extendedE"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.ExtendedE.Range, 50, Color.White);

        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(SpellManager.ExtendedE.Range);
            if (target == null)
                return;
            if (HasWBuff)
                return;
            if ((MenuManager.Combo["q"].Enabled && SpellManager.Q.Ready || MenuManager.Combo["w"].Enabled && SpellManager.W.Ready) && target.Distance(Player) <= SpellManager.W.Range && Orbwalker.Implementation.CanAttack())
            {
                //If we can weave auto attacks.
                Orbwalker.Implementation.Attack(target);
            }
            if (MenuManager.Combo["e"].Enabled && SpellManager.E.Ready)
            {
                if (!MenuManager.Combo["eTurret"].Enabled && target.IsUnderEnemyTurret())
                    return;
                if (target.Distance(Player) <= SpellManager.E.Range)
                {
                    if (!EIsEmpowered)
                        SpellManager.E.Cast(target.ServerPosition);
                }
                else if(MenuManager.Combo["extendedE"].Enabled && target.Distance(Player) <= SpellManager.ExtendedE.Range)
                {
                    var enemyTargets = GameObjects.EnemyHeroes.Cast<Obj_AI_Base>().ToList();
                    enemyTargets.AddRange(GameObjects.EnemyMinions.Cast<Obj_AI_Base>().ToList());
                    var targetToEFirst = enemyTargets.FirstOrDefault(x => x.Distance(Player) <= SpellManager.E.Range && x.Distance(target) <= SpellManager.E.Range);
                    if (targetToEFirst == null)
                        return;
                    SpellManager.E.Cast(targetToEFirst.ServerPosition);
                    DelayAction.Queue(500, () => SpellManager.E.Cast(target));
                }
            }
            if (MenuManager.Combo["w"].Enabled && SpellManager.W.Ready)
            {
                if (target.Distance(Player) <= SpellManager.W.Range)
                    SpellManager.W.Cast();
            }
            if (MenuManager.Combo["r"].Enabled && SpellManager.R.Ready && !SpellManager.W.Ready && target.Health < Player.GetComboDamage(target) && target.Distance(Player) <= SpellManager.W.Range)
            {
                SpellManager.R.Cast();
                DelayAction.Queue(250, UseItems);
            }
            if (MenuManager.Combo["q"].Enabled && SpellManager.Q.Ready)
            {
                if (MenuManager.Combo["w"].Enabled && SpellManager.W.Ready || HasWBuff)
                    return;
                if (target.Distance(Player) <= SpellManager.Q.Range)
                    SpellManager.Q.Cast();
            }
            if (MenuManager.Combo["e"].Enabled && SpellManager.E.Ready && EIsEmpowered)
            {
                if (MenuManager.Combo["q"].Enabled && SpellManager.Q.Ready)
                    return;
                if (MenuManager.Combo["w"].Enabled && SpellManager.W.Ready)
                    return;
                if (!MenuManager.Combo["eTurret"].Enabled && target.IsUnderEnemyTurret())
                    return;
                if (target.Distance(Player) <= SpellManager.E.Range)
                    SpellManager.E.Cast(target.ServerPosition);
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(SpellManager.E.Range);
            if (target == null)
                return;
            if (HasWBuff)
                return;
            if ((MenuManager.Harass["q"].Enabled && SpellManager.Q.Ready || MenuManager.Harass["w"].Enabled && SpellManager.W.Ready) && target.Distance(Player) <= SpellManager.W.Range && Orbwalker.Implementation.CanAttack())
            {
                //If we can weave auto attacks.
                Orbwalker.Implementation.Attack(target);
            }
            if (MenuManager.Harass["e"].Enabled && SpellManager.E.Ready)
            {
                if (target.Distance(Player) <= SpellManager.E.Range)
                {
                    if (!EIsEmpowered)
                    {
                        if (!MenuManager.Harass["eTurret"].Enabled && target.IsUnderEnemyTurret())
                            return;
                        SpellManager.E.Cast(target.ServerPosition);
                    }
                }
            }
            if (MenuManager.Harass["w"].Enabled && SpellManager.W.Ready)
            {
                if (target.Distance(Player) <= SpellManager.W.Range)
                    SpellManager.W.Cast();
            }
            if (MenuManager.Harass["q"].Enabled && SpellManager.Q.Ready)
            {
                if (MenuManager.Harass["w"].Enabled && SpellManager.W.Ready || HasWBuff)
                    return;
                if (target.Distance(Player) <= SpellManager.Q.Range)
                    SpellManager.Q.Cast();
            }
            if (MenuManager.Harass["e"].Enabled && SpellManager.E.Ready && EIsEmpowered)
            {
                if (MenuManager.Harass["q"].Enabled && SpellManager.Q.Ready)
                    return;
                if (MenuManager.Harass["w"].Enabled && SpellManager.W.Ready)
                    return;
                var nearestTurret = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Player)).FirstOrDefault();
                if (nearestTurret == null)
                    return;
                SpellManager.E.Cast(nearestTurret.ServerPosition);
            }
        }

        private static void Laneclear()
        {
            if (MenuManager.Laneclear["saveFury"].Enabled && Player.Mana >= 50)
                return;
            var target = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellManager.E.Range));
            if (target == null)
                return;
            if (HasWBuff)
                return;
            if ((MenuManager.Laneclear["q"].Enabled && SpellManager.Q.Ready || MenuManager.Laneclear["w"].Enabled && SpellManager.W.Ready) && target.Distance(Player) <= Player.AttackRange && Orbwalker.Implementation.CanAttack())
            {
                //If we can weave auto attacks.
                Orbwalker.Implementation.Attack(target);
                return;
            }
            if (MenuManager.Laneclear["e"].Enabled && SpellManager.E.Ready && !EIsEmpowered)
            {
                if (target.Distance(Player) <= SpellManager.E.Range)
                {
                    SpellManager.E.Cast(target.ServerPosition);
                    return;
                }
            }
            if (MenuManager.Laneclear["q"].Enabled && SpellManager.Q.Ready)
            {
                if (MenuManager.Laneclear["w"].Enabled && SpellManager.W.Ready)
                    return;
                var minionsInQRange = GameObjects.EnemyMinions.Where(x => x.IsValidTarget(SpellManager.Q.Range)).ToList();
                if (minionsInQRange.Count >= MenuManager.Laneclear["qMinions"].Value)
                {
                    SpellManager.Q.Cast();
                    return;
                }
            }
            if (MenuManager.Laneclear["e"].Enabled && SpellManager.E.Ready && EIsEmpowered && !SpellManager.Q.Ready && !SpellManager.W.Ready)
            {
                if (MenuManager.Laneclear["q"].Enabled && SpellManager.Q.Ready)
                    return;
                if (MenuManager.Laneclear["w"].Enabled && SpellManager.W.Ready)
                    return;
                if (target.Distance(Player) <= SpellManager.E.Range)
                    SpellManager.E.Cast(target.ServerPosition);
            }
        }

        private static void Jungleclear()
        {
            if (MenuManager.Jungleclear["saveFury"].Enabled && Player.Mana >= 50)
                return;
            var target = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget(SpellManager.E.Range));
            if (target == null)
                return;
            if (HasWBuff)
                return;
            if ((MenuManager.Jungleclear["q"].Enabled && SpellManager.Q.Ready || MenuManager.Jungleclear["w"].Enabled && SpellManager.W.Ready) && target.Distance(Player) <= Player.AttackRange && Orbwalker.Implementation.CanAttack())
            {
                //If we can weave auto attacks.
                Orbwalker.Implementation.Attack(target);
                return;
            }
            if (MenuManager.Jungleclear["e"].Enabled && SpellManager.E.Ready && !EIsEmpowered)
            {
                if (target.Distance(Player) <= SpellManager.E.Range)
                {
                    SpellManager.E.Cast(target.ServerPosition);
                    return;
                }
            }
            if (MenuManager.Jungleclear["q"].Enabled && SpellManager.Q.Ready)
            {
                if (MenuManager.Jungleclear["w"].Enabled && SpellManager.W.Ready)
                    return;
                if (target.Distance(Player) <= SpellManager.Q.Range)
                {
                    SpellManager.Q.Cast();
                    return;
                }
            }
            if (MenuManager.Jungleclear["e"].Enabled && SpellManager.E.Ready && EIsEmpowered && !SpellManager.Q.Ready && !SpellManager.W.Ready)
            {
                if (MenuManager.Jungleclear["q"].Enabled && SpellManager.Q.Ready)
                    return;
                if (MenuManager.Jungleclear["w"].Enabled && SpellManager.W.Ready)
                    return;
                if (target.Distance(Player) <= SpellManager.E.Range)
                    SpellManager.E.Cast(target.ServerPosition);
            }
        }

        private static void Killsteal()
        {
            if(MenuManager.Killsteal["q"].Enabled && SpellManager.Q.Ready)
            {
                var killable = GameObjects.EnemyHeroes.Where(x => !x.IsDead && !x.IsInvulnerable && x.Health < Player.GetQDamage(x) && x.Distance(Player) <= SpellManager.Q.Range).ToList();
                if (killable.Count == 0)
                    return;
                SpellManager.Q.Cast();
                return;
            }
            if(MenuManager.Killsteal["w"].Enabled && SpellManager.W.Ready)
            {
                var killable = GameObjects.EnemyHeroes.Where(x => !x.IsDead && !x.IsInvulnerable && x.Health < Player.GetWDamage(x) && x.Distance(Player) <= SpellManager.W.Range).OrderBy(x => x.Health).FirstOrDefault();
                if (killable == null)
                    return;
                SpellManager.W.Cast();
                DelayAction.Queue(250, () => Orbwalker.Implementation.Attack(killable));
                return;
            }
            if(MenuManager.Killsteal["e"].Enabled && SpellManager.E.Ready)
            {
                var killable = GameObjects.EnemyHeroes.Where(x => !x.IsDead && !x.IsInvulnerable && x.Health < Player.GetEDamage(x) && x.Distance(Player) <= SpellManager.E.Range).OrderBy(x => x.Health).FirstOrDefault();
                if (killable == null)
                    return;
                SpellManager.E.Cast(killable.ServerPosition);
            }
        }

        private static void UseItems()
        {
            //Use tiamat item.
            if (!Player.HasItem(ItemId.Tiamat) && !Player.HasItem(ItemId.RavenousHydra) && !Player.HasItem(ItemId.TitanicHydra))
                return;
            var items = new[] { ItemId.Tiamat, ItemId.RavenousHydra, ItemId.TitanicHydra };
            var slot = Player.Inventory.Slots.First(x => items.Contains(x.ItemId));
            if (slot == null)
                return;
            if (slot.SpellSlot != SpellSlot.Unknown && Player.SpellBook.GetSpell(slot.SpellSlot).State == SpellState.Ready)
                Player.SpellBook.CastSpell(slot.SpellSlot);
        }
    }
}
