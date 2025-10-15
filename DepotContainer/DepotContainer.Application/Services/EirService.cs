using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;
using DepotContainer.Domain.Enums;
using DepotContainer.Application.Interfaces.Repositories;

namespace DepotContainer.Application.Services
{
    public class EirService : IEirService
    {
        private readonly IEirRepository _eirRepository;

        public EirService(IEirRepository eirRepository)
        {
            _eirRepository = eirRepository;
        }

        // ✅ Lấy toàn bộ danh sách EIR
        public async Task<IEnumerable<EirDto>> GetAllAsync()
        {   
            var eirs = await _eirRepository.GetAllAsync();

            return eirs.Select(e => new EirDto
            {
                EirId = e.EirId,
                EirNumber = e.EirNumber,
                Type = e.Type,
                ContainerNumber = e.Container?.ContainerNo ?? string.Empty,
                CustomerName = e.Customer?.Name ?? string.Empty,
                StaffName = e.Staff?.StaffName ?? string.Empty, // 🟢 thêm dòng này
                PlateNumber = e.PlateNumber,
                BatNo = e.BatNo,
                IssueDate = e.IssueDate,
                RegisAt = e.RegisAt
            });
        }

        // ✅ Lấy EIR theo ID
        public async Task<EirDto?> GetByIdAsync(int id)
        {
            var e = await _eirRepository.GetByIdAsync(id);
            if (e == null) return null;

            return new EirDto
            {
                EirId = e.EirId,
                EirNumber = e.EirNumber,
                Type = e.Type,
                ContainerNumber = e.Container?.ContainerNo ?? string.Empty,
                CustomerName = e.Customer?.Name ?? string.Empty,
                StaffName = e.Staff?.StaffName ?? string.Empty, // 🟢 thêm dòng này
                PlateNumber = e.PlateNumber,
                BatNo = e.BatNo,
                IssueDate = e.IssueDate,
                RegisAt = e.RegisAt
            };
        }

        // ✅ Tạo mới EIR
        public async Task<EirDto> CreateAsync(CreateEirDto dto)
        {
            var eir = new EIR
            {
                EirNumber = string.IsNullOrWhiteSpace(dto.EirNumber)
                    ? $"EIR-{DateTime.Now:yyyyMMddHHmmss}" // tự sinh nếu trống
                    : dto.EirNumber,
                Type = dto.Type,
                ContId = dto.ContainerId,
                CusId = dto.CustomerId,
                StaffId = dto.StaffId,
                BookingId = dto.BookingId,
                SealId = dto.SealId ?? null,
                PlateNumber = dto.PlateNumber,
                BatNo = dto.BatNo,
                RegisAt = dto.RegisAt ?? DateTime.Now,
                IssueDate = DateTime.Now
            };

            await _eirRepository.AddAsync(eir);
            return new EirDto
            {
                EirId = eir.EirId,
                EirNumber = eir.EirNumber,
                Type = eir.Type,
                ContainerNumber = eir.Container?.ContainerNo ?? string.Empty,
                CustomerName = eir.Customer?.Name ?? string.Empty,
                StaffName = eir.Staff?.StaffName ?? string.Empty, // 🟢 thêm dòng này
                PlateNumber = eir.PlateNumber,
                BatNo = eir.BatNo,
                IssueDate = eir.IssueDate,
                RegisAt = eir.RegisAt
            };
        }

        // ✅ Cập nhật EIR
        public async Task UpdateAsync(UpdateEirDto dto)
        {
            var eir = await _eirRepository.GetByIdAsync(dto.EirId);
            if (eir == null)
                throw new Exception("EIR not found.");

            // Cập nhật nếu có giá trị mới
            if (!string.IsNullOrWhiteSpace(dto.EirNumber)) eir.EirNumber = dto.EirNumber;
            if (dto.Type.HasValue) eir.Type = dto.Type.Value;
            if (dto.ContainerId.HasValue) eir.ContId = dto.ContainerId.Value;
            if (dto.CustomerId.HasValue) eir.CusId = dto.CustomerId.Value;
            if (dto.StaffId.HasValue) eir.StaffId = dto.StaffId.Value;
            if (dto.BookingId.HasValue) eir.BookingId = dto.BookingId.Value;
            if (dto.SealId.HasValue) eir.SealId = dto.SealId.Value;
            if (!string.IsNullOrWhiteSpace(dto.PlateNumber)) eir.PlateNumber = dto.PlateNumber;
            if (dto.BatNo.HasValue) eir.BatNo = dto.BatNo.Value;
            if (dto.RegisAt.HasValue) eir.RegisAt = dto.RegisAt.Value;

            await _eirRepository.UpdateAsync(eir);
        }

        // ✅ Xóa EIR
        public async Task DeleteAsync(int id)
        {
            var eir = await _eirRepository.GetByIdAsync(id);
            if (eir == null)
                throw new Exception("EIR not found.");

            await _eirRepository.DeleteAsync(eir);
        }
    }
}
