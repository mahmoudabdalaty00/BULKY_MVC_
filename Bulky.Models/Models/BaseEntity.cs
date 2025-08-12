using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        public bool IsHidden { get; set; } 

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
