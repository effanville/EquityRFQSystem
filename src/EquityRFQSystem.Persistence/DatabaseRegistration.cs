using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EquityRFQSystem.Persistence;

public static class DatabaseRegistration
{
    public static IServiceCollection AddEquitySystemPersistence(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<PersistenceContext>(options =>
            options.UseInMemoryDatabase("EquityRFQPersistence"));
        serviceCollection.AddScoped<IPersistence, Persistence>();
        return serviceCollection;
    }
}