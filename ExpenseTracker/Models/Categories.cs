using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Categories
    {

        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string CategoryName { get; set; }

        [Column(TypeName = "float")]
        public float Categoryexpenselimit { get; set; }
        
    }
}

