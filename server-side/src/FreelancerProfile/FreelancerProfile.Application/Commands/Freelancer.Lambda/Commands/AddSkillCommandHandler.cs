using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.Repositories;
using ReadModelStore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace FreelancerCommands.Lambda.Commands;

public class AddSkillCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IProfessionRepository _professionRepository;
    private ILambdaContext _context;

    public AddSkillCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _professionRepository = new ProfessionRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        if (id != sub)
        {
            context.Logger.LogError("Skills assigning failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }


        var command = JsonSerializer.Deserialize<AddSkillCommand>(request.Body);
        command.FreelancerId = Guid.Parse(sub);

        var result = await CommandHandler(command);
        var statusCode = result.IsSuccess ? 201 : 400;

        return new APIGatewayProxyResponse()
        {
            StatusCode = statusCode
        };
    }

    public async Task<Result> CommandHandler(AddSkillCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
            {
                _context.Logger.LogError($"Freelander with {request.FreelancerId} does not exist");

                return Result.Fail("Education update failed");
            }

            if (freelancer.ProfessionId is null)
            {
                _context.Logger.LogError($"Freelancar({request.FreelancerId}) does not have assigned profession");

                return Result.Fail("Freelancer does not have assigned profession");
            }

            var profession = await _professionRepository.GetByIdAsync((Guid)freelancer.ProfessionId);

            if (!profession.TryGetSkills(request.Skills, out List<Skill> skills))
            {
                _context.Logger.LogError("Some skills are not valid");

                return Result.Fail("Some skills are not valid");
            }

            freelancer.UpdateSkills(skills);
            await _freelancerRepository.SaveAsync(freelancer);

            _context.Logger.LogInformation("Skills assigning failed successful");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            {
                _context.Logger.LogError($"Exception: {ex}");

                return Result.Fail("Skills assigning failed");
            }
        }
    }
}


public class AddSkillCommand
{
    public Guid FreelancerId { get; set; }
    public List<Guid> Skills { get; private set; }

    public AddSkillCommand(List<Guid> skills)
    {
        Skills = skills;
    }

}