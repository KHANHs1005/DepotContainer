using DepotContainer.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepotContainer.Domain.Entities
{
    [Table("Container")]
    public class Container
    {
        [Key]
        [Column("cont_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContainerId { get; set; }

        [Column("cont_no")]
        [MaxLength(20)]
        [Required]
        public string ContainerNo { get; set; } = string.Empty;

        [Column("date_of_manufacture")]
        public DateTime? DateOfManufacture { get; set; }

        [Column("operator_name")]
        [MaxLength(100)]
        public string? OperatorName { get; set; }

        [Column("container_type")]
        public ContainerType? ContainerType { get; set; }

        [Column("is_empty")]
        public bool IsEmpty { get; set; }

        [Column("weight")]
        public double? Weight { get; set; }

        [Column("cont_status")]
        [MaxLength(20)]
        public string? ContStatus { get; set; }

        [Column("time_in")]
        public DateTime? TimeIn { get; set; }

        [Column("time_out")]
        public DateTime? TimeOut { get; set; }

        [Column("cont_condition")]
        [MaxLength(20)]
        public string? ContCondition { get; set; }

        [Column("cont_iso_id")]
        public int? ContIsoId { get; set; }

        [ForeignKey(nameof(ContIsoId))]
        public ContainerISO? ContIso { get; set; }

        [Column("slot_id")]
        public int? SlotId { get; set; }

        [ForeignKey(nameof(SlotId))]
        public Slot? Slot { get; set; }

        // ✅ Sửa đúng chuẩn — tránh nhầm với kiểu string
        [Column("booking_id")]
        public int? BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; } // 🔹 đổi từ BookingNumber → Booking

        public ICollection<Seal>? Seals { get; set; }
        public ICollection<EIR>? EIRs { get; set; }
        public ICollection<ContainerMovementHis>? ContainerMovementHis { get; set; }
    }
}
