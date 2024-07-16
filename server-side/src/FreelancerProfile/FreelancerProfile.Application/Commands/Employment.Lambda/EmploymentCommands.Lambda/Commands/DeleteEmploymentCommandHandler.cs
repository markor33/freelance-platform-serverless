using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using WriteModel;

namespace EmploymentCommands.Lambda.Commands;

public class DeleteEmploymentCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<DeleteEmploymentCommand> _validator;
    private ILambdaContext _context;

    public DeleteEmploymentCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new DeleteEmploymentCommandValidator();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        var employmentId = request.PathParameters["employmentId"];
        if (id != sub || employmentId is null)
        {
            context.Logger.LogError("Employment delete failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var command = new DeleteEmploymentCommand(Guid.Parse(id), Guid.Parse(employmentId));

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
            StatusCode = statusCode
        };
    }

    public async Task<Result> CommandHandler(DeleteEmploymentCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
            {
                _context.Logger.LogError($"Freelander with {request.FreelancerId} does not exist");

                return Result.Fail("Employment delete failed");
            }

            freelancer.DeleteEmployment(request.EmploymentId);

            await _freelancerRepository.SaveAsync(freelancer);

            _context.Logger.LogInformation($"Employment successfully deleted - Id:{request.EmploymentId}");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"Employment delete failed with exception - {ex}");

            return Result.Fail("Delete employment action failed");
        }
    }

}

public class DeleteEmploymentCommand
{
    public Guid FreelancerId { get; set; }
    public Guid EmploymentId { get; set; }

    public DeleteEmploymentCommand(Guid freelancerId, Guid employmentId)
    {
        FreelancerId = freelancerId;
        EmploymentId = employmentId;
    }
}

public class DeleteEmploymentCommandValidator : AbstractValidator<DeleteEmploymentCommand>
{
    public DeleteEmploymentCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.EmploymentId).NotEmpty();
    }
}
