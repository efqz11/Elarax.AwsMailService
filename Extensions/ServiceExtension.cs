using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace  Elarax.AwsMailService
{
    public static class ServiceExtension
    {
        public static void AddEmailService(this IServiceCollection services,
            Action<EmailServiceConfig> setupAction)
        {
            // Add the service.
            services.AddSingleton<IEmailService, EmailService>();

            // configure service
            services.Configure(setupAction);
        }
    }
}
