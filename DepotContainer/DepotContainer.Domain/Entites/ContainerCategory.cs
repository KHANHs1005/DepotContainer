    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("ContainerCategory")]
    public class ContainerCategory
    {
        [Key]
        [Column("category_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Column("category_name")]
        [MaxLength(50)]
        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Column("description")]
        [MaxLength(200)]
        public string? Description { get; set; }

        [Column("category_code")]
        [MaxLength(10)]
        public string? CategoryCode { get; set; }

        public ICollection<ContainerISO>? ContainerISOs { get; set; }
    }
}
