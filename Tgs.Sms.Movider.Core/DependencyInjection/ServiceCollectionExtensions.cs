using System;
using Microsoft.Extensions.Options;
using Tgs.Sms.Movider;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMoviderSms(
        this IServiceCollection services,
        Action<MoviderSmsOptions, IServiceProvider> configure)
        => services.AddOptions<MoviderSmsOptions>()
            .Configure(configure)
            .Services
            .AddHttpClient<MoviderClient>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<MoviderSmsOptions>>().Value;

                httpClient.BaseAddress = options.SmsApiUrl;
            })
            .Services;
    }
}