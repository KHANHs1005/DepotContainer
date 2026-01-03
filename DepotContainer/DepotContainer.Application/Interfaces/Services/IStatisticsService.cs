namespace DepotContainer.Application.Interfaces.Services
{
    public interface    IStatisticsService
    {
        Task<object> GetDashboardStatisticsAsync();
    }
}