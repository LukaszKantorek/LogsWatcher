using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogsWatcher.Model
{
    public class LogType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public LogTypeEnum TypeNumber { get; set; }
        [MaxLength(300)]
        public string DisplayName { get; set; }
    }

}
