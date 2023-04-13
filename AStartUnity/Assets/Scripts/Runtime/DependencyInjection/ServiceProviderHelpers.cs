using System;

namespace Runtime.Grid.Services
{
    public static class ServiceProviderHelpers
    {
        public static T GetService<T>(this IServiceProvider source) => (T)source.GetService(typeof(T));
    }
}