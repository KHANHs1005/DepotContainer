using DepotContainer.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DepotContainer.Domain.Entities
{
    [Table("EIR")]
    public class EIR
    {
        [Key]
        [Column("eir_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EirId { get; set; }

        [Required]
        [Column("EirNumber")]
        [MaxLength(50)]
        public string EirNumber { get; set; } = string.Empty;

        [Required]
        [Column("IssueDate")]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required]
        [Column("EirType", TypeName = "nvarchar(20)")]
        [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ Giúp serialize/deserialze enum dạng chuỗi
        public EirType Type { get; set; }

        [Column("regis_at")]
        public DateTime RegisAt { get; set; } = DateTime.UtcNow;

        [Column("bat_no")]
        public int? BatNo { get; set; }

        // 🔹 Staff (Người xử lý)
        [Column("staff_id")]
        public int? StaffId { get; set; }

        [ForeignKey(nameof(StaffId))]
        public Staff? Staff { get; set; }

        // 🔹 Customer (Khách hàng)
        [Column("cus_id")]
        public int? CusId { get; set; }

        [ForeignKey(nameof(CusId))]
        public Customer? Customer { get; set; }

        // 🔹 Booking (Liên kết 1 Booking có nhiều EIR)
        [Column("booking_id")]
        public int? BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; }

        // 🔹 Container
        [Column("cont_id")]
        public int? ContId { get; set; }

        [ForeignKey(nameof(ContId))]
        public Container? Container { get; set; }

        // 🔹 Biển số xe
        [Column("plate_number")]
        [MaxLength(50)]
        public string? PlateNumber { get; set; }

        // 🔹 Seal (nếu bạn bỏ thì để nullable)
        [Column("seal_id")]
        public int? SealId { get; set; }

        [ForeignKey(nameof(SealId))]
        public Seal? Seal { get; set; }
    }
}
