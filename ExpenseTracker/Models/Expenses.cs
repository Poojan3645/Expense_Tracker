using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expenses
    {
        [Key]
        public int ExpenseId { get; set; }

        //CategoryId
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Categories? Categories { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Description { get; set; }

        [Column(TypeName = "int")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
