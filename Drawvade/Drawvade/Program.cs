using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;

namespace Drawvade
{
    public class Program
    {
        private static Obj_AI_Hero Player;
        private static List<DetectedSkillshot> DetectedSkillshots;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            Player = ObjectManager.GetLocalPlayer();
            DetectedSkillshots = new List<DetectedSkillshot>();
            SpellDatabase.Initialize();

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;

            Game.OnUpdate += Game_OnUpdate;

            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDestroy += GameObject_OnDestroy;

            Render.OnPresent += Render_OnPresent;
        }

        private static void Game_OnUpdate()
        {
            DetectedSkillshots.RemoveAll(x => !x.IsActive);
            foreach (var skillshot in DetectedSkillshots)
                skillshot.Update();
            //foreach (var obj in ObjectManager.Get<Obj_AI_Base>())
            //{
            //    if(obj.IsAlly)
            //        Console.WriteLine(obj.Name);
            //}
        }

        private static void Render_OnPresent()
        {
            foreach (var skillshot in DetectedSkillshots)
            {
                switch (skillshot.SpellData.Type)
                {
                    case SkillshotType.Line:
                        var rectangle = new Geometry.Rectangle(skillshot.Start, skillshot.End, skillshot.SpellData.AddHitbox ? skillshot.SpellData.Radius + Player.BoundingRadius : skillshot.SpellData.Radius);
                        rectangle.ToPolygon().Draw(Color.White);
                        break;
                    case SkillshotType.Circle:
                        Render.Circle(skillshot.End, skillshot.SpellData.AddHitbox ? skillshot.SpellData.Radius + Player.BoundingRadius : skillshot.SpellData.Radius, 30, Color.White);
                        break;
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs e)
        {
            var startTime = Game.TickCount;
            if (sender == null || !sender.IsValid)
                return;
            if (!sender.IsEnemy)
                return;
            //Console.WriteLine($"Spell {e.SpellData.Name} casted. ({e.Sender.NetworkId}) (Speed: {e.SpellData.MissileSpeed}) (Range: {e.SpellData.CastRange} | {e.SpellData.CastRadius})");

            var spellData = SpellDatabase.GetByName(e.SpellData.Name);
            if (spellData == null)
                return;
            //Found a valid spell.
            var startPosition = e.Start;
            var endPosition = e.End;
            var direction = (endPosition - startPosition).To2D().Normalized();
            if (startPosition.Distance(endPosition) > spellData.Range || spellData.FixedRange)
            {
                endPosition = startPosition + direction.To3D() * spellData.Range;
            }
            if(spellData.Centered) //Only Thresh flay uses this.
            {
                endPosition = startPosition + direction.To3D() * spellData.Range;
                startPosition = startPosition - direction.To3D() * spellData.Range;
            }
            if(spellData.ExtraRange > 0)
                endPosition = endPosition + Math.Min(spellData.ExtraRange, spellData.Range - endPosition.Distance(sender.ServerPosition)) * direction.To3D();
            if (spellData.MultipleCount != -1)
            {
                for (var i = -(spellData.MultipleCount - 1) / 2; i <= (spellData.MultipleCount - 1) / 2; i++)
                {
                    var end = startPosition + spellData.Range * direction.Rotated(spellData.MultipleAngle * i).To3D();
                    DetectedSkillshots.Add(new DetectedSkillshot(spellData, startPosition, end, startTime, sender));
                }
                return;
            }
            //Handle special kinds of spells.
            switch (e.SpellData.Name)
            {
                case "MalzaharQ":
                    startPosition = endPosition - direction.Perpendicular().To3D() * 400;
                    endPosition = endPosition + direction.Perpendicular().To3D() * 400;
                    break;
                case "OrianaDetonateCommand":
                    var ball = GameObjects.AllyMinions.FirstOrDefault(x => Math.Abs(x.Health) > 0 && x.UnitSkinName.Equals("OriannaBall"));
                    if (ball != null)
                        endPosition = ball.ServerPosition;
                    break;
                case "OrianaRedactCommand":
                case "OrianaIzunaCommand":
                    ball = GameObjects.AllyMinions.FirstOrDefault(x => Math.Abs(x.Health) > 0 &&  x.UnitSkinName.Equals("OriannaBall"));
                    if (ball != null)
                        startPosition = ball.ServerPosition;
                    break;
                case "TaliyahR":
                    spellData.Range = 1500 + 1500 * sender.SpellBook.GetSpell(SpellSlot.R).Level;
                    break;
            }
            DetectedSkillshots.Add(new DetectedSkillshot(spellData, startPosition, endPosition, startTime, sender)); //Only gets added if it is a single skillshot.
        }

        private static void GameObject_OnCreate(GameObject sender)
        {
            var startTime = Game.TickCount;
            if (!(sender is MissileClient missile))
                return;
            if (!sender.IsEnemy)
                return;
            //Console.WriteLine($"Missile '{missile.SpellData.Name}' created. ({missile.NetworkId}) (Speed: {missile.SpellData.MissileSpeed})");
            var spellData = SpellDatabase.GetByMissile(missile.SpellData.Name);
            if (spellData == null)
                return;
            if (DetectedSkillshots.Any(x => x.SpellData.SpellName == spellData.SpellName))
                return; //This is so we don't add the missile right after casting, resulting in two drawings.
            var startPosition = missile.StartPosition;
            var endPosition = missile.EndPosition;
            var direction = (endPosition - startPosition).To2D().Normalized();
            if (spellData.MultipleCount != -1)
            {
                for (var i = -(spellData.MultipleCount - 1) / 2; i <= (spellData.MultipleCount - 1) / 2; i++)
                {
                    var end = startPosition + spellData.Range * direction.Rotated(spellData.MultipleAngle * i).To3D();
                    DetectedSkillshots.Add(new DetectedSkillshot(spellData, startPosition, end, startTime, missile.SpellCaster));
                }
                return;
            }
            DetectedSkillshots.Add(new DetectedSkillshot(spellData, startPosition, endPosition, startTime, missile.SpellCaster)); //Only gets added if it is a single missile.
        }

        private static void GameObject_OnDestroy(GameObject sender)
        {
            if (!(sender is MissileClient missile))
                return;
            //Console.WriteLine($"Destroy {missile.SpellData.Name} ({missile.NetworkId})");
            var spellData = SpellDatabase.GetByMissile(missile.SpellData.Name);
            if (spellData == null)
                return;
            var toDelete = DetectedSkillshots.Where(x => (x.SpellData.MissileName == missile.SpellData.Name || x.SpellData.ExtraMissileNames.Contains(missile.SpellData.Name)) && x.Caster == missile.SpellCaster).OrderBy(x => x.StartTime).FirstOrDefault();
            if (toDelete != null)
                DetectedSkillshots.Remove(toDelete);
        }
    }
}
