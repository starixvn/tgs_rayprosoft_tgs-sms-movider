namespace Tgs.Sms.Movider
{
    public class SendSMSRequest
    {
        public string ApiKey { get; set; } = default!;
        public string ApiSecret { get; set; } = default!;

        /// <summary>
        /// The number that the message should be sent. Numbers must be specified in E.164 format. Use commas (,) for multiple receivers (the maximum is 1000 numbers).
        /// </summary>
        public string To { get; set; } = default!;

        /// <summary>
        /// The body of message being sent.
        /// </summary>
        public string Text { get; set; } = default!;
    }
}