using Domain.Interfaces;
using Infrastructre.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructre
{
    public static class DependencyInjection
    {
        public static void AddInfrastructre(this IServiceCollection @this)
        {
            @this.AddTransient<INotificationService, NotificationService>();
        }
    }
}
