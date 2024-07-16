using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using ReadModel;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace CertificationCommands.Lambda.Commands;

public class AddCertificationCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<AddCertificationCommand> _validator;
    private ILambdaContext _context;

    public AddCertificationCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new AddCertificationCommandValidator();
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

        var command = JsonSerializer.Deserialize<AddCertificationCommand>(request.Body);
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
            StatusCode = statusCode,
            Body = JsonSerializer.Serialize(CertificationViewModel.FromCertification(result.Value))
        };
    }

    public async Task<Result<Certification>> CommandHandler(AddCertificationCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);

            var attended = new DateRange(request.Start, request.End);
            var certification = new Certification(request.Name, request.Provider, attended, request.Description);
            freelancer.AddCertification(certification);

            await _freelancerRepository.SaveAsync(freelancer);

            return Result.Ok(certification);
        }
        catch (Exception ex)
        {
            _context.Logger.LogError(ex.ToString());
            return Result.Fail("Certification creation failed");
        }
    }
}

public class AddCertificationCommand
{
    public Guid FreelancerId { get; set; }
    public string Name { get; set; }
    public string Provider { get; set; }
    public string? Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class AddCertificationCommandValidator : AbstractValidator<AddCertificationCommand>
{
    public AddCertificationCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Provider).NotEmpty();

        RuleFor(x => x.Start).NotEmpty();

        RuleFor(x => x.End).NotEmpty();
    }
}

