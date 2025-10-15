using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IEnumerable<StaffDto>> GetAllAsync()
        {
            var staffs = await _staffRepository.GetAllAsync();
            return staffs.Select(s => new StaffDto
            {
                StaffId = s.StaffId,
                StaffName = s.StaffName,
                ContactPhone = s.ContactPhone,
                StaffType = s.StaffType,
                IsActive = s.IsActive
            });
        }

        public async Task<StaffDto?> GetByIdAsync(int id)
        {
            var s = await _staffRepository.GetByIdAsync(id);
            if (s == null) return null;

            return new StaffDto
            {
                StaffId = s.StaffId,
                StaffName = s.StaffName,
                ContactPhone = s.ContactPhone,
                StaffType = s.StaffType,
                IsActive = s.IsActive
            };
        }

        public async Task<StaffDto> CreateAsync(CreateStaffDto dto)
        {
            var staff = new Staff
            {
                StaffName = dto.StaffName,
                ContactPhone = dto.ContactPhone,
                StaffType = dto.StaffType,
                IsActive = dto.IsActive
            };

            await _staffRepository.AddAsync(staff);

            return new StaffDto
            {
                StaffId = staff.StaffId,
                StaffName = staff.StaffName,
                ContactPhone = staff.ContactPhone,
                StaffType = staff.StaffType,
                IsActive = staff.IsActive
            };
        }

        public async Task UpdateAsync(UpdateStaffDto dto)
        {
            var staff = await _staffRepository.GetByIdAsync(dto.StaffId);
            if (staff == null)
                throw new Exception("Staff not found.");

            if (!string.IsNullOrWhiteSpace(dto.StaffName)) staff.StaffName = dto.StaffName;
            if (!string.IsNullOrWhiteSpace(dto.ContactPhone)) staff.ContactPhone = dto.ContactPhone;
            if (!string.IsNullOrWhiteSpace(dto.StaffType)) staff.StaffType = dto.StaffType;
            if (dto.IsActive.HasValue) staff.IsActive = dto.IsActive.Value;

            await _staffRepository.UpdateAsync(staff);
        }

        public async Task DeleteAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
                throw new Exception("Staff not found.");

            await _staffRepository.DeleteAsync(staff);
        }
    }
}
