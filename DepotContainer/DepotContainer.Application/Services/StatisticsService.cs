using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IContainerRepository _containerRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly IEirRepository _eirRepo;
        private readonly ISlotRepository _slotRepo;

        public StatisticsService(
            IContainerRepository containerRepo,
            IBookingRepository bookingRepo,
            IEirRepository eirRepo,
            ISlotRepository slotRepo    )
        {
            _containerRepo = containerRepo;
            _bookingRepo = bookingRepo;
            _eirRepo = eirRepo;
            _slotRepo = slotRepo;
        }

        public async Task<object> GetDashboardStatisticsAsync()
        {
            var allContainers = (await _containerRepo.GetAllAsync()).ToList();
            var allBookings = (await _bookingRepo.GetAllAsync()).ToList();
            var allEirs = (await _eirRepo.GetAllAsync()).ToList();
            var allSlots = (await _slotRepo.GetAllAsync()).ToList();

            // ===== TỔNG QUAN =====
            var emptyContainerCount = allContainers.Count(c => c.IsEmpty || c.ContStatus == "Empty");
            var bookingCount = allBookings.Count();
            var eirCount = allEirs.Count();
            var slotCount = allSlots.Count();
            var emptySlotCount = 45; // tạm hard-code

            // ===== THỐNG KÊ THEO HÃNG TÀU =====
            var operatorStats = allContainers
                .Where(c => !string.IsNullOrEmpty(c.OperatorName))
                .GroupBy(c => c.OperatorName)
                .Select(g => new
                {
                    OperatorName = g.Key,
                    Import = g.Count(c => c.TimeIn.HasValue),
                    Export = g.Count(c => c.TimeOut.HasValue),
                    Stock0To10 = g.Count(c =>
                        c.TimeIn.HasValue &&
                        !c.TimeOut.HasValue &&
                        (DateTime.Now - c.TimeIn.Value).TotalDays <= 10),
                    Stock10Plus = g.Count(c =>
                        c.TimeIn.HasValue &&
                        !c.TimeOut.HasValue &&
                        (DateTime.Now - c.TimeIn.Value).TotalDays > 10)
                })
                .OrderByDescending(o => o.Import)
                .ToList();

            // ===== TRẢ VỀ KẾT QUẢ =====
            return new
            {
                Summary = new
                {
                    EmptyContainer = emptyContainerCount,
                    Booking = bookingCount,
                    Eir = eirCount,
                    Slot = emptySlotCount
                },
                operatorName = operatorStats  // ✅ key này Angular của bạn đang đọc
            };
        }
    }
}
