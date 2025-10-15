using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [Column("cus_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Column("name")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("tax_id")]
        [MaxLength(50)]
        public string? TaxId { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<EIR>? EIRs { get; set; }
    }
}
