using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Barricade;

namespace Zombie.Models
{
    public class Claim : DbModel, IClaim
    {
        // IClaim
        public string Type { get; set; }
        public string Value { get; set; }

        public virtual List<User> Users { get; set; }

        public static void Seed(SiteContext context)
        {
            var claims = new[] {
                new Claim { Type = "Role", Value = "Admin" },
                new Claim { Type = "Role", Value = "User" }
            };

            context.Claims.AddOrUpdate(c => new { c.Type, c.Value }, claims);
            context.SaveChanges();
        }
    }

    internal static class ClaimConfiguration
    {
        internal static DbModelBuilder ConfigureClaim(this DbModelBuilder modelBuilder)
        {
            var table = modelBuilder.Entity<Claim>();

            // IClaim
            table.Property(x => x.Type).HasMaxLength(250);
            table.Property(x => x.Value).HasMaxLength(250);

            return modelBuilder;
        }
    }
}
