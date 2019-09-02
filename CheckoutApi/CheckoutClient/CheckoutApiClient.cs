using Refit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApi.Client.Services
{
    public class CheckoutApiClient
    {
        private IPaymentsApi Client { get; }

        public CheckoutApiClient(IPaymentsApi client)
        {
            this.Client = client;
        }

        public async Task Run()
        {
            var payments = await this.Client.GetAll();
            Console.WriteLine(string.Join(Environment.NewLine, payments.Select(p => p.Id)));
        }
    }
}
