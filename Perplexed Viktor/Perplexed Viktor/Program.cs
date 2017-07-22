using System.Drawing;
using Aimtec;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Events;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Damage;
using System.Linq;
using System;

namespace Perplexed_Viktor
{
    public class Program
    {
        public static Obj_AI_Hero Player;
        public static bool HasQBuff => Player.HasBuff("viktorpowertransferreturn");
        public static bool UltCasted
        {
            get
            {
                if (Player.SpellBook.GetSpell(SpellSlot.R) == null)
                    return false;
                else
                    return Player.SpellBook.GetSpell(SpellSlot.R).Name != "ViktorChaosStorm";
            }
        }
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            Player = ObjectManager.GetLocalPlayer();
            if (Player.ChampionName != "Viktor")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();
            Game.OnUpdate += Game_OnUpdate;
            SpellBook.OnCastSpell += SpellBook_OnCastSpell;
            Render.OnPresent += Render_OnPresent;
        }


        private static void Render_OnPresent()
        {
            var target = TargetSelector.GetTarget(SpellManager.E.Range + SpellManager.E2.Range);
            if (target.IsValidTarget())
            {
                if (!target.IsInRange(SpellManager.E.Range))
                {
                    var startPos = Player.ServerPosition.Extend(target.ServerPosition, SpellManager.E.Range);
                    var endPos = target.ServerPosition;
                    Render.Line(startPos.ToScreenPosition(), endPos.ToScreenPosition(), Color.Red);
                    Render.Text(startPos.ToScreenPosition(), Color.Black, "A");
                    Render.Text(endPos.ToScreenPosition(), Color.Black, "B");
                }
                //Render.Text(target.ServerPosition.ToScreenPosition().X - 10, target.ServerPosition.ToScreenPosition().Y - 50, Color.Red, $"Combo dmg: {GetDamageCanDeal(target)}");
            }
        }

        private static void SpellBook_OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            if (sender.IsMe && e.Slot == SpellSlot.Q)
                Orbwalker.Implementation.ResetAutoAttackTimer();
        }

        private static void Game_OnUpdate()
        {
            if (UltCasted)
                AutoFollow();
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
            if (MenuManager.Combo["comboR"].As<MenuBool>().Enabled && SpellManager.R.Ready && !UltCasted)
            {
                var target = TargetSelector.GetTarget(SpellManager.R.Range);
                if (target.IsValidTarget())
                {
                    if (GetUltTotalDamage(target) > target.Health || GetDamageCanDeal(target) > target.Health)
                    {
                        SpellManager.R.Cast(target);
                        return;
                    }
                }
            }
            if (MenuManager.Combo["comboQ"].As<MenuBool>().Enabled && SpellManager.Q.Ready)
            {
                var target = TargetSelector.GetTarget(SpellManager.Q.Range);
                if (target.IsValidTarget())
                {
                    SpellManager.Q.Cast(target);
                    return;
                }
            }
            if (MenuManager.Combo["comboW"].As<MenuBool>().Enabled && SpellManager.W.Ready)
            {
                var target = TargetSelector.GetTarget(SpellManager.W.Range);
                if (target.IsValidTarget())
                {
                    SpellManager.W.Cast(target);
                    return;
                }
            }
            if (MenuManager.Combo["comboE"].As<MenuBool>().Enabled && SpellManager.E.Ready)
            {
                var target = TargetSelector.GetTarget(SpellManager.E.Range + SpellManager.E2.Range);
                if (target.IsValidTarget() && (!HasQBuff || !target.IsInRange(Player.AttackRange)))
                {
                    var bestCastPos = GetBestCastPositionE(target);
                    SpellManager.E.Cast(bestCastPos.Start, bestCastPos.End);
                    return;
                }
            }
        }

        private static void Harass()
        {
            var minManaPct = MenuManager.Harass["harassManaPct"].As<MenuSlider>().Value;
            if(MenuManager.Harass["harassQ"].As<MenuBool>().Enabled && SpellManager.Q.Ready && Player.ManaPercent() >= minManaPct)
            {
                var target = TargetSelector.GetTarget(SpellManager.Q.Range);
                if (target.IsValidTarget())
                {
                    SpellManager.Q.Cast(target);
                    return;
                }
            }
            if (MenuManager.Harass["harassE"].As<MenuBool>().Enabled && SpellManager.E.Ready && Player.ManaPercent() >= minManaPct)
            {
                var target = TargetSelector.GetTarget(SpellManager.E.Range + SpellManager.E2.Range);
                if (target.IsValidTarget() && (!HasQBuff || !target.IsInRange(Player.AttackRange)))
                {
                    var bestCastPos = GetBestCastPositionE(target);
                    SpellManager.E.Cast(bestCastPos.Start, bestCastPos.End);
                    return;
                }
            }
        }

        public static void AutoFollow()
        {
            var target = TargetSelector.GetTarget(float.MaxValue);
            if (target.IsValidTarget())
                SpellManager.R.Cast(target.ServerPosition);
        }

        public static BestCastPosition GetBestCastPositionE(Obj_AI_Base target)
        {

            if (!target.IsInRange(SpellManager.E.Range))
            {
                var startPos = target.ServerPosition;
                var endPos = Player.ServerPosition.Extend(target.ServerPosition, SpellManager.E.Range);
                return new BestCastPosition
                {
                    Start = startPos,
                    End = endPos
                };
            }
            else
                return new BestCastPosition
                {
                    Start = target.ServerPosition,
                    End = target.ServerPosition
                };
        }

        public static double GetDamageCanDeal(Obj_AI_Base target)
        {
            double dmg = 0;
            var mana = Player.Mana;
            var qCost = SpellManager.Q.Instance().Cost;
            var eCost = SpellManager.E.Instance().Cost;
            var rCost = SpellManager.R.Instance().Cost;
            if (SpellManager.Q.Ready)
            {
                dmg = Player.GetSpellDamage(target, SpellSlot.Q);
                dmg += (Player.SpellBook.GetSpell(SpellSlot.Q).Level * 20) + Player.GetAutoAttackDamage(target) + (Player.TotalAbilityDamage / 2);
                mana -= qCost;
            }
            if (Player.HasItem(ItemId.LichBane))
                dmg += (Player.BaseAttackDamage * 0.75) + (Player.TotalAbilityDamage / 2);
            if (SpellManager.E.Ready && mana >= eCost)
            {
                dmg += Player.GetSpellDamage(target, SpellSlot.E);
                mana -= eCost;
            }
            if(SpellManager.R.Ready && mana >= eCost && !UltCasted)
                dmg += GetUltTotalDamage(target);
            return dmg;
        }

        public static double GetUltTotalDamage(Obj_AI_Base target)
        {
            double dmg = Player.GetSpellDamage(target, SpellSlot.R);
            dmg += GetUltTickDamage(target);
            return dmg;
        }

        public static double GetUltTickDamage(Obj_AI_Base target)
        {
            double dmg = 0;
            var ultLevel = Player.SpellBook.GetSpell(SpellSlot.R).Level;
            dmg = ((100 * ultLevel) + 50) * 3;
            dmg += Player.TotalAbilityDamage * 0.6;
            dmg = Player.CalculateDamage(target, DamageType.Magical, dmg);
            return dmg;
        }
    }
}
