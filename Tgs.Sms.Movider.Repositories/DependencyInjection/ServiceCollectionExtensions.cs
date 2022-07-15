using System;
using Microsoft.EntityFrameworkCore;
using Tgs.Sms.Movider;
using Tgs.Sms.Movider.Repositories;
using Tgs.Sms.Movider.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMoviderRepositories(
            this IServiceCollection services,
            Action<IServiceProvider, DbContextOptionsBuilder> dbContextConfigure)
            => services
                .AddDbContext<TgsContext>(dbContextConfigure)
                .AddTransient<ITgsRepository, TgsRepository>();
    }
}