using System;
using System.Configuration;
using System.Data.Entity;
using ContactInformation.Models;
using ContactInformation.Contracts;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;

namespace ContactInformation.Data
{
    public class DataContext : DbContext, IDataContext
    {
       
        public DbSet<ContactInformations> ContactInformationsDetails { get; set; }       

        public DataContext()  : base(ConnenctionStringProject)
        {
            Database.SetInitializer<DataContext>(null);
        }

        public static string ConnenctionStringProject
        {
            get
            {
                return Convert.ToString(Settings.DBConnectionString.ConnectionString);
            }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
