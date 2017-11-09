using System;
using System.Data;

namespace UnitOfWorkCore.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TransactionIsolationLevelAttribute : Attribute
    {
        public IsolationLevel Level { get; set; }

        public string UnitOfWorkKey { get; set; }

        public TransactionIsolationLevelAttribute(IsolationLevel level)
        {
            Level = level;
        }
    }
}
