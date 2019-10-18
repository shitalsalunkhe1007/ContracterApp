using ContactInformation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Collections.Concurrent;
using System.Data.Entity.Core;

namespace ContactInformation.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private TransactionScope transaction;

		private readonly IDataContext context;

		private readonly ConcurrentDictionary<Type, object> repositories;

		private bool disposed;

		public UnitOfWork(IDataContext DbContext)
		{
			context = DbContext;
			repositories = new ConcurrentDictionary<Type, object>();
			disposed = false;
		}

		public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
		{
			// Checks if the Dictionary Key contains the Model class
			if (repositories.Keys.Contains(typeof(TEntity)))
			{
				// Return the repository for that Model class
				return repositories[typeof(TEntity)] as IGenericRepository<TEntity>;
			}

			// If the repository for that Model class doesn't exist, create it
			//var repository = GenericRepository<TEntity>(context);       

			var repositoryType = typeof(GenericRepository<>);
			var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context);

			// Add it to the dictionary
			repositories.GetOrAdd(typeof(TEntity), repository);
			return (IGenericRepository<TEntity>)repositories[typeof(TEntity)];

		}
		public bool Commit()
		{
			try
			{
				using (transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					context.SaveChanges();
					transaction.Complete();
					return true;
				}
			}
			catch (Exception ex)
			{
				transaction.Dispose();
			}
			return false;
		}

		public IDataContext DbContext
		{
			get { return context; }
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed) return;

			if (disposing)
			{
				context.Dispose();
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
