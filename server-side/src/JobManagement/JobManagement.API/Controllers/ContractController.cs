using JobManagement.API.Extensions;
using JobManagement.API.Security;
using JobManagement.Application.Commands.ContractCommands;
using JobManagement.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobManagement.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ContractController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public ContractController(IMediator mediator, IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }

        //[HttpGet("[controller]/freelancer")]
        //[Authorize(Roles = "Freelancer")]
        //public async Task<ActionResult<List<ContractViewModel>>> GetByFreelancer()
        //{
        //    var freelancerId = _identityService.GetDomainUserId();
        //    return await _contractQueries.GetByFreelancer(freelancerId);
        //}

        [HttpPost("job/{id}/[controller]/proposal/{proposalId}")]
        [Authorize(Roles = "Freelancer")]
        public async Task<ActionResult> Create(Guid id, Guid proposalId)
        {
            var command = new MakeContractCommand(id, proposalId);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok();
        }

        [HttpPut("job/{id}/[controller]/{contractId}/status/finished")]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult> Finish(Guid id, Guid contractId)
        {
            var command = new FinishContractCommand(id, contractId);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok();
        }

        [HttpPut("job/{id}/[controller]/{contractId}/status/terminated")]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult> Terminate(Guid id, Guid contractId)
        {
            var command = new TerminateContractCommand(id, contractId);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok();
        }

    }
}
