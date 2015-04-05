using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Zombie.Models;

namespace Zombie
{
    public class SiteContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Constructs a new context instance using the class name as the name of the connection string
        /// </summary>
        public SiteContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but before the model has been 
        /// locked down and used to initialize the context
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Add<ForeignKeyNamingConvention>();

            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder
                .ConfigureClaim()
                .ConfigureLanguage()
                .ConfigureUser();
        }
    }
}
