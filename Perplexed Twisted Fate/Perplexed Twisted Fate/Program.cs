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

namespace Perplexed_Twisted_Fate
{
    class Program
    {
        private static Obj_AI_Hero Player;
        private static Card CardChoice = Card.Gold;
        private static float LastPickTime;
        private static string WCard => Player.SpellBook.GetSpell(SpellSlot.W).Name;
        private static bool PickingCard => WCard != "PickACard";
        private static bool CanPick => Game.TickCount - LastPickTime >= 500 && !PickingCard;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            Player = ObjectManager.GetLocalPlayer();
            if (Player.ChampionName != "TwistedFate")
                return;

            MenuManager.Initialize();
            SpellManager.Initialize();


            LastPickTime = Game.TickCount;
            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += Render_OnPresent;
            BuffManager.OnAddBuff += BuffManager_OnAddBuff;
        }

        private static void BuffManager_OnAddBuff(Obj_AI_Base sender, Buff buff)
        {
            if (sender.IsMe && buff.Name == "Gate" && MenuManager.Misc["ultGold"].Enabled)
                PickCard(Card.Gold);
        }

        private static void Game_OnUpdate()
        {
            Killsteal();
            AutoQCCdEnemies();

            if (Player.HasBuff("pickacard_tracker"))
            {
                switch (WCard)
                {
                    case "BlueCardLock":
                        if (CardChoice == Card.Blue)
                            SpellManager.W.Cast();
                        break;
                    case "RedCardLock":
                        if (CardChoice == Card.Red)
                            SpellManager.W.Cast();
                        break;
                    case "GoldCardLock":
                        if (CardChoice == Card.Gold)
                            SpellManager.W.Cast();
                        break;
                }
            }
            if (MenuManager.PickACard["pickBlue"].Enabled && SpellManager.W.Ready && CanPick)
            {
                PickCard(Card.Blue);
                return;
            }
            if (MenuManager.PickACard["pickRed"].Enabled && SpellManager.W.Ready && CanPick)
            {
                PickCard(Card.Red);
                return;
            }
            if (MenuManager.PickACard["pickGold"].Enabled && SpellManager.W.Ready && CanPick)
            {
                PickCard(Card.Gold);
                return;
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
            if (MenuManager.Combo["comboW"].Enabled && SpellManager.W.Ready && CanPick)
            {
                var cardIndex = MenuManager.Combo["comboCard"].As<MenuList>().Value;
                PickCard((Card)cardIndex);
            }
            if (MenuManager.Combo["comboQ"].Enabled && SpellManager.Q.Ready)
            {
                var target = TargetSelector.GetTarget(SpellManager.Q.Range);
                if (target.IsValidTarget())
                {
                    if (MenuManager.Combo["comboQCC"].Enabled)
                    {
                        if (target.IsCCd())
                        {
                            var pred = SpellManager.Q.GetPrediction(target);
                            if (pred.HitChance >= HitChance.Medium)
                                SpellManager.Q.Cast(pred.UnitPosition);
                        }
                        return;
                    }
                    else
                    {
                        var pred = SpellManager.Q.GetPrediction(target);
                        if (pred.HitChance >= HitChance.High)
                            SpellManager.Q.Cast(pred.UnitPosition);
                    }
                }
            }
        }

        private static void Harass()
        {
            if (MenuManager.Harass["harassQ"].Enabled && SpellManager.Q.Ready)
            {
                var minManaPct = MenuManager.Harass["harassManaPct"].Value;
                if(Player.ManaPercent() >= minManaPct)
                {
                    var target = TargetSelector.GetTarget(SpellManager.Q.Range);
                    var pred = SpellManager.Q.GetPrediction(target);
                    if (pred.HitChance >= HitChance.High)
                        SpellManager.Q.Cast(pred.UnitPosition);
                }
            }
        }

        private static void Killsteal()
        {
            if (MenuManager.Killsteal["stealQ"].Enabled && SpellManager.Q.Ready)
            {
                var target = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.IsInRange(SpellManager.Q.Range) && Player.GetSpellDamage(x, SpellSlot.Q) > x.Health && x.IsValidTarget()).OrderBy(x => x.Distance(Player)).FirstOrDefault();
                if (target != null)
                {
                    var pred = SpellManager.Q.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Medium)
                        SpellManager.Q.Cast(pred.UnitPosition);
                }
            }
        }

        private static void AutoQCCdEnemies()
        {
            if(MenuManager.Misc["autoQ"].Enabled && SpellManager.Q.Ready)
            {
                var target = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.IsInRange(SpellManager.Q.Range) && x.IsCCd() && x.IsValidTarget()).OrderBy(x => x.Distance(Player)).FirstOrDefault();
                if (target != null)
                {
                    var pred = SpellManager.Q.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Medium)
                        SpellManager.Q.Cast(pred.UnitPosition);
                }
            }
        }

        private static void Render_OnPresent()
        {
            if (MenuManager.Drawing["drawQ"].Enabled)
                Render.Circle(Player.Position, SpellManager.Q.Range, 30, Color.Cyan);
        }

        private static void PickCard(Card card)
        {
            switch (card)
            {
                case Card.Blue:
                    CardChoice = Card.Blue;
                    LastPickTime = Game.TickCount;
                    SpellManager.W.Cast();
                    break;
                case Card.Red:
                    CardChoice = Card.Red;
                    LastPickTime = Game.TickCount;
                    SpellManager.W.Cast();
                    break;
                case Card.Gold:
                    CardChoice = Card.Gold;
                    LastPickTime = Game.TickCount;
                    SpellManager.W.Cast();
                    break;
            }
        }
    }
}
