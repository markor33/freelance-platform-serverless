using AutoMapper;
using JobManagement.API.Extensions;
using JobManagement.API.Security;
using JobManagement.Application.Commands.JobCommands;
using JobManagement.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public partial class JobController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public JobController(
            IMediator mediator,
            IMapper mapper,
            IIdentityService identityService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _identityService = identityService;
        }

        [HttpPost]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult<JobViewModel>> Create(CreateJobCommand command)
        {
            command.ClientId = _identityService.GetUserId();
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok(_mapper.Map<JobViewModel>(commandResult.Value));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult<JobViewModel>> Update(UpdateJobCommand command)
        {
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok(_mapper.Map<JobViewModel>(commandResult.Value));
        }

        [HttpPut("{id}/status/done")]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult> Done(Guid id)
        {
            var command = new JobDoneCommand(id);
            var commandResult = await _mediator.Send(command);
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employeer")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var commandResult = await _mediator.Send(new DeleteJobCommand(id));
            if (commandResult.IsFailed)
                return BadRequest(commandResult.Errors.ToStringList());
            return Ok();
        }

    }
}
