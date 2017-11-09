using System;

namespace UnitOfWorkCore.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TransactionOrderAttribute : Attribute
    {
        public string UnitOfWorkKey { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// Specifies the order of execution for the given UoW. Unspecified UoWs 
        /// will commit their transactions at the end of the queue, right after
        /// the ones for which order has been set.
        /// </summary>
        /// <param name="uowKeysOrder"></param>
        public TransactionOrderAttribute(string unitOfWorkKey, int order)
        {
            UnitOfWorkKey = unitOfWorkKey;
            Order = order;
        }
    }
}
