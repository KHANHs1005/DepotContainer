using DepotContainer.Infrastructure.Data;

namespace DepotContainer.Infrastructure.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly DepotDbContext _context;

        public ContainerRepository Containers { get; }
        public BookingRepository Bookings { get; }
        public EirRepository Eirs { get; }
        public CustomerRepository Customers { get; }

        public UnitOfWork(DepotDbContext context)
        {
            _context = context;
            Containers = new ContainerRepository(_context);
            Bookings = new BookingRepository(_context);
            Eirs = new EirRepository(_context);
            Customers = new CustomerRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
