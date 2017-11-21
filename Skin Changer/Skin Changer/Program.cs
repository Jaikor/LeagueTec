using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Skin_Changer
{
    public class Program
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        private static Obj_AI_Hero[] Allies => ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsAlly && !x.IsMe && x.ChampionName != "PracticeTool_TargetDummy").ToArray();
        private static Obj_AI_Hero[] Enemies => ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.ChampionName != "PracticeTool_TargetDummy").ToArray();
        private static readonly Random Random = new Random();

        private static readonly int[] MinionSkinIDs = {0, 2, 4, 6, 8, 10, 12};
        private static readonly Dictionary<string, int> MinionSkins = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            //Minion Skins
            MinionSkins["Normal"] = 0;
            MinionSkins["Beach"] = 2;
            MinionSkins["Project"] = 4;
            MinionSkins["Christmas"] = 6;
            MinionSkins["Draven"] = 8;
            MinionSkins["Championship"] = 10;
            MinionSkins["Arcade"] = 12;
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static async void GameEventsOnGameStart()
        {
            Console.WriteLine("Skin Changer - Perplexity");
            MenuManager.Initialize();
            //Add self to menu
            var skins = await Utility.GetSkins(Player.ChampionName);
            MenuManager.Self = new Menu("self", "Self")
            {
                new MenuList("mySkin", "Champion Skin", skins, 0)
            };
            MenuManager.SelfWard = new Menu("selfWard", "Ward Skin");
            MenuManager.Self["mySkin"].OnValueChanged += (sender, args) => Game.OnUpdate += UpdateSkin;
            
            //Allies
            MenuManager.Allies = new Menu("allies", "Allies");
            foreach (var ally in Allies)
            {
                var championName = ally.ChampionName;
                skins = await Utility.GetSkins(championName);
                var menuList = new MenuList(championName, championName, skins, 0);
                menuList.OnValueChanged += (sender, args) => Game.OnUpdate += UpdateSkin;
                MenuManager.Allies.Add(menuList);
            }
            MenuManager.AllyMinions = new Menu("allyMinions", "Minions")
            {
                new MenuList("allyMinionSkin", "Skin", MinionSkins.Keys.Select(x => x.ToString()).ToArray(), 0),
                new MenuBool("allyMinionRandom", "Random", false)
            };
            MenuManager.Allies.Add(MenuManager.AllyMinions);
            //Enemies
            MenuManager.Enemies = new Menu("enemies", "Enemies");
            foreach (var enemy in Enemies)
            {
                var championName = enemy.ChampionName;
                skins = await Utility.GetSkins(championName);
                var menuList = new MenuList(championName, championName, skins, 0);
                menuList.OnValueChanged += (sender, args) => Game.OnUpdate += UpdateSkin;
                MenuManager.Enemies.Add(menuList);
            }
            MenuManager.EnemyMinions = new Menu("enemyMinions", "Minions")
            {
                new MenuList("enemyMinionSkin", "Skin", MinionSkins.Keys.Select(x => x.ToString()).ToArray(), 0),
                new MenuBool("enemyMinionRandom", "Random", false)
            };
            MenuManager.Enemies.Add(MenuManager.EnemyMinions);

            MenuManager.Finish();
            Game.OnUpdate += UpdateSkin;
            GameObject.OnCreate += GameObjectOnOnCreate;
            GameObject.OnRevive += GameObjectOnOnRevive; 
        }

        private static void GameObjectOnOnRevive(GameObject sender)
        {
            if (sender is Obj_AI_Hero hero)
            {
                if (hero.IsMe)
                {
                    var mySkin = MenuManager.Root["mySkin"].Value;
                    Player.SetSkinId(mySkin);
                }
                else if (hero.IsAlly)
                {
                    var selectedSkin = MenuManager.Allies[hero.ChampionName].Value;
                    hero.SetSkinId(selectedSkin);
                }
                else if (hero.IsEnemy)
                {
                    var selectedSkin = MenuManager.Enemies[hero.ChampionName].Value;
                    hero.SetSkinId(selectedSkin);
                }
            }
        }

        private static void GameObjectOnOnCreate(GameObject sender)
        {
            if (sender is Obj_AI_Minion minion && sender.Name.Contains("Minion"))
            {
                if (minion.IsAlly)
                {
                    MinionSkins.TryGetValue(MenuManager.AllyMinions["allyMinionSkin"].As<MenuList>().SelectedItem, out var allySkin);
                    if (MenuManager.AllyMinions["allyMinionRandom"].Enabled)
                        allySkin = MinionSkinIDs[Random.Next(0, 6)];
                    minion.SetSkinId(allySkin);
                }
                else
                {
                    MinionSkins.TryGetValue(MenuManager.EnemyMinions["enemyMinionSkin"].As<MenuList>().SelectedItem, out var enemySkin);
                    if (MenuManager.EnemyMinions["enemyMinionRandom"].Enabled)
                        enemySkin = MinionSkinIDs[Random.Next(0, 6)];
                    minion.SetSkinId(enemySkin);
                }
            }
        }

        private static void UpdateSkin()
        {
            //Self
            var mySkin = MenuManager.Root["mySkin"].Value;
            Player.SetSkinId(mySkin);
            //Allies
            if (Allies != null)
            {
                foreach (var ally in Allies)
                {
                    var selectedSkin = MenuManager.Allies[ally.ChampionName].Value;
                    ally.SetSkinId(selectedSkin);
                }
            }
            //Enemies
            if (Enemies != null)
            {
                foreach (var enemy in Enemies)
                {
                    var selectedSkin = MenuManager.Enemies[enemy.ChampionName].Value;
                    enemy.SetSkinId(selectedSkin);
                }
            }
            Game.OnUpdate -= UpdateSkin;
        }
    }
}
