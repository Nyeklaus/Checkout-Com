namespace ThiagoCampos.Model
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Payment data
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Unique identifier of the payment request
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Defines if the payment is still generally visible in the platform
        /// </summary>
        [DefaultValue(true)]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Value of the purchase
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// ISO code of the currency of the payment
        /// </summary>
        [StringLength(3, MinimumLength = 2)]
        public string ISOCurrencyCode { get; set; }

        /// <summary>
        /// Date and time when the payment was submitted
        /// </summary>
        public DateTime SubmissionDate { get; set; }
    }
}
