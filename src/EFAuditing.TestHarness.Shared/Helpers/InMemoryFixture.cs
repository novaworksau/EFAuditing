﻿using Microsoft.Data.Entity;
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

            services
                .AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<TestDbContext>(options =>
                    options.UseInMemoryDatabase()
                );

            _serviceProvider = services.BuildServiceProvider();

        }

        public IServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }
}