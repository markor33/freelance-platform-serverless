using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;
namespace EducationCommands.Lambda.Commands;

public class UpdateEducationCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<UpdateEducationCommand> _validator;
    private ILambdaContext _context;

    public UpdateEducationCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new EditEducationCommandValidator();
    }

    public UpdateEducationCommandHandler(IFreelancerRepository freelancerRepository, IValidator<UpdateEducationCommand> validator, ILambdaContext context)
    {
        _freelancerRepository = freelancerRepository;
        _validator = validator;
        _context = context;
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        if (id != sub)
        {
            context.Logger.LogError("Education update failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var command = JsonSerializer.Deserialize<UpdateEducationCommand>(request.Body, JsonOptions.Options);
        command.FreelancerId = Guid.Parse(sub);

        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            context.Logger.LogError($"Validation failed - {validationResult.Errors}");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 400,
                Body = $"Validation failed - {validationResult.Errors}"
            };
        }

        var result = await CommandHandler(command);
        var statusCode = result.IsSuccess ? 201 : 400;

        return new APIGatewayProxyResponse()
        {
            StatusCode = statusCode,
            Headers = Headers.CORS
        };
    }

    public async Task<Result> CommandHandler(UpdateEducationCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
            {
                _context.Logger.LogError($"Freelander with {request.FreelancerId} does not exist");

                return Result.Fail("Education update failed");
            }

            freelancer.UpdateEducation(request.EducationId, request.SchoolName, request.Degree, new DateRange(request.Start, request.End));
            await _freelancerRepository.SaveAsync(freelancer);

            _context.Logger.LogInformation($"Education successfully updated - Id:{request.EducationId}");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"Exception - {ex}");

            return Result.Fail("Education update action failed");
        }
    }
}

public class UpdateEducationCommand
{
    public Guid FreelancerId { get; set; }
    public Guid EducationId { get; set; }
    public string SchoolName { get; set; }
    public string Degree { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class EditEducationCommandValidator : AbstractValidator<UpdateEducationCommand>
{
    public EditEducationCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.EducationId).NotEmpty();

        RuleFor(x => x.SchoolName).NotEmpty();

        RuleFor(x => x.Degree).NotEmpty();

        RuleFor(x => x.Start).NotEmpty();

        RuleFor(x => x.End).NotEmpty();
    }
}
