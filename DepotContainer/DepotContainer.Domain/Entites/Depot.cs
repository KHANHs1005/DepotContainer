using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Depot")]
    public class Depot
    {
        [Key]
        [Column("depot_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepotId { get; set; }

        [Column("depot_name")]
        [MaxLength(100)]
        [Required]
        public string DepotName { get; set; } = string.Empty;

        [Column("address")]
        [MaxLength(200)]
        public string? Address { get; set; }

        [Column("position")]
        [MaxLength(100)]
        public string? Position { get; set; }

        public ICollection<Block>? Blocks { get; set; }
    }
}
