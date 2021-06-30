using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klika.ResourceApi.Model.Entities.TemplateDbContext
{
    public class TemplateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TemplateProperty1 { get; set; }
        public decimal TemplateProperty2 { get; set; }
        public bool TemplateProperty3 { get; set; }
    }
}
