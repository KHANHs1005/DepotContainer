using Microsoft.Extensions.DependencyInjection;
using DepotContainer.Application.Services;
using DepotContainer.Application.Interfaces.Services;


namespace DepotContainer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IContainerService, ContainerService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEirService, EirService>();
            services.AddScoped<IAuthService, AuthService>();


            return services;
        }
    }
}
