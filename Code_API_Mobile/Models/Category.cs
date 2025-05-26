using System.ComponentModel.DataAnnotations;

namespace Code_API_Mobile.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }

}
