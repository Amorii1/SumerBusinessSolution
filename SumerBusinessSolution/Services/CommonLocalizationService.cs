using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SumerBusinessSolution.Resources;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace SumerBusinessSolution.Services
{
    public class CommonLocalizationService
    {
        private readonly IStringLocalizer localizer;
        public CommonLocalizationService(IStringLocalizerFactory factory)
        {
            var assemblyName = new AssemblyName(typeof(CommonResources).GetTypeInfo().Assembly.FullName);
            localizer = factory.Create(nameof(CommonResources), assemblyName.Name);
        }

        public string Get(string key)
        {
            return localizer[key];
        }
    }
}