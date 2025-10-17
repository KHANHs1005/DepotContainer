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

        // Lấy danh sách toàn bộ nhân viên
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

        // Lấy chi tiết nhân viên theo ID
        public async Task<StaffDto?> GetByIdAsync(int id)
        {
            var s = await _staffRepository.GetByIdAsync(id);
            if (s == null)
                return null;

            return new StaffDto
            {
                StaffId = s.StaffId,
                StaffName = s.StaffName,
                ContactPhone = s.ContactPhone,
                StaffType = s.StaffType,
                IsActive = s.IsActive
            };
        }

        //  Tạo nhân viên mới
        public async Task<StaffDto> CreateAsync(CreateStaffDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.StaffName))
                throw new Exception("Tên nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new Exception("Tên đăng nhập không được để trống.");
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Mật khẩu không được để trống.");
            // kiểm tra username trùng
            var existing = await _staffRepository.GetByUsernameAsync(dto.Username);
            if (existing != null)
                throw new Exception("Tên đăng nhập đã tồn tại.");

            // Tạo hash mật khẩu
            var newStaff = new Staff
            {
                StaffName = dto.StaffName.Trim(),
                ContactPhone = dto.ContactPhone,
                StaffType = dto.StaffType,
                Username = dto.Username,
                Password = dto.Password,  // gán thẳng password
                IsActive = dto.IsActive
            };

            await _staffRepository.AddAsync(newStaff);

            return new StaffDto
            {
                StaffId = newStaff.StaffId,
                StaffName = newStaff.StaffName,
                ContactPhone = newStaff.ContactPhone,
                StaffType = newStaff.StaffType,
                IsActive = newStaff.IsActive
            };
        }

        // Cập nhật nhân viên
        public async Task UpdateAsync(UpdateStaffDto dto)
        {
            var staff = await _staffRepository.GetByIdAsync(dto.StaffId);
            if (staff == null)
                throw new Exception($"Không tìm thấy nhân viên có ID {dto.StaffId}.");

            // Cập nhật có chọn lọc
            if (!string.IsNullOrWhiteSpace(dto.StaffName))
                staff.StaffName = dto.StaffName.Trim();

            if (!string.IsNullOrWhiteSpace(dto.ContactPhone))
                staff.ContactPhone = dto.ContactPhone;

            if (!string.IsNullOrWhiteSpace(dto.StaffType))
                staff.StaffType = dto.StaffType;

            if (dto.IsActive.HasValue)
                staff.IsActive = dto.IsActive.Value;

            // Có thể thêm Username, Password nếu cần
            if (!string.IsNullOrWhiteSpace(dto.Username))
                staff.Username = dto.Username;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                staff.Password = dto.Password;

            await _staffRepository.UpdateAsync(staff);
        }

        // Xoá nhân viên (vô hiệu hoá tài khoản)
        public async Task DeleteAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
                throw new Exception($"Không tìm thấy nhân viên có ID {id}.");

            if (!staff.IsActive)
                throw new Exception("Nhân viên này đã bị vô hiệu hoá trước đó.");

            staff.IsActive = false;
            await _staffRepository.UpdateAsync(staff);
        }

        //Khôi phục nhân viên đã xoá 
        public async Task RestoreAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
                throw new Exception($"Không tìm thấy nhân viên có ID {id}.");

            if (staff.IsActive)
                throw new Exception("Nhân viên này đang hoạt động, không cần khôi phục.");

            staff.IsActive = true;
            await _staffRepository.UpdateAsync(staff);
        }
    }
}
