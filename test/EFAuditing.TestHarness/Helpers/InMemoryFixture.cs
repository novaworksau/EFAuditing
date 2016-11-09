using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFAuditing.TestHarness.Helpers
{
    public class InMemoryFixture
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryFixture()
        {
            var services = new ServiceCollection();

            services.AddScoped<IExternalAuditStoreProvider, TestableDbAuditStoreProvider>();
            services.AddScoped<IAuditLogBuilder, AuditLogBuilder>();

            _serviceProvider = services.BuildServiceProvider();

            services
               .AddEntityFrameworkInMemoryDatabase()
               .AddDbContext<TestDbContext>(options => {
                   options.UseInMemoryDatabase();
                   options.UseInternalServiceProvider(_serviceProvider);
               });

            _serviceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }
}
