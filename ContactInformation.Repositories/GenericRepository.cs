using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ContactInformation.Contracts;
using System.Collections.Generic;
using System.Data.SqlClient;
using ContactInformation.Models;
using System.Transactions;
using ContactInformation.Data;

namespace ContactInformation.Repositories
{
    class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IDataContext context;
        private readonly IDbSet<TEntity> dbset;

        public GenericRepository(IDataContext context)
        {
            this.context = context;
            this.dbset = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get()
        {
            IQueryable<TEntity> query = dbset;
            return query;
        }


        public TEntity GetByID(int id)
        {
            var query = dbset.Find(id);
            return query;
        }


        public TEntity GetByType(string Type)
        {
            var query = dbset.Find(Type);
            return query;
        }
        
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = dbset.Where(predicate);
            return query;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string[] include)
        {
            IQueryable<TEntity> query = this.dbset;
            foreach (string inc in include)
            {
                query = query.Include(inc);
            }
            return query.Where(predicate);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string include)
        {
            IQueryable<TEntity> query = this.dbset;
            return query.Include(include).Where(predicate);
        }

        public void Add(TEntity entity)
        {
            DbEntityEntry entry = this.context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Detached;
            }
            else
            {
                dbset.Add(entity);
                //LogAuditEntry(entity, "Add");
            }

        }

        public void Update(TEntity entity)
        {
            DbEntityEntry entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                dbset.Attach(entity);
            }
            entry.State = EntityState.Modified;
            //LogAuditEntry(entity, "Update");
        }

        public void Delete(TEntity entity)
        {
            DbEntityEntry entry = this.context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                dbset.Attach(entity);
                dbset.Remove(entity);
                //LogAuditEntry(entity, "Delete");
            }

            
        }

        public void Delete(int id)
        {
            var entity = this.GetByID(id);
            if (entity != null)
            {
                this.Delete(entity);
                //LogAuditEntry(entity, "Delete");
            }
        }

        public void Detach(TEntity entity)
        {
            var entry = context.Entry(entity);
            entry.State = EntityState.Detached;
        }

        public List<TEntity> GetSP(string spName)
        {
            List<TEntity> result = context.Database.SqlQuery<TEntity>(spName).ToList();
            return result;
        }       
    }
}
