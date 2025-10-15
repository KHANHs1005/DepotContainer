using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("ContainerMovementHis")]
    public class ContainerMovementHis
    {
        [Key]
        [Column("his_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HisId { get; set; }

        [Column("staff_id")]
        public int? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public Staff? Staff { get; set; }

        [Column("slot_id")]
        public int? SlotId { get; set; }
        [ForeignKey("SlotId")]
        public Slot? Slot { get; set; }

        [Column("cont_id")]
        public int? ContId { get; set; }
        [ForeignKey("ContId")]
        public Container? Container { get; set; }

        [Column("move_at")]
        public DateTime MoveAt { get; set; }

        [Column("reason")]
        [MaxLength(200)]
        public string? Reason { get; set; }

        // status_his IN ('Current','History')
        [Column("status_his")]
        [MaxLength(20)]
        public string? StatusHis { get; set; }
    }
}
