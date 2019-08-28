using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using ThiagoCampos.Model;

namespace ThiagoCampos.CheckoutApi.Validators
{
    public class PaymentPatchValidator : CheckoutApiValidator<JsonPatchDocument<Payment>>
    {
        public PaymentPatchValidator()
        {
            RuleForEach(x => x.Operations).SetValidator(new PaymentOperationValidator());
        }
    }
}