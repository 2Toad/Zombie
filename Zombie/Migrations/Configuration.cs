using System.Data.Entity.Migrations;
using Zombie.Models;

namespace Zombie.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SiteContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// This method is called after migrating to the latest version
        /// </summary>
        protected override void Seed(SiteContext context)
        {
            Claim.Seed(context);
            Language.Seed(context);

            if (Config.GenerateTestData) {
                User.Seed(context);
            }

            context.SaveChanges();
        }
    }
}
