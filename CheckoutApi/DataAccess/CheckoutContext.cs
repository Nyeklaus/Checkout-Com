using AutoFixture;
using Microsoft.EntityFrameworkCore;
using System;
using ThiagoCampos.Model;

namespace ThiagoCampos.DataAccess
{
    public class CheckoutContext : DbContext
    {
        public CheckoutContext(DbContextOptions<CheckoutContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        public bool Loaded { get; private set; }

        /// <summary>
        /// Adds random data to the context
        /// </summary>
        /// <param name="paymentCount">Number of Payments to add</param>
        /// <returns>Task result</returns>
        public void AddRandomData(byte paymentCount = 20)
        {
            var fixture = new Fixture();
            fixture.Customize<Payment>(f => f
                .With(p => p.Visible, f.Create<byte>() % 2 == 0)
                .With(p => p.SubmissionDate, DateTime.Now.AddTicks((f.Create<long>() % DateTime.Now.AddYears(-1).Ticks) * -1))
            );

            Payments.AddRange(fixture.CreateMany<Payment>(paymentCount));
            this.Loaded = true;
            this.SaveChanges();
        }
    }
}
