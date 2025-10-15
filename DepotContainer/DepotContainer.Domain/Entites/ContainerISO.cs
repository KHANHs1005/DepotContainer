using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("ContainerISO")]
    public class ContainerISO
    {
        [Key]
        [Column("cont_iso_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContIsoId { get; set; }

        [Column("iso_code")]
        [MaxLength(20)]
        [Required]
        public string IsoCode { get; set; } = string.Empty;

        [Column("length")]
        public double? Length { get; set; }

        [Column("height")]
        public double? Height { get; set; }

        [Column("width")]
        public double? Width { get; set; }

        [Column("maximum_weight")]
        public double? MaximumWeight { get; set; }

        [Column("tare_weight")]
        public double? TareWeight { get; set; }

        [Column("description")]
        [MaxLength(200)]
        public string? Description { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public ContainerCategory? Category { get; set; }

        public ICollection<Container>? Containers { get; set; }
    }
}
