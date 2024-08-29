using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechTest_KawanLama.Models
{
    [Table("SequenceNumber")]
    public class SequenceNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SequenceName { get; set; }
        public string Format { get; set; }
        public int LastRunNo { get; set; }
    }
}
