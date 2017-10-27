using System.Data.Entity;
using ReflectionPoC.Entities;

namespace ReflectionPoC
{
    public class OwnContext : DbContext
    {
        public OwnContext()
        {
            Database.SetInitializer<OwnContext>(new DropCreateDatabaseAlways<OwnContext>());
        }

        public DbSet<CompanyImportation> CompaniesImportations { get; set; }
        public DbSet<CustomerImportation> CustomersImportations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CompanyImportation>().ToTable("_Importation_Companies");
            modelBuilder.Entity<CustomerImportation>().ToTable("_Importation_Customers");
        }
    }
}