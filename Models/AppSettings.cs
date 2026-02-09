using System.Text.Json.Serialization;

namespace PromptGen.Models;

public class AppSettings
{
    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonPropertyName("settingsDirectory")]
    public string SettingsDirectory { get; set; } = string.Empty;
}
