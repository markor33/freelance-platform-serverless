using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using WriteModel;

namespace CertificationCommands.Lambda.Commands;

public class DeleteCertificationCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<DeleteCertificationCommand> _validator;
    private ILambdaContext _context;

    public DeleteCertificationCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new DeleteCertificationCommandValidator();
    }

    public DeleteCertificationCommandHandler(IFreelancerRepository freelancerRepository, IValidator<DeleteCertificationCommand> validator, ILambdaContext context)
    {
        _freelancerRepository = freelancerRepository;
        _validator = validator;
        _context = context;
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        var certificationId = request.PathParameters["certificationId"];
        if (id != sub || certificationId is null)
        {
            context.Logger.LogError("Certification delete failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var command = new DeleteCertificationCommand(Guid.Parse(id), Guid.Parse(certificationId));

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

    public async Task<Result> CommandHandler(DeleteCertificationCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
            {
                _context.Logger.LogError($"Freelander with {request.FreelancerId} does not exist");

                return Result.Fail("Certification delete failed");
            }

            freelancer.DeleteCertification(request.CertificationId);
            await _freelancerRepository.SaveAsync(freelancer);

            _context.Logger.LogInformation($"Certification successfully deleted - Id:{request.CertificationId}");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"Certification delete failed with exception - {ex}");

            return Result.Fail("Delete employment action failed");
        }
    }
}

public class DeleteCertificationCommand
{
    public Guid FreelancerId { get; set; }
    public Guid CertificationId { get; set; }

    public DeleteCertificationCommand(Guid freelancerId, Guid certificationId)
    {
        FreelancerId = freelancerId;
        CertificationId = certificationId;
    }
}

public class DeleteCertificationCommandValidator : AbstractValidator<DeleteCertificationCommand>
{
    public DeleteCertificationCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.CertificationId).NotEmpty();
    }
}
