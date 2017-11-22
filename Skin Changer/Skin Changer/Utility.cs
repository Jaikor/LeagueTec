using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Skin_Changer
{
    public static class Utility
    {
        private const string CurrentPatch = "7.23";
        private static readonly string AppData = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\SkinHax\{CurrentPatch}";
        private static HttpClient Http = new HttpClient();
        public static async Task<string> GetChampionData(string champion)
        {
            try
            {
                if (Directory.Exists(AppData))
                {
                    var championFile = $@"{AppData}\{champion}.json";
                    if (File.Exists(championFile))
                        return File.ReadAllText(championFile);
                    var response = await Http.GetAsync($"http://ddragon.leagueoflegends.com/cdn/{CurrentPatch}.1/data/en_US/champion/{champion}.json");
                    if (!response.IsSuccessStatusCode)
                        return null;
                    var json = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(championFile, json);
                    return json;
                }
                Directory.CreateDirectory(AppData);
                return await GetChampionData(champion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not retrieve champion data for {champion}: {ex.Message}");
                return null;
            }
        }

        public static async Task<string[]> GetSkins(string champion)
        {
            try
            {
                if (champion == "FiddleSticks")
                    champion = "Fiddlesticks";
                var json = await GetChampionData(champion);
                dynamic data = JObject.Parse(json);
                var skinJson = data["data"][champion]["skins"].ToString();
                var skins = (Skin[]) JsonConvert.DeserializeObject<Skin[]>(skinJson);
                var lastNum = -1;
                var chromasToAdd = new List<Tuple<int, int>>();
                for (var i = 0; i < skins.Length; i++)
                {
                    var skin = skins[i];
                    var diff = Math.Abs(skin.Num - lastNum);
                    if (diff > 1)
                    {
                        var index = i - 1;
                        var chromas = diff - 1;
                        chromasToAdd.Add(new Tuple<int, int>(index, chromas));
                    }
                    lastNum = skin.Num;
                }
                var newSkins = new List<Skin>();
                var chromasAdded = 0;
                for (var i = 0; i < skins.Length; i++)
                {
                    var skin = skins[i];
                    newSkins.Add(skin);
                    if (chromasToAdd.Count > chromasAdded && i == chromasToAdd[chromasAdded].Item1)
                    {
                        var skinName = skins.Where(x => x.HasChromas).ToArray()[chromasAdded].Name;
                        var chromaToAdd = chromasToAdd[chromasAdded];
                        Console.WriteLine($"Adding {chromaToAdd.Item2} chromas for {skinName}");
                        for (var j = 0; j < chromaToAdd.Item2; j++)
                        {
                            var skinToAdd = new Skin(skin.Num + j + 1, $"{skinName} Chroma {j + 1}", false);
                            newSkins.Add(skinToAdd);
                        }
                        chromasAdded += 1;
                    }
                }
                return newSkins.Select(x => x.Name == "default" ? "Default" : x.Name).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching skins: {ex.Message}");
                return new[] {""};
            }
        }
    }
    public class Skin
    {
        [JsonProperty("num")] public int Num;
        [JsonProperty("name")] public string Name;
        [JsonProperty("chromas")] public bool HasChromas;

        public Skin(int num, string name, bool hasChromas)
        {
            Num = num;
            Name = name;
            HasChromas = hasChromas;
        }
    }
}
