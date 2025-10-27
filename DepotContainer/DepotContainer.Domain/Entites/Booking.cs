using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        [Column("booking_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        [Column("booking_number")]
        [MaxLength(50)]
        [Required]
        public string BookingNumber { get; set; } = string.Empty;
        [Column("cont_size")]
        [MaxLength(20)]
        public string? ContSize { get; set; }
        [Column("cont_quantity")]
        public int? ContQuantity { get; set; }
        [Column("operator_name")]
        [MaxLength(100)]
        public string? OperatorName { get; set; }
        [Column("release_expire_date")]
        public DateTime? ReleaseExpireDate { get; set; }
        [Column("cus_id")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        public virtual ICollection<Container> Container { get; set; }
        public ICollection<EIR>? EIRs { get; set; } = new List<EIR>();
    }
}
