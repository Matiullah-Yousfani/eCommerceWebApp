using System.ComponentModel.DataAnnotations;

namespace eCommerceWebApp.Models
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }
        [Required]
        public string CategoryName { get; set; }

        
    }
}
