using Microsoft.Extensions.DependencyInjection;
using Standard.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
namespace Standard.Infrastructure.Iocs
{
    /// <summary>
    /// IOC容器
    /// </summary>
    public static class Ioc
    {
        public static IServiceProvider Container { get; set; }
        public static IScope CreateScope()
        {
            return new Scope();
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
            {
                return WebHelper.HttpContext?.RequestServices != null ?
                WebHelper.HttpContext.RequestServices.GetService<T>() :
                Container.GetService<T>();
            }
        }
        /// <summary>
        /// IOC局部生命周期
        /// </summary>
        public interface IScope : IDisposable
            {
                T GetService<T>();
                object GetService(Type serviceType);
            }
    /// <summary>
    /// IOC局部生命周期
    /// </summary>
    public class Scope : IScope
    {
        IServiceScope serviceScope;
        public Scope()
        {
            serviceScope = Ioc.Container.CreateScope();
        }
        public void Dispose()
        {
            serviceScope.Dispose();


        }
        public T GetService<T>()
        {
            return serviceScope.ServiceProvider.GetService<T>();
        }
        public object GetService(Type serviceType)
        {
            return serviceScope.ServiceProvider.GetService(serviceType);
        }
    }
}
