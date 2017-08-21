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

namespace URF_Spammer
{
    public static class Program
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        private static void Main(string[] args)
        {
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static void GameEventsOnGameStart()
        {
            MenuManager.Initialize();
            SpellManager.Initialize();
            Game.OnUpdate += GameOnOnUpdate;
        }

        private static void GameOnOnUpdate()
        {
            if (Player.IsRecalling())
                return;
            if (MenuManager.QMenu["spamQ"].Enabled && SpellManager.Q.Ready)
            {
                var castOn = MenuManager.QMenu["castOn"].Value;
                var target = TargetSelector.GetSelectedTarget() ?? TargetSelector.GetTarget(1000);
                if(castOn == 3) //Mouse
                {
                    if (MenuManager.QMenu["comboOnly"].Enabled)
                    {
                        if(Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.Q.Cast(Game.CursorPos);
                    }
                    else
                    {
                        SpellManager.Q.Cast(Game.CursorPos);
                    }
                    return;
                }
                switch (castOn)
                {
                    case 0: //Self
                        target = Player;
                        break;
                    case 1: //Ally
                        target = GameObjects.AllyHeroes.OrderBy(x => x.Distance(Player)).FirstOrDefault();
                        break;
                    case 2: //Enemy
                        target = TargetSelector.GetTarget(1000);
                        break;
                }
                if (target != null)
                {
                    if (MenuManager.QMenu["comboOnly"].Enabled)
                    {
                        if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.Q.Cast(target);
                    }
                    else
                    {
                        SpellManager.Q.Cast(target);
                    }
                }
            }
            if (MenuManager.WMenu["spamW"].Enabled && SpellManager.W.Ready)
            {
                var castOn = MenuManager.WMenu["castOn"].Value;
                var target = TargetSelector.GetSelectedTarget() ?? TargetSelector.GetTarget(1000);
                if (castOn == 3) //Mouse
                {
                    if (MenuManager.WMenu["comboOnly"].Enabled)
                    {
                        if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.W.Cast(Game.CursorPos);
                    }
                    else
                    {
                        SpellManager.W.Cast(Game.CursorPos);
                    }
                    return;
                }
                switch (castOn)
                {
                    case 0: //Self
                        target = Player;
                        break;
                    case 1: //Ally
                        target = GameObjects.AllyHeroes.OrderBy(x => x.Distance(Player)).FirstOrDefault();
                        break;
                    case 2: //Enemy
                        target = TargetSelector.GetTarget(1000);
                        break;
                }
                if (target != null)
                {
                    if (MenuManager.WMenu["comboOnly"].Enabled)
                    {
                        if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.W.Cast(target);
                    }
                    else
                    {
                        SpellManager.W.Cast(target);
                    }
                }
            }
            if (MenuManager.EMenu["spamE"].Enabled && SpellManager.E.Ready)
            {
                var castOn = MenuManager.EMenu["castOn"].Value;
                var target = TargetSelector.GetSelectedTarget() ?? TargetSelector.GetTarget(1000);
                if (castOn == 3) //Mouse
                {
                    if (MenuManager.EMenu["comboOnly"].Enabled)
                    {
                        if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.E.Cast(Game.CursorPos);
                    }
                    else
                    {
                        SpellManager.E.Cast(Game.CursorPos);
                    }
                    return;
                }
                switch (castOn)
                {
                    case 0: //Self
                        target = Player;
                        break;
                    case 1: //Ally
                        target = GameObjects.AllyHeroes.OrderBy(x => x.Distance(Player)).FirstOrDefault();
                        break;
                    case 2: //Enemy
                        target = TargetSelector.GetTarget(1000);
                        break;
                }
                if (target != null)
                {
                    if (MenuManager.EMenu["comboOnly"].Enabled)
                    {
                        if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.E.Cast(target);
                    }
                    else
                    {
                        SpellManager.E.Cast(target);
                    }
                }
            }
            if (MenuManager.RMenu["spamR"].Enabled && SpellManager.R.Ready)
            {
                var castOn = MenuManager.RMenu["castOn"].Value;
                var target = TargetSelector.GetSelectedTarget() ?? TargetSelector.GetTarget(1000);
                if (castOn == 3) //Mouse
                {
                    if (MenuManager.RMenu["comboOnly"].Enabled)
                    {
                        if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.R.Cast(Game.CursorPos);
                    }
                    else
                    {
                        SpellManager.R.Cast(Game.CursorPos);
                    }
                    return;
                }
                switch (castOn)
                {
                    case 0: //Self
                        target = Player;
                        break;
                    case 1: //Ally
                        target = GameObjects.AllyHeroes.OrderBy(x => x.Distance(Player)).FirstOrDefault();
                        break;
                    case 2: //Enemy
                        target = TargetSelector.GetTarget(1000);
                        break;
                }
                if (target != null)
                {
                    if (MenuManager.RMenu["comboOnly"].Enabled)
                    {
                        if (Orbwalker.Implementation.Mode == OrbwalkingMode.Combo)
                            SpellManager.R.Cast(target);
                    }
                    else
                    {
                        SpellManager.R.Cast(target);
                    }
                }
            }
        }
    }
}
