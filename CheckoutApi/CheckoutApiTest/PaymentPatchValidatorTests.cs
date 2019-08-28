using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using ThiagoCampos.CheckoutApi.Validators;
using ThiagoCampos.Model;

namespace CheckoutApiTest
{
    [TestClass]
    public class PaymentPatchValidatorTests
    {
        private Fixture Fixture { get; } = new Fixture();

        [TestMethod]
        public async Task PaymentPatch_WhenNull_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentPatchValidator();
            var result = await validator.ValidateAsync((JsonPatchDocument<Payment>)null);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals($"Null{typeof(JsonPatchDocument<Payment>).Name}"));
        }

        [TestMethod]
        public async Task PaymentPatch_WhenOperationOverId_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentPatchValidator();
            var patchDocument = new JsonPatchDocument<Payment>().Replace(p => p.Id, this.Fixture.Create<Guid>());
            var result = await validator.ValidateAsync(patchDocument);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentIdOperationNotAllowed"));
        }

        [TestMethod]
        public async Task PaymentPatch_WhenOperationOverSubmissionDate_MustReturnCorrespondentErrorMessage()
        {
            var validator = new PaymentPatchValidator();
            var patchDocument = new JsonPatchDocument<Payment>().Replace(p => p.SubmissionDate, this.Fixture.Create<DateTime>());
            var result = await validator.ValidateAsync(patchDocument);
            result.Errors.Should().Contain(e => e.ErrorCode.Equals("PaymentSubmissionDateOperationNotAllowed"));
        }

        [TestMethod]
        public async Task PaymentPatch_WhenMultipleInvalidOperations_MustReturnMultipleErrorMessages()
        {
            var validator = new PaymentPatchValidator();
            var patchDocument = new JsonPatchDocument<Payment>()
                .Replace(p => p.Id, this.Fixture.Create<Guid>())
                .Replace(p => p.SubmissionDate, this.Fixture.Create<DateTime>());
            var result = await validator.ValidateAsync(patchDocument);
            result.Errors.Should().HaveCountGreaterThan(1);
        }
    }
}
