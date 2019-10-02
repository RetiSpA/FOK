using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Reti.Lab.FoodOnKontainers.Middleware.Extensions
{
    public static class DateTimeFormatter
    {
        public static IServiceCollection DateTimeItFormatter(this IServiceCollection services)
        {
            services.Configure<MvcJsonOptions>(jsonOptions =>
            {
                if (string.Equals(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, "it", StringComparison.OrdinalIgnoreCase))
                {
                    //Solo se i server non sono con la culture IT
                    jsonOptions.SerializerSettings.Culture = new System.Globalization.CultureInfo("it-IT");
                }

                jsonOptions.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });
            return services;
        }
    }
}
