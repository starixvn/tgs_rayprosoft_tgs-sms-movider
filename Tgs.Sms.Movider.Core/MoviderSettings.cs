namespace Tgs.Sms.Movider
{
    public record MoviderSettings
    {
        public string ApiKey { get; init; } = default!;

        public string ApiSecret { get; init; } = default!;
    };
}