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

namespace EducationCommands.Lambda.Commands;

public class AddEducationCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<AddEducationCommand> _validator;
    private ILambdaContext _context;

    public AddEducationCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new AddEducationCommandValidator();
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

        var command = JsonSerializer.Deserialize<AddEducationCommand>(request.Body);
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
            Body = JsonSerializer.Serialize(EducationViewModel.FromEducation(result.Value))
        };
    }

    public async Task<Result<Education>> CommandHandler(AddEducationCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);

            var attended = new DateRange(request.Start, request.End);
            var education = new Education(request.SchoolName, request.Degree, attended);
            freelancer.AddEducation(education);

            await _freelancerRepository.SaveAsync(freelancer);

            return Result.Ok(education);
        }
        catch (Exception ex)
        {
            _context.Logger.LogError(ex.ToString());
            return Result.Fail("Education creation failed");
        }
    }

}

public class AddEducationCommand
{
    public Guid FreelancerId { get; set; }
    public string SchoolName { get; set; }
    public string Degree { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class AddEducationCommandValidator : AbstractValidator<AddEducationCommand>
{
    public AddEducationCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.SchoolName).NotEmpty();

        RuleFor(x => x.Degree).NotEmpty();

        RuleFor(x => x.Start).NotEmpty();

        RuleFor(x => x.End).NotEmpty();
    }
}
