using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Domain
{
    public static class DependencyInjection
    {
        public static void AddDomain(this IServiceCollection @this)
        {
            Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IModel).IsAssignableFrom(x))
                .ToList()
                .ForEach(x => @this.AddTransient(x));
        }
    }
}
