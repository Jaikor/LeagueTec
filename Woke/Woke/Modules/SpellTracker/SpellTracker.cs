using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Woke.Modules.SpellTracker
{
    internal class SpellTracker : IModule
    {
        private List<AChampion> Champions;
        public Menu Menu { get; set; }

        public void Load()
        {
            Console.WriteLine("Module 'SpellTracker' loading...");
            LoadMenu();
            Champions = new List<AChampion>();
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.ChampionName != "PracticeTool_TargetDummy"))
            {
                Champions.Add(new AChampion(hero));
            }
            Render.OnPresent += RenderOnOnPresent;
        }

        public void LoadMenu()
        {
            Menu = new Menu("spellTracker", "Spell Tracker")
            {
                new Menu("miniTracker", "Mini Tracker")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuSlider("xOffset", "X Offset", 0, 0, 1000),
                    new MenuBool("invertX", "Invert X"),
                    new MenuSlider("yOffset", "Y Offset", 0, 0, 1000),
                    new MenuBool("invertY", "Invert Y"),
                }.SetToolTip("Displays enemy champion information above minimap."),
                new MenuBool("self", "Track Self"),
                new MenuBool("ally", "Track Allies"),
                new MenuBool("enemy", "Track Enemies")
            };
            MenuManager.Root.Add(Menu);
        }

        private void RenderOnOnPresent()
        {
            //Mini tracker
            var enemies = Champions.Where(x => x.Champion.IsEnemy && !x.Champion.IsDead).ToList();
            if (Menu["miniTracker"]["on"].Enabled)
            {
                for (var i = 0; i < enemies.Count; i++)
                {
                    var champion = enemies[i];
                    var StartX = Render.Width - 48 * (i + 1) + (Menu["miniTracker"]["invertX"].Enabled ? -Menu["miniTracker"]["xOffset"].Value : Menu["miniTracker"]["xOffset"].Value);
                    var StartY = Render.Height - 300 + (Menu["miniTracker"]["invertY"].Enabled ? -Menu["miniTracker"]["yOffset"].Value : Menu["miniTracker"]["yOffset"].Value);
                    var championRect = new Aimtec.Rectangle(StartX, StartY, StartX + 48, StartY + 48);
                    champion.Texture.Draw(new Vector2(championRect.Left, championRect.Top));
                    var summoner1Rect = new Aimtec.Rectangle(championRect.Left, championRect.Top - 24, championRect.Left + 24, championRect.Top - 24);
                    var summoner2Rect = new Aimtec.Rectangle(summoner1Rect.Left + 24, summoner1Rect.Top, summoner1Rect.Left + 24 + 24, summoner1Rect.Top + 24 + 24);
                    foreach (var spell in champion.SummonerSpells)
                    {
                        if (spell.Spell.Slot == SpellSlot.Summoner1)
                        {
                            spell.LargeTexture.Draw(new Vector2(summoner1Rect.Left, summoner1Rect.Top));
                            if (!spell.Ready)
                            {
                                var timer = spell.TimeLeft < 1 ? $"{spell.TimeLeft:0.0}" : $"{Math.Ceiling(spell.TimeLeft)}";
                                Render.Rectangle(summoner1Rect.Left, summoner1Rect.Top, 24, 24, Color.FromArgb(150, 0, 0, 0));
                                Render.Text(timer, new Aimtec.Rectangle(summoner1Rect.Left, summoner1Rect.Top, summoner1Rect.Left + 24, summoner1Rect.Top + 24), RenderTextFlags.VerticalCenter | RenderTextFlags.HorizontalCenter, Color.White);
                            }
                        }
                        else if (spell.Spell.Slot == SpellSlot.Summoner2)
                        {
                            spell.LargeTexture.Draw(new Vector2(summoner2Rect.Left, summoner2Rect.Top));
                            if (!spell.Ready)
                            {
                                var timer = spell.TimeLeft < 1 ? $"{spell.TimeLeft:0.0}" : $"{Math.Ceiling(spell.TimeLeft)}";
                                Render.Rectangle(summoner2Rect.Left, summoner2Rect.Top, 24, 24, Color.FromArgb(150, 0, 0, 0));
                                Render.Text(timer, new Aimtec.Rectangle(summoner2Rect.Left, summoner2Rect.Top, summoner2Rect.Left + 24, summoner2Rect.Top + 24), RenderTextFlags.VerticalCenter | RenderTextFlags.HorizontalCenter, Color.White);
                            }
                        }
                    }
                    //Draw level
                    var levelRect = new Aimtec.Rectangle(championRect.Right - 15, championRect.Bottom - 15, championRect.Right, championRect.Bottom);
                    Render.Rectangle(new Vector2(levelRect.Left, levelRect.Top), 15, 15, Color.FromArgb(200, Color.Black));
                    Render.Text(champion.Champion.Level.ToString(), levelRect, RenderTextFlags.VerticalCenter | RenderTextFlags.HorizontalCenter, Color.White);
                    //Draw health
                    var healthRect = new Aimtec.Rectangle(championRect.Left, championRect.Top + 48, championRect.Left + 48, championRect.Top + 48 + 5);
                    var healthPct = champion.Champion.HealthPercent();
                    Render.Rectangle(new Vector2(healthRect.Left, healthRect.Top), 48 * (healthPct / 100), 5, Color.FromArgb(255, 35, 193, 26));

                    //Draw mana
                    var manaRect = new Aimtec.Rectangle(healthRect.Left, healthRect.Top + 5, healthRect.Left + 48, healthRect.Top + 5 + 5);
                    var manaPct = champion.Champion.ManaPercent();
                    Render.Rectangle(new Vector2(manaRect.Left, manaRect.Top), 48 * (manaPct / 100), 5, Color.FromArgb(255, 75, 155, 238));

                    //Health Separator Lines
                    Render.Line(new Vector2(healthRect.Left, healthRect.Top), new Vector2(healthRect.Left, healthRect.Top + 5), 1, true, Color.Black);
                    Render.Line(new Vector2(healthRect.Left, healthRect.Top + 5), new Vector2(healthRect.Left + 48, healthRect.Top + 5), 1, false, Color.Black);
                    //Mana Separator Lines
                    Render.Line(new Vector2(manaRect.Left, manaRect.Top), new Vector2(manaRect.Left, manaRect.Top + 5), 1, true, Color.Black);
                    Render.Line(new Vector2(manaRect.Left, manaRect.Top + 5), new Vector2(manaRect.Left + 48, manaRect.Top + 5), 1, false, Color.Black);
                }
            }
            foreach (var champion in Champions.Where(x => x.Champion.IsVisible && !x.Champion.IsDead))
            {
                if (champion.Champion.IsMe && !Menu["self"].Enabled)
                    continue;
                if (champion.Champion.IsAlly && !champion.Champion.IsMe && !Menu["ally"].Enabled)
                    continue;
                if(champion.Champion.IsEnemy && !Menu["enemy"].Enabled)
                    continue;
                var healthPos = champion.Champion.FloatingHealthBarPosition;
                //Draw champion spells.
                for (var i = 0; i < champion.ChampionSpells.Count; i++)
                {
                    var xOffset = Utility.GetChampionSpellXOffset(champion.Champion);
                    var yOffset = Utility.GetChampionSpellYOffset(champion.Champion);
                    var spell = champion.ChampionSpells[i];
                    var drawPos = healthPos + new Vector2((i * 32) + xOffset, yOffset);
                    spell.Texture.Draw(drawPos);
                    if (!spell.Ready)
                    {
                        var timer = spell.TimeLeft < 1 ? $"{spell.TimeLeft:0.0}" : $"{Math.Ceiling(spell.TimeLeft)}";
                        var rect = new Aimtec.Rectangle((int)drawPos.X, (int)drawPos.Y, (int)drawPos.X + 24, (int)drawPos.Y + 24);
                        Render.Rectangle(drawPos, 24, 24, Color.FromArgb(150, 0, 0, 0));
                        Render.Text(timer, rect, RenderTextFlags.VerticalCenter | RenderTextFlags.HorizontalCenter, Color.White);
                    }
                }
                //Draw summoner spells.
                for (var i = 0; i < champion.SummonerSpells.Count; i++)
                {
                    var spell = champion.SummonerSpells[i];
                    var xOffset = Utility.GetSummonerSpellXOffset(champion.Champion);
                    var yOffset = Utility.GetSummonerSpellYOffset(champion.Champion);
                    var drawPos = healthPos + new Vector2(xOffset, yOffset + i * 25);
                    spell.Texture.Draw(drawPos);
                    if (!spell.Ready)
                    {
                        var timer = spell.TimeLeft < 1 ? $"{spell.TimeLeft:0.0}" : $"{Math.Ceiling(spell.TimeLeft)}";
                        var rect = new Aimtec.Rectangle((int)drawPos.X, (int)drawPos.Y, (int)drawPos.X + 24, (int)drawPos.Y + 24);
                        Render.Rectangle(drawPos, 24, 24, Color.FromArgb(150, 0, 0, 0));
                        Render.Text(timer, rect, RenderTextFlags.VerticalCenter | RenderTextFlags.HorizontalCenter, Color.White);
                    }
                }
            }
        }
    }
}
