using System;
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
                return skins.Select(x => x.Name == "default" ? "Default" : x.Name).ToArray();
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
        [JsonProperty("name")] public string Name;
    }
}
