using System;
using System.Collections.Generic;

namespace UnitOfWorkCore.MultiContext
{
    public class UnitOfWorkPool : IUnitOfWorkPool
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UnitOfWorkPoolOptions _options;

        public IEnumerable<string> RegisteredUoWKeys => _options.RegisteredUoWs.Keys;


        public UnitOfWorkPool(IServiceProvider provider, UnitOfWorkPoolOptions options)
        {
            _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IUnitOfWork Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (!_options.RegisteredUoWs.ContainsKey(key))
                throw new ArgumentException("Unit of Work for the specified key is not registered in the pool", nameof(key));

            var uowType = _options.RegisteredUoWs[key];
            return _serviceProvider.GetService(uowType) as IUnitOfWork;
        }

        public IEnumerable<IUnitOfWork> GetAll()
        {
            foreach (var uowType in _options.RegisteredUoWs)
            {
                yield return _serviceProvider.GetService(uowType.Value) as IUnitOfWork;
            }
        }
    }
}
