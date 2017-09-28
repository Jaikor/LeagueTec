using Aimtec;
using Aimtec.SDK.Extensions;
using System.Collections.Generic;
using System.Linq;
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Gangplank
{
    public static class BarrelManager
    {
        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        public static List<Barrel> Barrels;
        public static bool BarrelWillHit(Barrel barrel, Obj_AI_Base target)
        {
            return barrel.Object.Distance(target) <= SpellManager.ExplosionRadius;
        }
        public static bool BarrelWillHit(Barrel barrel, Vector3 position)
        {
            return barrel.Object.Distance(position) <= SpellManager.ExplosionRadius;
        }
        public static List<Obj_AI_Hero> GetEnemiesInChainRadius(Barrel barrel, bool outsideExplosionRadius = true)
        {
            if (outsideExplosionRadius)
                return GameObjects.EnemyHeroes.Where(x => x.IsValidTarget() && barrel.Object.Distance(x) <= SpellManager.ChainRadius + SpellManager.ExplosionRadius && barrel.Object.Distance(x) >= SpellManager.ExplosionRadius && x.Distance(Player) <= SpellManager.E.Range).ToList();
            else
                return GameObjects.EnemyHeroes.Where(x => x.IsValidTarget() && barrel.Object.Distance(x) <= SpellManager.ChainRadius + SpellManager.ExplosionRadius && x.Distance(Player) <= SpellManager.E.Range).ToList();
        }
        public static List<Barrel> GetBarrelsThatWillHit()
        {
            //Returns all barrels that can explode on an enemy.
            var barrelsWillHit = new List<Barrel>();
            foreach(var enemy in Utility.GetAllEnemiesInRange(float.MaxValue))
                barrelsWillHit.AddRange(GetBarrelsThatWillHit(enemy));
            return barrelsWillHit.Distinct().OrderBy(x => x.Object.Distance(Utility.Player)).ToList();
        }
        public static List<Barrel> GetBarrelsThatWillHit(Obj_AI_Base target)
        {
            return Barrels.Where(x => target.Distance(x.Object) <= SpellManager.ExplosionRadius).OrderBy(x => x.Object.Distance(Utility.Player)).ToList();
        }
        public static List<Barrel> GetChainedBarrels(Barrel barrel)
        {
            var barrels = new List<Barrel> {barrel};
            var currentBarrelId = barrel.NetworkId;
            while(true)
            {
                var barrelToAdd = Barrels.FirstOrDefault(x => !x.Object.IsDead && x.Object.Distance(barrel.Object) <= SpellManager.ChainRadius && x.NetworkId != currentBarrelId && !barrels.Contains(x));
                if (barrelToAdd != null)
                {
                    barrels.Add(barrelToAdd);
                    currentBarrelId = barrelToAdd.NetworkId;
                }
                else
                    break;
            }
            return barrels;
        }
        public static Barrel GetBestBarrelToQ(List<Barrel> barrels)
        {
            return barrels.Where(x => !x.Object.IsDead && x.CanQ && x.Object.IsInRange(SpellManager.Q.Range)).OrderBy(x => x.Created).FirstOrDefault();
        }
        public static Barrel GetNearestBarrel()
        {
            return Barrels.Where(x => !x.Object.IsDead && x.Object.Distance(Player) <= SpellManager.E.Range).OrderBy(x => x.Object.Distance(Utility.Player)).FirstOrDefault();
        }
        public static Barrel GetNearestBarrel(Vector3 position)
        {
            return Barrels.Where(x => !x.Object.IsDead && x.Object.Distance(Player) <= SpellManager.E.Range).OrderBy(x => x.Object.Distance(position)).FirstOrDefault();
        }
        public static Vector3 GetBestChainPosition(Obj_AI_Base target, Barrel barrel, bool usePred = true)
        {

            var input = SpellManager.E.GetPredictionInput(target);
            input.Delay = input.Delay * 2;
            var aa = Prediction.Instance.GetPrediction(input);
            if (barrel.Object.Distance(target) <= SpellManager.ChainRadius)
            {
                if (usePred)
                {
                    var pred = aa;
                    if(pred.HitChance >= HitChance.Medium)
                        return pred.UnitPosition;
                }
                    return target.ServerPosition;
            }
                    
            if (barrel.Object.Distance(target) <= SpellManager.ChainRadius + SpellManager.ExplosionRadius)
            {
                var pred = aa;
                var bestCastPos = barrel.ServerPosition.Extend(usePred && pred.HitChance >= HitChance.Medium ? pred.UnitPosition : target.ServerPosition, SpellManager.ChainRadius - 5);
                return bestCastPos;
            }
            return Vector3.Zero;
        }
        public static Vector3 GetBestChainPosition(Vector3 position, Barrel barrel)
        {
            if (barrel.Object.Distance(position) <= SpellManager.ChainRadius)
                return position;
            if (barrel.Object.Distance(position) <= SpellManager.ChainRadius + SpellManager.ExplosionRadius)
            {
                var bestCastPos = barrel.ServerPosition.Extend(position, SpellManager.ChainRadius - 5);
                return bestCastPos;
            }
            return Vector3.Zero;
        }
    }
}
