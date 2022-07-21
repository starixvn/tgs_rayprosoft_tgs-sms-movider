using System.Text.Json.Serialization;

namespace Tgs.Sms.Movider.Models
{
    public class BadPhoneNumberList
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = default!;

        [JsonPropertyName("number")]
        public string Number { get; set; } = default!;
    }
}