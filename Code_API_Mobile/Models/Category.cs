using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Code_API_Mobile.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

}
