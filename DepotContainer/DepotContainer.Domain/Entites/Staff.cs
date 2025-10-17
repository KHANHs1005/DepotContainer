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

        [Column("staff_type")]
        [MaxLength(30)]

        public string? StaffType { get; set; }
        [Column("username")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        public string? Password { get; set; }


        public ICollection<EIR>? EIRs { get; set; }
        public ICollection<ContainerMovementHis>? ContainerMovementHis { get; set; }
    }
}
