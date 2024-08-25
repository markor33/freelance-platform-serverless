using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using WriteModel;

namespace EducationCommands.Lambda.Commands;

public class DeleteEducationCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<DeleteEducationCommand> _validator;
    private ILambdaContext _context;

    public DeleteEducationCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new DeleteEducationCommandValidator();
    }

    public DeleteEducationCommandHandler(IFreelancerRepository freelancerRepository, IValidator<DeleteEducationCommand> validator, ILambdaContext context)
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
        var educationId = request.PathParameters["educationId"];
        if (id != sub || educationId is null)
        {
            context.Logger.LogError("Education delete failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var command = new DeleteEducationCommand(Guid.Parse(id), Guid.Parse(educationId));

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

    public async Task<Result> CommandHandler(DeleteEducationCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
            {
                _context.Logger.LogError($"Freelander with {request.FreelancerId} does not exist");

                return Result.Fail("Education delete failed");
            }

            freelancer.DeleteEducation(request.EducationId);
            await _freelancerRepository.SaveAsync(freelancer);

            _context.Logger.LogInformation($"Education successfully deleted - Id:{request.EducationId}");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"Education delete failed with exception - {ex}");

            return Result.Fail("Delete education action failed");
        }
    }

}

public class DeleteEducationCommand
{
    public Guid FreelancerId { get; set; }
    public Guid EducationId { get; set; }

    public DeleteEducationCommand(Guid freelancerId, Guid educationId)
    {
        FreelancerId = freelancerId;
        EducationId = educationId;
    }
}

public class DeleteEducationCommandValidator : AbstractValidator<DeleteEducationCommand>
{
    public DeleteEducationCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.EducationId).NotEmpty();
    }
}
