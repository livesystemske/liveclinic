using System;
using System.Threading.Tasks;
using LiveClinic.Billing.Application.Commands;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Billing.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveClinic.Billing.Controllers
{
    
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class BillsController : ControllerBase
    {
         private readonly IMediator _mediator;
        
        public BillsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var res = await _mediator.Send(new GetBillsQuery());
                if (res.IsSuccess)
                    return Ok(res.Value);
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "Get Prices Error");
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("Patient/{patientId}")]
        public async Task<IActionResult> GetPatientBill(long patientId)
        {
            try
            {
                var res = await _mediator.Send(new GetPatientBillQuery(patientId));
                if (res.IsSuccess)
                    return Ok(res.Value);
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "Get Patient Bill Error");
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("Prices")]
        public async Task<IActionResult> GetPrices()
        {
            try
            {
                var res = await _mediator.Send(new GetServicePricesQuery());
                if (res.IsSuccess)
                    return Ok(res.Value);
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "Get Prices Error");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateBill(NewBillDto bill)
        {
            try
            {
                var res = await _mediator.Send(new GenerateBillCommand(bill));
                if (res.IsSuccess)
                    return Ok();
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "RegisterPatient Error");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Item")]
        public async Task<IActionResult> NewBillItem(NewBillItemDto billItem)
        {
            try
            {
                var res = await _mediator.Send(new AddBillItemCommand(billItem));
                if (res.IsSuccess)
                    return Ok();
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "NewPatientEncounter Error");
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost("Payment")]
        public async Task<IActionResult> NewBillPayment(NewPaymentDto payment)
        {
            try
            {
                var res = await _mediator.Send(new AddPaymentCommand(payment));
                if (res.IsSuccess)
                    return Ok();
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "NewPatientEncounter Error");
                return StatusCode(500, e.Message);
            }
        }
    }
}