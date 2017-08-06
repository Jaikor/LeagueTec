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

namespace Perplexed_Pantheon
{
    class Program
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        private static int ECasted = 0;
        private static bool CastingE => Game.TickCount - ECasted < 1000 || Player.HasBuff("pantheonesound");
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static void GameEventsOnGameStart()
        {
            if (Player.ChampionName != "Pantheon")
                return;

            ECasted = Game.TickCount;

            MenuManager.Initialize();
            SpellManager.Initialize();

            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Render.OnPresent += RenderOnOnPresent;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMe && e.SpellSlot == SpellSlot.E)
                ECasted = Game.TickCount;
        }

        private static void Game_OnUpdate()
        {
            Killsteal();

            Orbwalker.Implementation.MovingEnabled = !CastingE;
            Orbwalker.Implementation.AttackingEnabled = !CastingE;

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
        private static void RenderOnOnPresent()
        {
            if(MenuManager.Drawing["drawRange"].Enabled)
                Render.Circle(Player.ServerPosition, 600, 30, Color.White);
            if (MenuManager.Drawing["showVuln"].Enabled)
            {
                foreach (var hero in Utility.GetVulnerableHeroes())
                    Render.Text(hero.ServerPosition.ToScreenPosition(), Color.Red, "**CAN CRIT**");
            }
        }

        private static void Combo()
        {
            if (CastingE)
                return;
            var target = TargetSelector.GetTarget(600);
            if (target == null)
                return;
            if (MenuManager.Combo["comboW"].Enabled && SpellManager.W.Ready)
            {
                SpellManager.W.Cast(target);
                return;
            }
            if (MenuManager.ComboQ["on"].Enabled && SpellManager.Q.Ready)
            {
                if (MenuManager.ComboQ["whenStunned"].Enabled)
                {
                    if (target.HasBuffOfType(BuffType.Stun))
                    {
                        SpellManager.Q.Cast(target);
                        return;
                    }
                }
                else
                {
                    SpellManager.Q.Cast(target);
                    return;
                }
            }
            if (MenuManager.ComboE["on"].Enabled && SpellManager.E.Ready)
            {
                if (MenuManager.ComboE["whenStunned"].Enabled)
                {
                    if (target.HasBuffOfType(BuffType.Stun))
                        SpellManager.E.Cast(target);
                }
                else
                    SpellManager.E.Cast(target);
            }
        }
        private static void Harass()
        {
            if (CastingE)
                return;
            var target = TargetSelector.GetTarget(600);
            if (target == null)
                return;
            var minManaPct = MenuManager.Harass["harassManaPct"].Value;
            if (MenuManager.Harass["harassQ"].Enabled && Player.ManaPercent() >= minManaPct && SpellManager.Q.Ready)
            {
                SpellManager.Q.Cast(target);
                return;
            }
            if (MenuManager.Harass["harassW"].Enabled && Player.ManaPercent() >= minManaPct && SpellManager.W.Ready)
            {
                SpellManager.W.Cast(target);
                return;
            }
            if (MenuManager.Harass["harassE"].Enabled && Player.ManaPercent() >= minManaPct && SpellManager.E.Ready)
                SpellManager.E.Cast(target);

        }
        private static void Killsteal()
        {
            if (MenuManager.Killsteal["ksQ"].Enabled && SpellManager.Q.Ready)
            {
                var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsInRange(600) && Utility.CanKillWithQ(x));
                if (target != null)
                    SpellManager.Q.Cast(target);
            }
        }
    }
}
