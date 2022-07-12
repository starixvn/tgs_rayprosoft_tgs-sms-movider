using System.Text.Json.Serialization;

namespace Tgs.Sms.Movider
{
    public record MoviderSettings
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; init; } = default!;

        [JsonPropertyName("api_secret")]
        public string ApiSecret { get; init; } = default!;
    };
}