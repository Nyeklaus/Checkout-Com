namespace ThiagoCampos.CheckoutApi.Validators
{
    using FluentValidation;
    using FluentValidation.Results;

    public abstract class CheckoutApiValidator<T> : AbstractValidator<T> where T : class
    {
        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure("", "Empty object provided.") { ErrorCode = $"Null{typeof(T).Name}" });
                return false;
            }

            return true;
        }
    }
}