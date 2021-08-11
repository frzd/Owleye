using System;

namespace Owleye.Infrastructure.Service
{
    /// <summary>
    ///  temporary usage of this pattern 
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceProvider _provider;
        public static void Init(IServiceProvider provider)
        {
            _provider = provider;
        }

        public static T Resolve<T>() => (T)_provider.GetService(typeof(T));
    }
}
