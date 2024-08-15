using Newtonsoft.Json;

namespace UExpo.Domain.Shared.Converters;

public static class JsonConverter
{
    public static Dictionary<string, object> JsonToDictionary(string json = "")
    {
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? [];
    }

    public static string DictionaryToJson(Dictionary<string, object>? dictionary = null)
    {
        return JsonConvert.SerializeObject(dictionary);
    }
}