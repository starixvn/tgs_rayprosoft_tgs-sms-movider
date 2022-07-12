using System;

namespace Tgs.Sms.Movider
{
    public record BrandInfo(
        Guid Id,
        string Name,
        bool SmsEnabled,
        string? SmsVendorCode,
        MoviderSettings? SmsVendorSettings);
}