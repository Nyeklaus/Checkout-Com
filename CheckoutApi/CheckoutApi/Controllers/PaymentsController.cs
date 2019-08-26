using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThiagoCampos.DataAccess;
using ThiagoCampos.Model;

namespace ThiagoCampos.CheckoutApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly CheckoutContext _context;

        public PaymentsController(CheckoutContext context)
        {
            _context = context;

            if (!_context.Loaded)
            {
                _context.AddRandomData();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAll()
        {
            return await _context.Payments.ToListAsync();
        }

        /// <summary>
        /// Requests a payment information based on its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the payment</param>
        /// <returns>Recorded payment</returns>
        /// <response code="200">Returns the payment data</response>
        /// <response code="404">If the item is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Payment>> Get(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            return payment == null ? NotFound() : (ActionResult<Payment>)payment;
        }

        /// <summary>
        /// Submits a payment
        /// </summary>
        /// <param name="paymentRequest">Payment being submitted</param>
        /// <returns>Result of the payment request</returns>
        /// <response code="201">Success status along with the generated route of the payment in the header</response>
        /// <response code="409">If the payment cannot be received due to the existence of a previous one with the same unique identifier</response>            
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post([FromBody] Payment paymentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if ((await _context.Payments.FindAsync(paymentRequest.Id)) != null)
            {
                return Conflict();
            }

            _context.Payments.Add(paymentRequest);
            await _context.SaveChangesAsync();

            return CreatedAtRoute(nameof(Get), new { id = paymentRequest.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Put(Guid id, [FromBody] Payment paymentRequest)
        {
            return Ok();
        }

        /// <summary>
        /// Requests a payment information based on its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the payment</param>
        /// <returns>Recorded payment</returns>
        /// <response code="204">If the delete action was executed</response>
        /// <response code="404">If the item is not found</response>            
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.Visible = false;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
