using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.Text.RegularExpressions;
using ThiagoCampos.Model;

namespace ThiagoCampos.CheckoutApi.Validators
{
    public class PaymentOperationValidator : CheckoutApiValidator<Operation<Payment>>
    {
        public PaymentOperationValidator()
        {
            RuleFor(o => o.path).Must((a) => !Regex.Match(a, $"/{nameof(Payment.Id)}", RegexOptions.IgnoreCase).Success).WithErrorCode("PaymentIdOperationNotAllowed");
            RuleFor(o => o.path).Must((a) => !Regex.Match(a, $"/{nameof(Payment.SubmissionDate)}", RegexOptions.IgnoreCase).Success).WithErrorCode("PaymentSubmissionDateOperationNotAllowed");
        }
    }
}