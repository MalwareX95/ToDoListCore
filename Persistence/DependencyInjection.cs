using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection @this, IConfiguration configuration)
        {
            @this.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("defaultConnection")));
            //@this.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            @this.AddScoped<ApplicationDbContextBase>(provider => provider.GetRequiredService<ApplicationDbContext>());
        }
    }
}
