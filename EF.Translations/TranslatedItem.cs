using System.Text.Json.Serialization;

namespace EF.Translations;

public class TranslatedItem
{
    [JsonPropertyName("locale")] public string Language { get; set; }
    public object Value { get; set; }
}