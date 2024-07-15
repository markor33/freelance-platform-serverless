﻿using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace EmploymentCommands.Lambda.Commands;

public class AddEmploymentCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<AddEmploymentCommand> _validator;
    private ILambdaContext _context;

    public AddEmploymentCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new AddEmploymentCommandValidator();
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

        var command = JsonSerializer.Deserialize<AddEmploymentCommand>(request.Body);
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

    public async Task<Result> CommandHandler(AddEmploymentCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);

            var period = new DateRange(request.Start, request.End);
            var employment = new Employment(request.Company, request.Title, period, request.Description);
            freelancer.AddEmployment(employment);

            await _freelancerRepository.SaveAsync(freelancer);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError(ex.ToString());
            return Result.Fail("Employment creation failed");
        }
    }

}

public class AddEmploymentCommand
{
    public Guid FreelancerId { get; set; }
    public string Company { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Description { get; set; }

}

public class AddEmploymentCommandValidator : AbstractValidator<AddEmploymentCommand>
{
    public AddEmploymentCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.Title).NotEmpty();

        RuleFor(x => x.Company).NotEmpty();

        RuleFor(x => x.Start).NotEmpty();

        RuleFor(x => x.End).NotEmpty();
    }
}
