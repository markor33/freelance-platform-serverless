using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Enums;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using ReadModelStore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace FreelancerCommands.Lambda.Commands;

public class SetupProfileCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IProfessionRepository _professionRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IValidator<SetupProfileCommand> _validator;
    private ILambdaContext _context;

    public SetupProfileCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _professionRepository = new ProfessionRepository();
        _languageRepository = new LanguageRepository();
        _validator = new SetupProfileCommandValidator();
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


        var command = JsonSerializer.Deserialize<SetupProfileCommand>(request.Body);
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

    public async Task<Result> CommandHandler(SetupProfileCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);

            _context.Logger.LogInformation(request.FreelancerId.ToString());
            _context.Logger.LogInformation(freelancer.Id.ToString());

            var profession = await _professionRepository.GetByIdAsync(request.ProfessionId);

            var language = await _languageRepository.GetByIdAsync(request.LanguageId);

            var languageKnowledge = new LanguageKnowledge(language, request.LanguageProficiencyLevel);

            freelancer.SetupProfile(request.FirstName, request.LastName, request.Contact, 
                request.IsProfilePublic, request.ProfileSummary, request.HourlyRate,
                    request.Availability, request.ExperienceLevel, profession, languageKnowledge);

            await _freelancerRepository.SaveAsync(freelancer);

            return Result.Ok();
        }
        catch
        {
            return Result.Fail("Freelancer profile setup failed");
        }
    }
}

public class SetupProfileCommand
{
    public Guid FreelancerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Contact Contact { get; set; }
    public bool IsProfilePublic { get; set; }
    public ProfileSummary ProfileSummary { get; set; }
    public HourlyRate HourlyRate { get; set; }
    public Availability Availability { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public Guid ProfessionId { get; set; }
    public int LanguageId { get; set; }
    public LanguageProficiencyLevel LanguageProficiencyLevel { get; set; }
}

public class SetupProfileCommandValidator : AbstractValidator<SetupProfileCommand>
{
    public SetupProfileCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();

        RuleFor(x => x.LastName).NotEmpty();

        RuleFor(x => x.Contact.PhoneNumber).NotEmpty();

        RuleFor(x => x.Contact.Address.Country).NotEmpty();
        RuleFor(x => x.Contact.Address.City).NotEmpty();
        RuleFor(x => x.Contact.Address.Street).NotEmpty();
        RuleFor(x => x.Contact.Address.Number).NotEmpty();
        RuleFor(x => x.Contact.Address.ZipCode).NotEmpty();

        RuleFor(x => x.IsProfilePublic).NotEmpty();

        RuleFor(x => x.ProfileSummary.Title).NotEmpty().MaximumLength(50);

        RuleFor(x => x.ProfileSummary.Description).NotEmpty();

        RuleFor(x => x.HourlyRate.Amount).NotEmpty().GreaterThan(0);

        RuleFor(x => x.HourlyRate.Currency).NotEmpty().MaximumLength(5);

        RuleFor(x => x.Availability).IsInEnum();

        RuleFor(x => x.ExperienceLevel).IsInEnum();

        RuleFor(x => x.ProfessionId).NotEmpty();

        RuleFor(x => x.LanguageId).NotEmpty();

        RuleFor(x => x.LanguageProficiencyLevel).IsInEnum();
    }
}
