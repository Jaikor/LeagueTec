using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawvade
{
    public static class SpellDatabase
    {
        public static List<SpellData> Spells = new List<SpellData>();

        public static void Initialize()
        {
            #region Aatrox
            Spells.Add(new SpellData
            {
                ChampionName = "Aatrox",
                SpellName = "AatroxQ",
                Delay = 600,
                Range = 650,
                Radius = 250,
                MissileSpeed = 2000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Aatrox",
                SpellName = "AatroxE",
                MissileName = "AatroxEConeMissile",
                Delay = 250,
                Range = 1075,
                Radius = 35,
                MissileSpeed = 1250,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Ahri
            Spells.Add(new SpellData
            {
                ChampionName = "Ahri",
                SpellName = "AhriOrbofDeception",
                MissileName = "AhriOrbMissile",
                Delay = 250,
                Range = 1000,
                Radius = 100,
                MissileSpeed = 2500,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Ahri",
                SpellName = "AhriOrbofDeceptionReturn",
                MissileName = "AhriOrbReturn",
                Delay = 250,
                Range = 1000,
                Radius = 100,
                MissileSpeed = 2500,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true,
                IsReturnSpell = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Ahri",
                SpellName = "AhriSeduce",
                MissileName = "AhriSeduceMissile",
                Delay = 250,
                Range = 1000,
                Radius = 60,
                MissileSpeed = 1550,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Amumu
            Spells.Add(new SpellData
            {
                ChampionName = "Amumu",
                SpellName = "BandageToss",
                MissileName = "SadMummyBandageToss",
                Delay = 250,
                Range = 1100,
                Radius = 90,
                MissileSpeed = 2000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Anivia
            Spells.Add(new SpellData
            {
                ChampionName = "Anivia",
                SpellName = "FlashFrost",
                MissileName = "FlashFrostSpell",
                Delay = 250,
                Range = 1100,
                Radius = 110,
                MissileSpeed = 850,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Ashe
            //Spells.Add(new SpellData
            //{
            //    ChampionName = "Ashe",
            //    SpellName = "Volley",
            //    MissileName = "VolleyAttack",
            //    Delay = 250,
            //    Range = 1250,
            //    Radius = 60,
            //    MultipleCount = 9,
            //    MultipleAngle = 4.62f * (float)Math.PI / 180,
            //    MissileSpeed = 1500,
            //    Slot = SpellSlot.W,
            //    Type = SkillshotType.Line,
            //    FixedRange = true,
            //    AddHitbox = true
            //});
            Spells.Add(new SpellData
            {
                ChampionName = "Ashe",
                SpellName = "EnchantedCrystalArrow",
                MissileName = "EnchantedCrystalArrow",
                Delay = 250,
                Range = 20000,
                Radius = 130,
                MissileSpeed = 1600,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Aurelion Sol
            Spells.Add(new SpellData
            {
                ChampionName = "AurelionSol",
                SpellName = "AurelionSolR",
                MissileName = "AurelionSolRBeamMissile",
                Delay = 300,
                Range = 1420,
                Radius = 120,
                MissileSpeed = 4500,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Bard
            Spells.Add(new SpellData
            {
                ChampionName = "Bard",
                SpellName = "BardQ",
                MissileName = "BardQMissile",
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1600,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Bard",
                SpellName = "BardR",
                MissileName = "BardR",
                Delay = 500,
                Range = 3400,
                Radius = 350,
                MissileSpeed = 2100,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Blitzcrank
            Spells.Add(new SpellData
            {
                ChampionName = "Blitzcrank",
                SpellName = "RocketGrab",
                MissileName = "RocketGrabMissile",
                Delay = 250,
                Range = 1050,
                Radius = 70,
                MissileSpeed = 1800,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Brand
            Spells.Add(new SpellData
            {
                ChampionName = "Brand",
                SpellName = "BrandQ",
                MissileName = "BrandQMissile",
                Delay = 250,
                Range = 1100,
                Radius = 60,
                MissileSpeed = 1600,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Brand",
                SpellName = "BrandW",
                Delay = 850,
                Range = 900,
                Radius = 260,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = false
            });
            #endregion
            #region Braum
            Spells.Add(new SpellData
            {
                ChampionName = "Braum",
                SpellName = "BraumQ",
                MissileName = "BraumQMissile",
                Delay = 250,
                Range = 1050,
                Radius = 60,
                MissileSpeed = 1700,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Braum",
                SpellName = "BraumRWrapper",
                MissileName = "BraumRMissile",
                Delay = 500,
                Range = 1200,
                Radius = 115,
                MissileSpeed = 1400,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Caitlyn
            Spells.Add(new SpellData
            {
                ChampionName = "Caitlyn",
                SpellName = "CaitlynPiltoverPeacemaker",
                MissileName = "CaitlynPiltoverPeacemaker",
                Delay = 625,
                Range = 1300,
                Radius = 90,
                MissileSpeed = 2200,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Caitlyn",
                SpellName = "CaitlynEntrapment",
                MissileName = "CaitlynEntrapmentMissile",
                Delay = 125,
                Range = 1000,
                Radius = 70,
                MissileSpeed = 1600,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Camille
            Spells.Add(new SpellData
            {
                ChampionName = "Camille",
                SpellName = "CamilleE",
                MissileName = "CamilleEMissile",
                Delay = 250,
                Range = 1000,
                Radius = 60,
                MissileSpeed = 1400,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = false
            });
            #endregion
            #region Cassiopeia
            Spells.Add(new SpellData
            {
                ChampionName = "Cassiopeia",
                SpellName = "CassiopeiaQ",
                Delay = 600,
                Range = 825,
                Radius = 160,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = false
            });
            #endregion
            #region Cho'Gath
            Spells.Add(new SpellData
            {
                ChampionName = "ChoGath",
                SpellName = "Rupture",
                Delay = 1200,
                Range = 960,
                Radius = 250,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Corki
            Spells.Add(new SpellData
            {
                ChampionName = "Corki",
                SpellName = "PhosphorusBomb",
                MissileName = "PhosphorusBombMissile",
                Delay = 300,
                Range = 825,
                Radius = 250,
                MissileSpeed = 1000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Corki",
                SpellName = "MissileBarrage",
                MissileName = "MissileBarrageMissile",
                Delay = 200,
                Range = 1300,
                Radius = 40,
                MissileSpeed = 2000,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Corki",
                SpellName = "MissileBarrage2",
                MissileName = "MissileBarrageMissile2",
                Delay = 200,
                Range = 1500,
                Radius = 40,
                MissileSpeed = 2000,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Darius
            Spells.Add(new SpellData
            {
                ChampionName = "Darius",
                SpellName = "DariusCleave",
                Delay = 750,
                Range = 0,
                Radius = 375,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = true,
                AddHitbox = true,
                FollowCaster = true
            });
            #endregion
            #region Dr. Mundo
            Spells.Add(new SpellData
            {
                ChampionName = "DrMundo",
                SpellName = "InfectedCleaverMissile",
                MissileName = "InfectedCleaverMissile",
                Delay = 250,
                Range = 1050,
                Radius = 60,
                MissileSpeed = 2000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Draven
            Spells.Add(new SpellData
            {
                ChampionName = "Draven",
                SpellName = "DravenDoubleShot",
                MissileName = "DravenDoubleShotMissile",
                Delay = 250,
                Range = 1100,
                Radius = 130,
                MissileSpeed = 1400,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Draven",
                SpellName = "DravenRCast",
                MissileName = "DravenR",
                Delay = 400,
                Range = 20000,
                Radius = 160,
                MissileSpeed = 2000,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Ekko
            Spells.Add(new SpellData
            {
                ChampionName = "Ekko",
                SpellName = "EkkoQ",
                MissileName = "EkkoQMis",
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1650,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Ekko",
                SpellName = "EkkoW",
                MissileName = "EkkoW",
                Delay = 3750,
                Range = 1600,
                Radius = 375,
                MissileSpeed = 1650,
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = false
            });
            #endregion
            #region Elise
            Spells.Add(new SpellData
            {
                ChampionName = "Elise",
                SpellName = "EliseHumanE",
                MissileName = "EliseHumanE",
                Delay = 250,
                Range = 1100,
                Radius = 55,
                MissileSpeed = 1600,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Ezreal
            Spells.Add(new SpellData
            {
                ChampionName = "Ezreal",
                SpellName = "EzrealMysticShot",
                MissileName = "EzrealMysticShotMissile",
                Delay = 250,
                Range = 1150,
                Radius = 60,
                MissileSpeed = 2000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Ezreal",
                SpellName = "EzrealEssenceFlux",
                MissileName = "EzrealEssenceFluxMissile",
                Delay = 250,
                Range = 1000,
                Radius = 80,
                MissileSpeed = 1550,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Ezreal",
                SpellName = "EzrealTrueshotBarrage",
                MissileName = "EzrealTrueshotBarrage",
                Delay = 1000,
                Range = 20000,
                Radius = 160,
                MissileSpeed = 2000,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Fiora
            Spells.Add(new SpellData
            {
                ChampionName = "Fiora",
                SpellName = "FioraW",
                MissileName = "FioraWMissile",
                Delay = 500,
                Range = 750,
                Radius = 70,
                MissileSpeed = 3200,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Fizz
            Spells.Add(new SpellData
            {
                ChampionName = "Fizz",
                SpellName = "FizzR",
                MissileName = "FizzRMissile",
                Delay = 250,
                Range = 1275,
                Radius = 110,
                MissileSpeed = 1300,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Galio
            Spells.Add(new SpellData
            {
                ChampionName = "Galio",
                SpellName = "GalioQ",
                MissileName = "GalioQMissile",
                Delay = 250,
                Range = 825,
                Radius = 250,
                MissileSpeed = 500,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = false
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Galio",
                SpellName = "GalioE",
                Delay = 250,
                Range = 600,
                Radius = 150,
                MissileSpeed = 500,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Gnar
            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarQ",
                MissileName = "GnarQMissile",
                Delay = 250,
                Range = 1125,
                Radius = 60,
                MissileSpeed = 2500,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarQReturn",
                MissileName = "GnarQMissileReturn",
                Delay = 0,
                Range = 2500,
                Radius = 75,
                MissileSpeed = 1000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarBigQ",
                MissileName = "GnarBigQMissile",
                Delay = 500,
                Range = 1150,
                Radius = 90,
                MissileSpeed = 2100,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                SpellName = "GnarBigW",
                Delay = 600,
                Range = 600,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Gragas
            Spells.Add(new SpellData
            {
                ChampionName = "Gragas",
                SpellName = "GragasQ",
                MissileName = "GragasQMissile",
                Delay = 250,
                Range = 1100,
                Radius = 275,
                MissileSpeed = 1300,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = false
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Gragas",
                SpellName = "GragasE",
                Delay = 0,
                Range = 950,
                ExtraRange = 300,
                Radius = 200,
                MissileSpeed = 1200,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Gragas",
                SpellName = "GragasR",
                MissileName = "GragasRBoom",
                Delay = 250,
                Range = 1050,
                Radius = 375,
                MissileSpeed = 1800,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Graves
            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesQLineSpell",
                MissileName = "GravesQLineMis",
                Delay = 250,
                Range = 800,
                Radius = 40,
                MissileSpeed = 3000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesQReturn",
                MissileName = "GravesQReturn",
                Delay = 0,
                Range = 800,
                Radius = 40,
                MissileSpeed = 3000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesSmokeGrenade",
                MissileName = "GravesSmokeGrenadeBoom",
                Delay = 250,
                Range = 800,
                Radius = 250,
                MissileSpeed = 3000,
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = false
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                SpellName = "GravesChargeShot",
                MissileName = "GravesChargeShotShot",
                Delay = 250,
                Range = 1100,
                Radius = 100,
                MissileSpeed = 2100,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Illaoi
            Spells.Add(new SpellData
            {
                ChampionName = "Illaoi",
                SpellName = "IllaoiQ",
                Delay = 750,
                Range = 850,
                Radius = 100,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Illaoi",
                SpellName = "IllaoiE",
                MissileName = "IllaoiEMis",
                Delay = 250,
                Range = 950,
                Radius = 50,
                MissileSpeed = 1900,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Illaoi",
                SpellName = "IllaoiR",
                Delay = 500,
                Range = 0,
                Radius = 450,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = true,
                AddHitbox = false
            });
            #endregion
            #region Irelia
            Spells.Add(new SpellData
            {
                ChampionName = "Irelia",
                SpellName = "IreliaTranscendentBlades",
                MissileName = "IreliaTranscendentBlades",
                Delay = 0,
                Range = 1200,
                Radius = 65,
                MissileSpeed = 1600,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Ivern
            Spells.Add(new SpellData
            {
                ChampionName = "Ivern",
                SpellName = "IvernQ",
                MissileName = "IvernQ",
                Delay = 250,
                Range = 1075,
                Radius = 65,
                MissileSpeed = 1300,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Janna
            Spells.Add(new SpellData
            {
                ChampionName = "Janna",
                SpellName = "JannaQ",
                MissileName = "HowlingGaleSpell",
                Delay = 250,
                Range = 1700,
                Radius = 120,
                MissileSpeed = 900,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region JarvanIV
            Spells.Add(new SpellData
            {
                ChampionName = "JarvanIV",
                SpellName = "JarvanIVDragonStrike",
                Delay = 600,
                Range = 770,
                Radius = 70,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "JarvanIV",
                SpellName = "JarvanIVDemacianStandard",
                Delay = 500,
                Range = 860,
                Radius = 175,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Jayce
            Spells.Add(new SpellData
            {
                ChampionName = "Jayce",
                SpellName = "JayceShockBlast",
                MissileName = "JayceShockBlastMis",
                Delay = 250,
                Range = 1150,
                Radius = 70,
                MissileSpeed = 1450,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Jayce",
                SpellName = "JayceShockBlastAccelerated",
                MissileName = "JayceShockBlastWallMis",
                Delay = 250,
                Range = 1550,
                Radius = 70,
                MissileSpeed = 2350,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Jhin
            Spells.Add(new SpellData
            {
                ChampionName = "Jhin",
                SpellName = "JhinW",
                Delay = 750,
                Range = 2550,
                Radius = 40,
                MissileSpeed = 5000,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Jhin",
                SpellName = "JhinRShot",
                MissileName = "JhinRShotMis",
                Delay = 250,
                Range = 3500,
                Radius = 80,
                MissileSpeed = 5000,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Jinx
            Spells.Add(new SpellData
            {
                ChampionName = "Jinx",
                SpellName = "JinxW",
                MissileName = "JinxWMissile",
                Delay = 600,
                Range = 1500,
                Radius = 60,
                MissileSpeed = 3300,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Jinx",
                SpellName = "JinxR",
                MissileName = "JinxR",
                Delay = 600,
                Range = 20000,
                Radius = 140,
                MissileSpeed = 1700,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Kalista
            Spells.Add(new SpellData
            {
                ChampionName = "Kalista",
                SpellName = "KalistaMysticShot",
                MissileName = "KalistaMysticShotMisTrue",
                Delay = 250,
                Range = 1200,
                Radius = 40,
                MissileSpeed = 1700,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Karma
            Spells.Add(new SpellData
            {
                ChampionName = "Karma",
                SpellName = "KarmaQ",
                MissileName = "KarmaQMissile",
                Delay = 250,
                Range = 1050,
                Radius = 60,
                MissileSpeed = 1700,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Karthus
            Spells.Add(new SpellData
            {
                ChampionName = "Karthus",
                SpellName = "KarthusLayWasteA1",
                Delay = 1000,
                Range = 875,
                Radius = 160,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Kassadin
            Spells.Add(new SpellData
            {
                ChampionName = "Kassadin",
                SpellName = "RiftWalk",
                Delay = 250,
                Range = 500,
                Radius = 270,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Kayn
            Spells.Add(new SpellData
            {
                ChampionName = "Kayn",
                SpellName = "KaynW",
                Delay = 800,
                Range = 750,
                Radius = 80,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Kennen
            Spells.Add(new SpellData
            {
                ChampionName = "Kennen",
                SpellName = "KennenShurikenHurlMissile1",
                MissileName = "KennenShurikenHurlMissile1",
                Delay = 125,
                Range = 1050,
                Radius = 50,
                MissileSpeed = 1700,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Kha'Zix
            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                SpellName = "KhazixW",
                MissileName = "KhazixWMissile",
                Delay = 250,
                Range = 1025,
                Radius = 73,
                MissileSpeed = 1700,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                SpellName = "KhazixWLong",
                MissileName = "KhazixWMissile",
                Delay = 250,
                Range = 1025,
                Radius = 73,
                MultipleCount = 3,
                MultipleAngle = 22f * (float)Math.PI / 180,
                MissileSpeed = 1700,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                SpellName = "KhazixE",
                Delay = 250,
                Range = 600,
                Radius = 300,
                MissileSpeed = 1500,
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                SpellName = "KhazixELong",
                Delay = 250,
                Range = 800,
                Radius = 300,
                MissileSpeed = 1500,
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });

            #endregion
            #region Kled
            Spells.Add(new SpellData
            {
                ChampionName = "Kled",
                SpellName = "KledQ",
                MissileName = "KledQMissile",
                Delay = 250,
                Range = 800,
                Radius = 45,
                MissileSpeed = 1600,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Kled",
                SpellName = "KledE",
                Delay = 0,
                Range = 750,
                Radius = 125,
                MissileSpeed = 945,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Kled",
                SpellName = "KledRiderQ",
                MissileName = "KledRiderQMissile",
                Delay = 250,
                Range = 700,
                Radius = 40,
                MultipleCount = 5,
                MultipleAngle = 5 * (float)Math.PI / 180,
                MissileSpeed = 3000,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Kog'Maw
            Spells.Add(new SpellData
            {
                ChampionName = "KogMaw",
                SpellName = "KogMawQ",
                MissileName = "KogMawQ",
                Delay = 250,
                Range = 1200,
                Radius = 70,
                MissileSpeed = 1650,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "KogMaw",
                SpellName = "KogMawVoidOoze",
                MissileName = "KogMawVoidOozeMissile",
                Delay = 250,
                Range = 1360,
                Radius = 70,
                MissileSpeed = 120,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "KogMaw",
                SpellName = "KogMawLivingArtillery",
                Delay = 1200,
                Range = 1800,
                Radius = 225,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region LeBlanc
            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                SpellName = "LeblancW",
                Delay = 0,
                Range = 600,
                Radius = 220,
                MissileSpeed = 1450,
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                SpellName = "LeblancE",
                MissileName = "LeblancEMissile",
                Delay = 250,
                Range = 960,
                Radius = 55,
                MissileSpeed = 1750,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Lee Sin
            Spells.Add(new SpellData
            {
                ChampionName = "LeeSin",
                SpellName = "BlindMonkQOne",
                MissileName = "BlindMonkQOne",
                Delay = 250,
                Range = 1100,
                Radius = 65,
                MissileSpeed = 1800,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Leona
            Spells.Add(new SpellData
            {
                ChampionName = "Leona",
                SpellName = "LeonaZenithBlade",
                MissileName = "LeonaZenithBladeMissile",
                Delay = 250,
                Range = 905,
                Radius = 70,
                MissileSpeed = 2000,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Leona",
                SpellName = "LeonaSolarFlare",
                Delay = 1000,
                Range = 1200,
                Radius = 300,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Lissandra
            Spells.Add(new SpellData
            {
                ChampionName = "Lissandra",
                SpellName = "LissandraQ",
                MissileName = "LissandraQMissile",
                Delay = 250,
                Range = 825,
                Radius = 75,
                MissileSpeed = 2200,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Lissandra",
                SpellName = "LissandraQShards",
                MissileName = "LissandraQShards",
                Delay = 250,
                Range = 825,
                Radius = 90,
                MissileSpeed = 2200,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Lissandra",
                SpellName = "LissandraE",
                MissileName = "LissandraEMissile",
                Delay = 250,
                Range = 1025,
                Radius = 125,
                MissileSpeed = 850,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Lucian
            Spells.Add(new SpellData
            {
                ChampionName = "Lucian",
                SpellName = "LucianQ",
                Delay = 500,
                Range = 900,
                Radius = 65,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Lucian",
                SpellName = "LucianW",
                MissileName = "LucianWMissile",
                Delay = 250,
                Range = 1000,
                Radius = 55,
                MissileSpeed = 1600,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Lucian",
                SpellName = "LucianR",
                MissileName = "LucianRMissile",
                ExtraMissileNames = new [] { "LucianRMissileOffhand" },
                Delay = 500,
                Range = 1400,
                Radius = 110,
                MissileSpeed = 2800,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Lulu
            Spells.Add(new SpellData
            {
                ChampionName = "Lulu",
                SpellName = "LuluQ",
                MissileName = "LuluQMissile",
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1450,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Lulu",
                SpellName = "LuluQMissileTwo",
                MissileName = "LuluQMissileTwo",
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1450,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Lux
            Spells.Add(new SpellData
            {
                ChampionName = "Lux",
                SpellName = "LuxLightBinding",
                MissileName = "LuxLightBindingMis",
                Delay = 250,
                Range = 1300,
                Radius = 70,
                MissileSpeed = 1200,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Lux",
                SpellName = "LuxLightStrikeKugel",
                MissileName = "LuxLightStrikeKugel",
                Delay = 250,
                Range = 1100,
                Radius = 275,
                MissileSpeed = 1300,
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Lux",
                SpellName = "LuxMaliceCannon",
                Delay = 1000,
                Range = 3500,
                Radius = 190,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Malphite
            Spells.Add(new SpellData
            {
                ChampionName = "Malphite",
                SpellName = "UFSlash",
                Delay = 0,
                Range = 1000,
                Radius = 270,
                MissileSpeed = 1500,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Malzahar
            Spells.Add(new SpellData
            {
                ChampionName = "Malzahar",
                SpellName = "MalzaharQ",
                MissileName = "MalzaharQMissile",
                Delay = 750,
                Range = 900,
                Radius = 85,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Miss Fortune
            Spells.Add(new SpellData
            {
                ChampionName = "MissFortune",
                SpellName = "MissFortuneScattershot",
                Delay = 2250,
                Range = 1000,
                Radius = 275,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.E,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Morgana
            Spells.Add(new SpellData
            {
                ChampionName = "Morgana",
                SpellName = "DarkBindingMissile",
                MissileName = "DarkBindingMissile",
                Delay = 250,
                Range = 1300,
                Radius = 80,
                MissileSpeed = 1200,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Nami
            Spells.Add(new SpellData
            {
                ChampionName = "Nami",
                SpellName = "NamiQ",
                MissileName = "NamiQMissile",
                Delay = 950,
                Range = 1625,
                Radius = 150,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Nami",
                SpellName = "NamiR",
                MissileName = "NamiRMissile",
                Delay = 500,
                Range = 2750,
                Radius = 260,
                MissileSpeed = 850,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Nautilus
            Spells.Add(new SpellData
            {
                ChampionName = "Nautilus",
                SpellName = "NautilusAnchorDrag",
                MissileName = "NautilusAnchorDragMissile",
                Delay = 250,
                Range = 1250,
                Radius = 90,
                MissileSpeed = 2000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Nautilus
            Spells.Add(new SpellData
            {
                ChampionName = "Nidalee",
                SpellName = "JavelinToss",
                MissileName = "JavelinToss",
                Delay = 250,
                Range = 1500,
                Radius = 40,
                MissileSpeed = 1300,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Nocturne
            Spells.Add(new SpellData
            {
                ChampionName = "Nocturne",
                SpellName = "NocturneDuskbringer",
                MissileName = "NocturneDuskbringer",
                Delay = 250,
                Range = 1125,
                Radius = 60,
                MissileSpeed = 1400,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Olaf
            Spells.Add(new SpellData
            {
                ChampionName = "Olaf",
                SpellName = "OlafAxeThrowCast",
                MissileName = "OlafAxeThrow",
                Delay = 250,
                Range = 1000,
                ExtraRange = 150,
                Radius = 105,
                MissileSpeed = 1600,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Orianna
            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OrianaIzunaCommand",
                MissileName = "OrianaIzuna",
                Delay = 0,
                Range = 1500,
                Radius = 80,
                MissileSpeed = 1200,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OrianaRedactCommand",
                MissileName = "OrianaRedact",
                Delay = 0,
                Range = 1500,
                Radius = 85,
                MissileSpeed = 1850,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true,
                IsReturnSpell = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Orianna",
                SpellName = "OrianaDetonateCommand",
                Delay = 1000,
                Range = 2500,
                Radius = 325,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.R,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Poppy
            Spells.Add(new SpellData
            {
                ChampionName = "Poppy",
                SpellName = "PoppyQ",
                Delay = 500,
                Range = 430,
                Radius = 100,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Poppy",
                SpellName = "PoppyRSpell",
                MissileName = "PoppyRMissile",
                Delay = 300,
                Range = 1200,
                Radius = 100,
                MissileSpeed = 1600,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Quinn
            Spells.Add(new SpellData
            {
                ChampionName = "Quinn",
                SpellName = "QuinnQ",
                MissileName = "QuinnQ",
                Delay = 250,
                Range = 1050,
                Radius = 60,
                MissileSpeed = 1550,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Rek'Sai
            Spells.Add(new SpellData
            {
                ChampionName = "RekSai",
                SpellName = "RekSaiQBurrowed",
                MissileName = "RekSaiQBurrowedMis",
                Delay = 500,
                Range = 1500,
                Radius = 60,
                MissileSpeed = 1950,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Rengar
            Spells.Add(new SpellData
            {
                ChampionName = "Rengar",
                SpellName = "RengarE",
                ExtraSpellNames = new[] { "RengarEEmp" },
                MissileName = "RengarEMis",
                Delay = 250,
                Range = 1000,
                Radius = 70,
                MissileSpeed = 1500,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Riven
            Spells.Add(new SpellData
            {
                ChampionName = "Riven",
                SpellName = "RivenIzunaBlade",
                MissileName = "RivenWindslashMissileLeft",
                ExtraMissileNames = new []{ "RivenWindslashMissileRight", "RivenWindslashMissileCenter" },
                Delay = 250,
                Range = 1100,
                Radius = 125,
                MultipleCount = 3,
                MultipleAngle = 15 * (float)Math.PI / 180,
                MissileSpeed = 1600,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = false
            });
            #endregion
            #region Rumble
            Spells.Add(new SpellData
            {
                ChampionName = "Rumble",
                SpellName = "RumbleGrenade",
                MissileName = "RumbleGrenadeMissile",
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 2000,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Rumble",
                SpellName = "RumbleR",
                MissileName = "RumbleCarpetBombMissile",
                Delay = 400,
                Range = 1200,
                Radius = 200,
                MissileSpeed = 1600,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Ryze
            Spells.Add(new SpellData
            {
                ChampionName = "Ryze",
                SpellName = "RyzeQ",
                MissileName = "RyzeQ",
                Delay = 250,
                Range = 1000,
                Radius = 55,
                MissileSpeed = 1700,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Sejuani
            Spells.Add(new SpellData
            {
                ChampionName = "Sejuani",
                SpellName = "SejuaniQ",
                Delay = 0,
                Range = 900,
                ExtraRange = 200,
                Radius = 70,
                MissileSpeed = 1600,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Ryze",
                SpellName = "SejuaniR",
                MissileName = "SejuaniRMissile",
                Delay = 250,
                Range = 1500,
                Radius = 110,
                MissileSpeed = 1600,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Shen
            Spells.Add(new SpellData
            {
                ChampionName = "Shen",
                SpellName = "ShenQMissile",
                MissileName = "ShenQMissile",
                Delay = 0,
                Range = 20000,
                Radius = 50,
                MissileSpeed = 2000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true,
                IsReturnSpell = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Shen",
                SpellName = "ShenE",
                Delay = 0,
                Range = 650,
                ExtraRange = 200,
                Radius = 50,
                MissileSpeed = 1600,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Shyvana
            Spells.Add(new SpellData
            {
                ChampionName = "Shyvana",
                SpellName = "ShyvanaFireball",
                MissileName = "ShyvanaFireballMissile",
                Delay = 250,
                Range = 950,
                Radius = 60,
                MissileSpeed = 1700,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Shyvana",
                SpellName = "ShyvanaTransformCast",
                Delay = 250,
                Range = 1000,
                Radius = 150,
                MissileSpeed = 1500,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Shyvana",
                SpellName = "ShyvanaFireballDragon2",
                MissileName = "ShyvanaFireballDragonMissileMax",
                Delay = 250,
                Range = 850,
                Radius = 70,
                MissileSpeed = 2000,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Sion
            Spells.Add(new SpellData
            {
                ChampionName = "Sion",
                SpellName = "SionE",
                MissileName = "SionEMissile",
                Delay = 250,
                Range = 800,
                Radius = 80,
                MissileSpeed = 1800,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Sivir
            Spells.Add(new SpellData
            {
                ChampionName = "Sivir",
                SpellName = "SivirQ",
                MissileName = "SivirQMissile",
                Delay = 250,
                Range = 1250,
                Radius = 90,
                MissileSpeed = 1350,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Sivir",
                SpellName = "SivirQReturn",
                MissileName = "SivirQMissileReturn",
                Delay = 250,
                Range = 1250,
                Radius = 90,
                MissileSpeed = 1350,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true,
                IsReturnSpell = true
            });
            #endregion
            #region Skarner
            Spells.Add(new SpellData
            {
                ChampionName = "Skarner",
                SpellName = "SkarnerFracture",
                MissileName = "SkarnerFractureMissile",
                Delay = 250,
                Range = 1000,
                Radius = 70,
                MissileSpeed = 1500,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Sona
            Spells.Add(new SpellData
            {
                ChampionName = "Sona",
                SpellName = "SonaR",
                MissileName = "SonaR",
                Delay = 250,
                Range = 1000,
                Radius = 140,
                MissileSpeed = 2400,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Soraka
            Spells.Add(new SpellData
            {
                ChampionName = "Soraka",
                SpellName = "SorakaQ",
                MissileName = "SorakaQMissile",
                Delay = 500,
                Range = 950,
                Radius = 300,
                MissileSpeed = 1750,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = false
            });
            #endregion
            #region Swain
            Spells.Add(new SpellData
            {
                ChampionName = "Swain",
                SpellName = "SwainShadowGrasp",
                Delay = 1100,
                Range = 900,
                Radius = 180,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Syndra
            Spells.Add(new SpellData
            {
                ChampionName = "Syndra",
                SpellName = "SyndraQ",
                MissileName = "SyndraQSpell",
                Delay = 600,
                Range = 800,
                Radius = 150,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Syndra",
                SpellName = "SyndraWCast",
                Delay = 250,
                Range = 950,
                Radius = 210,
                MissileSpeed = 1450,
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            #endregion
            #region Tahm Kench
            Spells.Add(new SpellData
            {
                ChampionName = "TahmnKench",
                SpellName = "TahmKenchQ",
                MissileName = "TahmKenchQMissile", 
                Delay = 250,
                Range = 850,
                Radius = 90,
                MissileSpeed = 2800,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Taliyah
            Spells.Add(new SpellData
            {
                ChampionName = "Taliyah",
                SpellName = "TaliyahQ",
                MissileName = "TaliyahQMis",
                Delay = 275,
                Range = 1000,
                Radius = 100,
                MissileSpeed = 3600,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Taliyah",
                SpellName = "TaliyahW",
                Delay = 600,
                Range = 900,
                Radius = 200,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.W,
                Type = SkillshotType.Circle,
                FixedRange = false,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Taliyah",
                SpellName = "TaliyahR",
                MissileName = "TaliyahRMis",
                Delay = 1000,
                Range = 3000,
                Radius = 150,
                MissileSpeed = 850,
                Slot = SpellSlot.R,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = false
            });
            #endregion
            #region Talon
            Spells.Add(new SpellData
            {
                ChampionName = "Talon",
                SpellName = "TalonW",
                MissileName = "TalonWMissileOne",
                Delay = 250,
                Range = 800,
                Radius = 80,
                MultipleCount = 3,
                MultipleAngle = 12 * (float)Math.PI / 180,
                MissileSpeed = 2300,
                Slot = SpellSlot.W,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = false
            });
            #endregion
            #region Taric
            Spells.Add(new SpellData
            {
                ChampionName = "Taric",
                SpellName = "TaricE",
                Delay = 1000,
                Range = 750,
                Radius = 100,
                MissileSpeed = int.MaxValue,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true,
                FollowCaster = true
            });
            #endregion
            #region Thresh
            Spells.Add(new SpellData
            {
                ChampionName = "Thresh",
                SpellName = "ThreshQ",
                MissileName = "ThreshQMissile",
                Delay = 500,
                Range = 1100,
                Radius = 70,
                MissileSpeed = 1900,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            Spells.Add(new SpellData
            {
                ChampionName = "Thresh",
                SpellName = "ThreshE",
                MissileName = "ThreshEMissile1",
                Delay = 125,
                Range = 600,
                Radius = 110,
                MissileSpeed = 2000,
                Slot = SpellSlot.E,
                Type = SkillshotType.Line,
                FixedRange = true,
                Centered = true,
                AddHitbox = true
            });
            #endregion
            #region Twisted Fate
            Spells.Add(new SpellData
            {
                ChampionName = "TwistedFate",
                SpellName = "Wildcards",
                MissileName = "SealFateMissile",
                Delay = 250,
                Range = 1450,
                Radius = 40,
                MultipleCount = 3,
                MultipleAngle = 28 * (float)Math.PI / 180,
                MissileSpeed = 1000,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
            #region Varus
            Spells.Add(new SpellData
            {
                ChampionName = "Varus",
                SpellName = "VarusQMissile",
                MissileName = "VarusQMissile",
                Delay = 250,
                Range = 1800,
                Radius = 70,
                MissileSpeed = 1900,
                Slot = SpellSlot.Q,
                Type = SkillshotType.Line,
                FixedRange = true,
                AddHitbox = true
            });
            #endregion
        }

        public static SpellData GetByName(string name)
        {
            return Spells.FirstOrDefault(x => x.SpellName == name || x.ExtraSpellNames.Contains(name));
        }

        public static SpellData GetByMissile(string missile)
        {
            return Spells.FirstOrDefault(x => x.MissileName == missile || x.ExtraMissileNames.Contains(missile));
        }
    }
}
