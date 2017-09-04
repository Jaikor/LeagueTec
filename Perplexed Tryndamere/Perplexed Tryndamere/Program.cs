using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Threading;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Tryndamere
{
    public class Program
    {
        private static Obj_AI_Hero Player => GameObjects.Player;
        private static int LastUltTime;
        private static int UltCounter => Game.TickCount - LastUltTime;
        private static bool IsUlting => Player.HasBuff("UndyingRage") || UltCounter < 5000;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            if (Player.ChampionName != "Tryndamere")
                return;

            LastUltTime = Game.TickCount;

            MenuManager.Initialize();
            SpellManager.Initialize();

            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;

            BuffManager.OnRemoveBuff += BuffManagerOnOnRemoveBuff;
        }

        private static void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender.IsMe && e.SpellSlot == SpellSlot.R)
                LastUltTime = Game.TickCount;
        }

        private static void BuffManagerOnOnRemoveBuff(Obj_AI_Base target, Buff buff)
        {
            if (target.IsMe && buff.Name == "UndyingRage" && MenuManager.QMenu["qAfterUlt"].Enabled && SpellManager.Q.Ready)
                SpellManager.Q.Cast();
        }

        private static void Game_OnUpdate()
        {
            if (MenuManager.QMenu["autoQ"].Enabled)
                AutoQ();
            if (MenuManager.RMenu["autoR"].Enabled)
                AutoR();
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo();
                    break;
            }
        }

        private static void Combo()
        {
            if (MenuManager.RMenu["autoR"].Enabled)
            {
                var rPercent = MenuManager.RMenu["rPercent"].Value;
                if (Player.HealthPercent() <= rPercent)
                {
                    if (MenuManager.RMenu["rMode"].Value == 1 && SpellManager.R.Ready) //Only if mode is combo.
                        SpellManager.R.Cast();
                }
            }
            var target = TargetSelector.GetTarget(SpellManager.W.Range);
            if (target == null || !target.IsValidTarget())
                return;
            if(MenuManager.Combo["w"].Enabled && SpellManager.W.Ready)
            {
                if(MenuManager.Combo["wMode"].Value == 0) //Always cast
                    SpellManager.W.Cast();
                else //When facing away
                {
                    if (!target.IsFacingUnit(Player))
                        SpellManager.W.Cast();
                }
                return;
            }
            if(MenuManager.Combo["e"].Enabled && target.Distance(Player) <= MenuManager.Combo["eDistance"].Value && SpellManager.E.Ready)
            {
                var castPos = target.ServerPosition.Extend(Player.ServerPosition, -100);
                SpellManager.E.Cast(castPos);
            }
        }

        private static void AutoQ()
        {
            if (IsUlting)
                return;
            var qPercent = MenuManager.QMenu["qPercent"].Value;
            if(Player.HealthPercent() <= qPercent)
            {
                if (MenuManager.QMenu["dontQHaveUlt"].Enabled && SpellManager.R.Ready)
                    return;
                SpellManager.Q.Cast();
            }
        }

        private static void AutoR()
        {
            var rPercent = MenuManager.RMenu["rPercent"].Value;
            if (Player.HealthPercent() <= rPercent)
            {
                if (MenuManager.RMenu["rMode"].Value == 0 && SpellManager.R.Ready) //Only if mode is always.
                    SpellManager.R.Cast();
            }
        }

        private static void Render_OnPresent()
        {
            if(MenuManager.Drawing["drawW"].Enabled)
                Render.Circle(Player.ServerPosition, SpellManager.W.Range, 50, Color.White);
            if(MenuManager.Drawing["drawE"].Enabled)
                Render.Circle(Player.ServerPosition, MenuManager.Combo["eDistance"].Value, 50, Color.Blue);
            if(MenuManager.Drawing["drawR"].Enabled)
            {
                if(IsUlting)
                {
                    double secondsLeft = ((5000d - UltCounter) / 1000d);
                    Render.Text(Player.ServerPosition.ToScreenPosition(), Color.Red, $"Ult Timer: {secondsLeft:0.##}");
                }
            }
        }
    }
}
