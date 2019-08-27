using FluentValidation;
using ThiagoCampos.Model;

namespace ThiagoCampos.CheckoutApi.Validators
{
    public class PaymentsValidator : CheckoutApiValidator<Payment>
    {
        public PaymentsValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithErrorCode("PaymentIdEmpty");
            RuleFor(x => x.ISOCurrencyCode).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithErrorCode("PaymentISOCurrencyInvalid").Length(2, 3).WithErrorCode("PaymentISOCurrencyInvalidLength");
            RuleFor(x => x.Value).GreaterThan(0).WithErrorCode("PaymentValueNotPositive");
        }
    }
}