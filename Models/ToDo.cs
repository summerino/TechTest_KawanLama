using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechTest_KawanLama.Models
{
    [Table("Todo")]
    public class ToDo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string activities_no { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
}
