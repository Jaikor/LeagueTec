using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;
using Aimtec;
using Rectangle = System.Drawing.Rectangle;

namespace Woke
{
    public static class Utility
    {
        public static Obj_AI_Hero LocalPlayer => ObjectManager.GetLocalPlayer();
        private static HttpClient Http = new HttpClient();

        public static bool IsSpecialChampion(this Obj_AI_Hero champion)
        {
            return champion.ChampionName == "Annie" || champion.ChampionName == "Jhin";
        }

        public static int GetChampionSpellXOffset(Obj_AI_Hero champion)
        {
            return champion.IsSpecialChampion() ? 0 : 7;
        }
        public static int GetChampionSpellYOffset(Obj_AI_Hero champion)
        {
            return champion.IsSpecialChampion() ? -25 : -20;
        }
        public static int GetSummonerSpellXOffset(Obj_AI_Hero champion)
        {
            return champion.IsSpecialChampion() ? -28 : -20;
        }
        public static int GetSummonerSpellYOffset(Obj_AI_Hero champion)
        {
            return champion.IsSpecialChampion() ? -25 : -20;
        }

        public static Bitmap GetSpellBitmap(string spellName)
        {
            try
            {
                var response = Http.GetAsync($"http://ddragon.leagueoflegends.com/cdn/7.20.1/img/spell/{spellName}.png")
                    .Result;
                return new Bitmap(response.Content.ReadAsStreamAsync().Result);
            }
            catch
            {
                Console.WriteLine($"Exception when retrieving spell image for [{spellName}]");
                return new Bitmap(24, 24);
            }
        }

        public static Bitmap GetChampionBitmap(string championName)
        {
            var response = Http.GetAsync($"http://ddragon.leagueoflegends.com/cdn/7.20.1/img/champion/{championName}.png").Result;
            return new Bitmap(response.Content.ReadAsStreamAsync().Result);
        }

        /*https://stackoverflow.com/questions/5734710/c-sharp-crop-circle-in-a-image-or-bitmap*/
        public static Bitmap ClipToCircle(this Bitmap bmp, int circleUpperLeftX, int circleUpperLeftY, int circleDiameter)
        {
            using (var sourceImage = bmp)
            {
                var cropRect = new Rectangle(circleUpperLeftX, circleUpperLeftY, circleDiameter, circleDiameter);
                using (var croppedImage = sourceImage.Clone(cropRect, sourceImage.PixelFormat))
                {
                    using (var tb = new TextureBrush(croppedImage))
                    {
                        var finalImage = new Bitmap(circleDiameter, circleDiameter);
                        using (var g = Graphics.FromImage(finalImage))
                        {
                            g.FillEllipse(tb, 0, 0, circleDiameter, circleDiameter);
                            return finalImage;
                        }
                    }
                }
            }
        }

        /*https://stackoverflow.com/a/2265990*/
        public static Bitmap AsGrayscale(this Bitmap original)
        {
            var newBitmap = new Bitmap(original.Width, original.Height);
            var g = Graphics.FromImage(newBitmap);
            var colorMatrix = new ColorMatrix(
                new[]
                {
                    new[] {.3f, .3f, .3f, 0, 0},
                    new[] {.59f, .59f, .59f, 0, 0},
                    new[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();
            return newBitmap;
        }

        public static Bitmap Resize(this Bitmap imgToResize, Size size)
        {
            var b = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }
            return b;
        }

        /*Thanks, Exory*/
        public static void DrawCircleOnMinimap(Vector3 center, float radius, Color color, int thickness = 1, int quality = 100)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(
                    new Vector3(
                        center.X + radius * (float)Math.Cos(angle),
                        center.Y,
                        center.Z + radius * (float)Math.Sin(angle))
                );
            }
            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                Render.WorldToMinimap(a, out var aonScreen);
                Render.WorldToMinimap(b, out var bonScreen);

                Render.Line(aonScreen, bonScreen, color);
            }
        }
    }
}
