namespace ContactInformation.Contracts
{
    public interface IUnitOfWork
    {
        IDataContext DbContext { get; }
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        bool Commit();
    }
}
