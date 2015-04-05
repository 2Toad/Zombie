using System.ComponentModel.DataAnnotations.Schema;

namespace Zombie.Models
{
    public abstract class LocalizedModel : DbModel
    {
        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
    }
}
