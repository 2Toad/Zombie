using Zombie.Models;

namespace Zombie.ViewModels
{
    public class LanguageVm
    {
        public string Locale { get; set; }
        public string Description { get; set; }

        internal static LanguageVm MapFrom(Language language)
        {
            return new LanguageVm {
                Locale = language.Locale,
                Description = language.Description
            };
        }
    }
}
