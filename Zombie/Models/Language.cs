using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;

namespace Zombie.Models
{
    public class Language : DbModel
    {
        /// <summary>
        /// ISO standard code consiting of up to three parts: [language code] - [script code] - [country code]
        /// <seealso cref="http://userguide.icu-project.org/locale"/>
        /// </summary>
        public string Locale { get; set; }
        public string Description { get; set; }

        public static void Seed(SiteContext context)
        {
            var languages = new[] {
                new Language {Locale = "en-US", Description = "English (United States)"},
                new Language {Locale = "es-MX", Description = "Spanish (Mexico)"}
            };

            context.Languages.AddOrUpdate(u => new { u.Locale }, languages);
            context.SaveChanges();
        }
    }

    internal static class LanguageConfiguration
    {
        internal static DbModelBuilder ConfigureLanguage(this DbModelBuilder modelBuilder)
        {
            var table = modelBuilder.Entity<Language>();

            table.Property(x => x.Locale).IsRequired().HasMaxLength(11)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true }));

            table.Property(x => x.Description).IsRequired().HasMaxLength(250);

            return modelBuilder;
        }
    }
}
