using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Staff")]
    public class Staff
    {
        [Key]
        [Column("staff_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }

        [Column("staff_name")]
        [MaxLength(100)]
        [Required]
        public string StaffName { get; set; } = string.Empty;

        [Column("contact_phone")]
        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        // staff_type IN ('cont_handler','gate_administrator','doc_staff','admin')
        [Column("staff_type")]
        [MaxLength(30)]
        public string? StaffType { get; set; }

        public ICollection<EIR>? EIRs { get; set; }
        public ICollection<ContainerMovementHis>? ContainerMovementHis { get; set; }
    }
}
