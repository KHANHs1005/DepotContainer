using System.ComponentModel.DataAnnotations;

namespace DepotContainer.Application.DTOs
{
    public class ContainerDto
    {
        public int ContainerId { get; set; }

        [Required]
        public string ContainerNumber { get; set; } = string.Empty;

        public string? OperatorName { get; set; }

        public DateTime? DateOfManufacture { get; set; }

        public bool? IsEmpty { get; set; }

        public int? Weight { get; set; }

        public string? ContStatus { get; set; }

        public string? ContCondition { get; set; }

        public string? Size { get; set; }

        public string? Type { get; set; }

        public string? CurrentBlock { get; set; }

        public int? SlotId { get; set; }

        public int? Bay { get; set; }

        public int? Row { get; set; }

        public int? Tier { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }
    }

    public class CreateContainerDto
    {
        [Required]
        public string ContainerNumber { get; set; } = string.Empty;

        public string OperatorName { get; set; } = string.Empty;

        public string? Size { get; set; }

        public string? Type { get; set; }

        public DateTime? DateOfManufacture { get; set; }

        public bool? IsEmpty { get; set; }

        public int? Weight { get; set; }

        public string? ContStatus { get; set; }

        public string? ContCondition { get; set; }

        public string? CurrentBlock { get; set; }

        public int? SlotId { get; set; }

        public int? Bay { get; set; }

        public int? Row { get; set; }

        public int? Tier { get; set; }

        public DateTime? TimeIn { get; set; }
    }

    public class UpdateContainerDto
    {
        [Required]
        public int ContainerId { get; set; }

        [Required]
        public string ContainerNumber { get; set; } = string.Empty;

        public string? OperatorName { get; set; }

        public DateTime? DateOfManufacture { get; set; }

        public bool? IsEmpty { get; set; }

        public int? Weight { get; set; }

        public string? ContStatus { get; set; }

        public string? ContCondition { get; set; }

        public string? Size { get; set; }

        public string? Type { get; set; }

        public string? CurrentBlock { get; set; }

        public int? SlotId { get; set; }

        public int? Bay { get; set; }

        public int? Row { get; set; }

        public int? Tier { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }
    }
}
