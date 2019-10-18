using ContactInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactInformation.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        TEntity GetByID(int id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(int id);
        void Detach(TEntity entity);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string include);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string[] include);
        List<TEntity> GetSP(string spName);
        //List<TEntity> GetSP(string spName, List<spParameter> mysp);
    }
}
