using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Tools.Emails
{
    public class EmailExtensions
    {
        public static void AddEmail(this IServiceCollection services, Action<EmailOptions> action)
        {
            var option = new EmailOptions();
            action(option);
            services.AddSingleton<EmailOptions>(option);
            services.AddSingleton<IEmail, Email>();
        }
    }
}
