using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Ioc
{
    public class ModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.ServiceProvider.GetService<MainWindowViewModel>();
    }
}
