using FluentValidation;

namespace ThiagoCampos.CheckoutApi.Validators
{
    public abstract class CheckoutApiValidator<T> : AbstractValidator<T> where T: class
    {
    }
}