using AutoMapper;
using JobManagement.API.Extensions;
using JobManagement.API.Security;
using JobManagement.Application.Commands.ProposalCommands;
using JobManagement.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobManagement.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public ProposalController(
            IMediator mediator, 
            IIdentityService identityService, 
            IMapper mapper)
        {
            _mediator = mediator;
            _identityService = identityService;
            _mapper = mapper;
        }

        //[HttpGet("[controller]/{proposalId}")]
        //public async Task<ActionResult<ProposalViewModel>> GetById(Guid proposalId)
        //{
        //    var result = await _proposalQueries.GetByIdAsync(proposalId);
        //    if (result is null)
        //        return BadRequest();
        //    return Ok(result);
        //}

        //[HttpGet("{proposalId}/answers")]
        //public async Task<ActionResult<List<AnswerViewModel>>> GetAnswers(Guid proposalId)
        //{
        //    return await _answerQueries.GetByProposalAsync(proposalId);
        //}

        [HttpPost("job/{id}/[controller]")]
        [Authorize(Roles = "Freelancer")]
        public async Task<ActionResult<ProposalViewModel>> Create(CreateProposalCommand command)
        {
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Accepted(_mapper.Map<ProposalViewModel>(commandResult.Value));
        }

        [HttpPut("job/{id}/[controller]/{proposalId}/payment")]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult> UpdatePayment([FromBody] UpdateProposalPaymentCommand command)
        {
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok();
        }

        [HttpPut("job/{id}/[controller]/{proposalId}/status/approved")]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult> Approve(Guid id, Guid proposalId)
        {
            var command = new ApproveProposalCommand(id, proposalId);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok();
        }

    }
}
