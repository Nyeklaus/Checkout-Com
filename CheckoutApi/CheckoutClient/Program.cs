namespace CheckoutApi.Client
{
    using CheckoutApi.Client.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<CheckoutApiClient>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var service = new ServiceCollection()
                .AddSingleton(config.Get<CheckoutApiClientConfiguration>())
                .AddSingleton<CheckoutApiClient>();
            service.AddRefitClient<IPaymentsApi>().ConfigureHttpClient((a, c) => c.BaseAddress = new Uri(a.GetService<CheckoutApiClientConfiguration>().ApiUrl));
            return service;
        }
    }
}
