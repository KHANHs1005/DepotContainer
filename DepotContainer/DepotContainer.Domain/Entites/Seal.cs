using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Seal")]
    public class Seal
    {
        [Key]
        [Column("seal_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SealId { get; set; }

        [Column("seal_no")]
        [MaxLength(50)]
        [Required]
        public string SealNo { get; set; } = string.Empty;

        // seal_owner IN ('shipping_line','customer','port')
        [Column("seal_owner")]
        [MaxLength(20)]
        public string? SealOwner { get; set; }

        [Column("seal_applied_at")]
        public DateTime? SealAppliedAt { get; set; }

        [Column("seal_removed_at")]
        public DateTime? SealRemovedAt { get; set; }

        [Column("cont_id")]
        public int ContId { get; set; }

        [ForeignKey("ContId")]
        public Container? Container { get; set; }
    }
}
