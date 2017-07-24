using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Drawvade
{
    public class Program
    {
        private static List<DetectedSkillshot> DetectedSkillshots;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            DetectedSkillshots = new List<DetectedSkillshot>();
            SpellDatabase.Initialize();
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDestroy += GameObject_OnDestroy;
            Render.OnPresent += Render_OnPresent;
        }
        private static void Render_OnPresent()
        {
            foreach(var skillshot in DetectedSkillshots)
            {
                var rectangle = new Geometry.Rectangle(skillshot.Start, skillshot.End, skillshot.SpellData.Radius);
                rectangle.ToPolygon().Draw(Color.White);
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            if (sender == null || !sender.IsValid)
                return;

            var spellData = SpellDatabase.GetByName(e.SpellData.Name);
            if (spellData != null)
            {
                //Found a valid spell.
                Console.WriteLine($"{spellData.ChampionName} : {spellData.SpellName} ({spellData.MissileName})");
                var startPosition = sender.ServerPosition;
                var endPosition = e.End;
                var direction = (endPosition - startPosition).Normalized();
                if (startPosition.Distance(endPosition) > spellData.Range || spellData.FixedRange)
                {
                    endPosition = startPosition + direction * spellData.Range;
                }
                DetectedSkillshots.Add(new DetectedSkillshot(spellData, startPosition, endPosition));
            }

            //if(sender.IsMe)
            //    Console.WriteLine($"{e.SpellData.Name}");
        }

        private static void GameObject_OnCreate(GameObject sender)
        {
            //if (sender is MissileClient a)
                //Console.WriteLine($"Missile '{a.SpellData.Name}' created.");
        }

        private static void GameObject_OnDestroy(GameObject sender)
        {
            if(sender is MissileClient missile)
            {
                //Console.WriteLine($"Destroy {missile.SpellData.Name}");
                var spellData = SpellDatabase.GetByMissile(missile.SpellData.Name);
                if (spellData != null)
                {
                    Console.WriteLine($"{spellData.MissileName}");
                    DetectedSkillshots.RemoveAll(x => x.SpellData.MissileName == missile.SpellData.Name);
                }
            }
        }
    }
}
