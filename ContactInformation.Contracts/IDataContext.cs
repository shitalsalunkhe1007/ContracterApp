using ContactInformation.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ContactInformation.Contracts
{
    public interface IDataContext
    {
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        Database Database { get; }
        int SaveChanges();
        void Dispose();
		//void Refresh(System.Data.Linq.RefreshMode mode, object entity);
		//DbSet<QueryDetails> QueryDetails { get; set; }
		//DbSet<Attachments_Query> Attachments_Query { get; set; }
		//DbSet<MstImpact> MstImpact { get; set; }
		//DbSet<MstActionReason> MstActionReason { get; set; }
		//DbSet<MstQuerySource> MstQuerySource { get; set; }
		//DbSet<UserManagerMapping> UserManagerMapping { get; set; }
		//DbSet<MstQueryAction> MstQueryAction { get; set; }
		//DbSet<QueryJourney> QueryJourney { get; set; }
		//DbSet<MstQueryStatus> QueryStatus { get; set; }
	}
}
