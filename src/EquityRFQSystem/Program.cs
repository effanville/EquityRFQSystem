using EquityRFQSystem.Common;
using EquityRFQSystem.Persistence;
using EquityRFQSystem.RealTimeQuote;
using EquityRFQSystem.Services;

namespace EquityRFQSystem;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss] ";
        });

        builder.Services.AddSingleton<IRealTimeQuoteProvider, RealTimeQuoteProvider>();
        builder.Services.AddSingleton(x => Random.Shared);
        builder.Services.AddSingleton<IClock, Clock>();
        builder.Services.AddScoped<IQuoteRetrieverService, QuoteRetrieverService>();

        builder.Services.AddEquitySystemPersistence();
        
        builder.Services.AddControllers();
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection()
            .UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}
