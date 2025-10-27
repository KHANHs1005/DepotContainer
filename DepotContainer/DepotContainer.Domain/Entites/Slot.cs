using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Slot")]
    public class Slot
    {
        [Key]
        [Column("slot_id")]
        public int SlotId { get; set; }
        public int Bay { get; set; }
        public int Row { get; set; }
        public int Tier { get; set; }

        [Column("status_slot")]
        [MaxLength(20)]
        public string StatusSlot { get; set; } = "Empty";

        [Column("block_id")] // ✅ thêm dòng này
        public int BlockId { get; set; }

        [ForeignKey("BlockId")]
        public Block? Block { get; set; }
        public Container? Container { get; set; }
    }

}
