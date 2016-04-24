using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Base.Model
{


    public abstract class BaseUnitOfWork : IDisposable
    {
        public DbContext Context { get; set; }       


        #region Legacy - подобни методи
        public IQueryable<T> Query<T>() where T : class
        {
            return this.Context.Set<T>().AsQueryable();
        }
        public T Find<T>(object id) where T : class
        {
            try
            {
                return this.Context.Set<T>().Find(id);
            }
            catch
            {
                return null;
            }
        }
        

        public TransactionScope CreateTransactionScope(System.Transactions.IsolationLevel level = System.Transactions.IsolationLevel.ReadCommitted)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = level;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
        #endregion



        #region IDisposable Members

        public virtual void Dispose()
        {
            Context.Dispose();
            Context = null;
        }

        #endregion
    }


}
