using System.Text.Json.Serialization;

namespace Tgs.Sms.Movider.Models
{
    public class SendSMSResponse
    {
        [JsonPropertyName("remaining_balance")]
        public decimal RemainingBalance { get; set; }

        [JsonPropertyName("total_sms")]
        public int TotalSms { get; set; }

        [JsonPropertyName("phone_number_list")]
        public PhoneNumberList[] PhoneNumberList { get; set; } = default!;

        [JsonPropertyName("bad_phone_number_list")]
        public BadPhoneNumberList[] BadPhoneNumberList { get; set; } = default!;
    }
}