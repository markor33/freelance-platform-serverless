using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
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

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        if (id != sub)
        {
            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var command = JsonSerializer.Deserialize<UpdateEducationCommand>(request.Body);
        command.FreelancerId = Guid.Parse(sub);

        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
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
            StatusCode = statusCode
        };
    }

    public async Task<Result> CommandHandler(UpdateEducationCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);

            freelancer.UpdateEducation(request.EducationId, request.SchoolName, request.Degree, new DateRange(request.Start, request.End));

            await _freelancerRepository.SaveAsync(freelancer);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError(ex.ToString());
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
