using System.Text.Json.Serialization;

namespace Tgs.Sms.Movider.Models
{
    public class PhoneNumberList
    {
        [JsonPropertyName("number")]
        public string Number { get; set; } = default!;

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; } = default!;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}