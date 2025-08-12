using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.Models
{
    public class Category : BaseEntity
    {
       
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
            
    }
}
