using Newtonsoft.Json;

namespace UExpo.Domain.Shared.Converters;

public static class JsonConverter
{
    public static List<Dictionary<string, object>> JsonToDictionary(string json = "")
    {
        if (string.IsNullOrEmpty(json)) return [];
        return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json) ?? [];
    }

    public static string DictionaryToJson(List<Dictionary<string, object>>? dictionary = null)
    {
        return JsonConvert.SerializeObject(dictionary, Formatting.None);
    }
}