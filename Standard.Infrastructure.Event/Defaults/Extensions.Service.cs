using Microsoft.Extensions.DependencyInjection;
using Standard.Infrastructure.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Events.Defaults
{
    /// <summary>
    /// 事件总线扩展
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 注册事件总线服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            return services.AddSingleton<IEventHandlerManager, EventHandlerManager>()
                .AddSingleton<IEventBus, Standard.Infrastructure.Events.Defaults.EventBus>();
        }
    }
}
