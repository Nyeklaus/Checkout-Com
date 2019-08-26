﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Payment>>> Get()
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
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Payment>> Get(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            return payment == null ? NotFound() : (ActionResult<Payment>)payment;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Put(int id, [FromBody] Payment paymentRequest)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return Conflict();
            }

            _context.Payments.Add(payment);

            return Ok();
        }

        /// <summary>
        /// Requests a payment information based on its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the payment</param>
        /// <returns>Recorded payment</returns>
        /// <response code="200">Returns the payment data</response>
        /// <response code="404">If the item is not found</response>            
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            return payment == null ? (StatusCodeResult)NotFound() : Ok();
        }
    }
}