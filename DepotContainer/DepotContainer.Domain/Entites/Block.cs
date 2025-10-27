using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Block")]
    public class Block
    {
        [Key]
        [Column("block_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlockId { get; set; }

        [Column("block_name")]
        [MaxLength(50)]
        [Required]
        public string BlockName { get; set; } = string.Empty;

        [Column("max_tiers")]
        public int MaxTiers { get; set; }

        [Column("max_rows")]
        public int MaxRows { get; set; }

        [Column("max_bays")]
        public int MaxBays { get; set; }

        [Column("block_capacity")]
        public int BlockCapacity { get; set; }

        [Column("is_virtual")]
        public bool IsVirtual { get; set; }

        [Column("description")]
        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<Slot>? Slots { get; set; } = new List<Slot>();
    }
}
