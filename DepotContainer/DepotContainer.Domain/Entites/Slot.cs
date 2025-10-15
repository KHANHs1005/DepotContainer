using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Slot")]
    public class Slot
    {
        [Key]
        [Column("slot_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlotId { get; set; }

        [Column("bay")]
        public int Bay { get; set; }

        [Column("row")]
        public int Row { get; set; }

        [Column("tier")]
        public int Tier { get; set; }

        // DB lưu 'Empty' / 'Full' -> string trong entity
        [Column("status_slot")]
        [MaxLength(20)]
        public string StatusSlot { get; set; } = "Empty";

        [Column("block_id")]
        public int BlockId { get; set; }

        [ForeignKey("BlockId")]
        public Block? Block { get; set; }

        public ICollection<Container>? Containers { get; set; }
    }
}
