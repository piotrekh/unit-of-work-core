using System.Collections.Generic;

namespace UnitOfWorkCore.MultiContext
{
    public interface IUnitOfWorkPool
    {
        IEnumerable<string> RegisteredUoWKeys { get; }

        IUnitOfWork Get(string key);

        IEnumerable<IUnitOfWork> GetAll();
    }
}
