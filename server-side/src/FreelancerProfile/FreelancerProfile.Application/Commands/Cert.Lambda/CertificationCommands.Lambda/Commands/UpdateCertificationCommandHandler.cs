﻿using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace CertificationCommands.Lambda.Commands;

public class UpdateCertificationCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<UpdateCertificationCommand> _validator;
    private ILambdaContext _context;

    public UpdateCertificationCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new UpdateCertificationCommandValidator();
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

        var command = JsonSerializer.Deserialize<UpdateCertificationCommand>(request.Body);
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

    public async Task<Result> CommandHandler(UpdateCertificationCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);

            freelancer.UpdateCertification(request.CertificationId, request.Name, request.Provider,
                new DateRange(request.Start, request.End), request.Description);

            await _freelancerRepository.SaveAsync(freelancer);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError(ex.ToString());
            return Result.Fail("Edit employment action failed");
        }
    }
}

public class UpdateCertificationCommand
{
    public Guid FreelancerId { get; set; }
    public Guid CertificationId { get; set; }
    public string Name { get; set; }
    public string Provider { get; set; }
    public string? Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class UpdateCertificationCommandValidator : AbstractValidator<UpdateCertificationCommand>
{
    public UpdateCertificationCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.CertificationId).NotEmpty();

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Provider).NotEmpty();

        RuleFor(x => x.Start).NotEmpty();

        RuleFor(x => x.End).NotEmpty();
    }
}
