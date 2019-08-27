using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using ThiagoCampos.CheckoutApi.Validators;
using ThiagoCampos.Model;

namespace CheckoutApiTest
{
    [TestClass]
    public class PaymentsValidatorTests
    {
        private Fixture Fixture { get; } = new Fixture();

        [TestMethod]
        public async Task PaymentsValidator_WhenNull_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var result = await validator.ValidateAsync((Payment)null);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("NullPayment"));
        }

        [TestMethod]
        public async Task PaymentsValidator_WhenIdEmpty_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var payment = this.Fixture.Build<Payment>().With(p => p.Id, Guid.Empty).Create();
            var result = await validator.ValidateAsync(payment);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentIdEmpty"));
        }

        [TestMethod]
        public async Task PaymentsValidator_WhenISOCurrencyCodeNull_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var payment = this.Fixture.Build<Payment>().Without(p => p.ISOCurrencyCode).Create();
            var result = await validator.ValidateAsync(payment);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalid")).And.NotContain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalidLength"));
        }

        [TestMethod]
        public async Task PaymentsValidator_WhenISOCurrencyCodeEmpty_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var payment = this.Fixture.Build<Payment>().With(p => p.ISOCurrencyCode, string.Empty).Create();
            var result = await validator.ValidateAsync(payment);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalid")).And.NotContain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalidLength"));
        }

        [TestMethod]
        public async Task PaymentsValidator_WhenISOCurrencyCodeTooShort_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var payment = this.Fixture.Create<Payment>();
            payment.ISOCurrencyCode = payment.ISOCurrencyCode.Substring(0, 1);
            var result = await validator.ValidateAsync(payment);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalidLength")).And.NotContain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalid"));
        }

        [TestMethod]
        public async Task PaymentsValidator_WhenISOCurrencyCodeTooLong_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var payment = this.Fixture.Create<Payment>();
            payment.ISOCurrencyCode = payment.ISOCurrencyCode.PadRight(10, '0');
            var result = await validator.ValidateAsync(payment);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalidLength")).And.NotContain(e => e.ErrorCode.Equals("PaymentISOCurrencyInvalid"));
        }

        [TestMethod]
        public async Task PaymentsValidator_WhenValueZero_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var payment = this.Fixture.Build<Payment>().With(p => p.Value, 0).Create();
            var result = await validator.ValidateAsync(payment);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentValueNotPositive"));
        }

        [TestMethod]
        public async Task PaymentsValidator_WhenValueNegative_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentsValidator();
            var payment = this.Fixture.Build<Payment>().With(p => p.Value, this.Fixture.Create<byte>() * -1).Create();
            var result = await validator.ValidateAsync(payment);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentValueNotPositive"));
        }
    }
}
