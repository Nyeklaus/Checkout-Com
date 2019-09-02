namespace CheckoutApi.Client.Services
{
    using Microsoft.AspNetCore.JsonPatch;
    using Refit;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ThiagoCampos.Model;

    public interface IPaymentsApi
    {
        [Get("")]
        Task<IEnumerable<Payment>> GetAll();

        [Get("/{id}")]
        Task<Payment> Get(Guid id);

        [Post("")]
        Task Submit([Body] Payment paymentRequest);

        [Patch("/{id}")]
        Task Update(Guid id, [Body] JsonPatchDocument<Payment> paymentPatch);

        [Delete("/{id}")]
        Task Delete(Guid id);
    }
}
