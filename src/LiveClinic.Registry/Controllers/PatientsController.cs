using System;
using System.Threading.Tasks;
using LiveClinic.Registry.Application.Commands;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Registry.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveClinic.Registry.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPatient(long id)
        {
            try
            {
                var res = await _mediator.Send(new GetPatientQuery(id));
                if (res.IsSuccess)
                    return Ok(res.Value);
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "Get Patient Error");
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            try
            {
                var res = await _mediator.Send(new GetPatientsQuery());
                if (res.IsSuccess)
                    return Ok(res.Value);
                throw new Exception(res.Error);
            }
            catch (Exception e)
            {
                Log.Error(e, "Get Patients Error");
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> RegisterPatient(NewPatientDto patient)
        {
            try
            {
                var res = await _mediator.Send(new RegisterPatientCommand(patient));
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

        [HttpPost("Encounter")]
        public async Task<IActionResult> NewPatientEncounter(NewEncounterDto encounter)
        {
            try
            {
                var res = await _mediator.Send(new CreateEncounterCommand(encounter));
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