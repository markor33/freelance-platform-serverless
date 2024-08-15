using Amazon;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
<<<<<<< HEAD
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
=======
>>>>>>> 35a2ed2d0888ac63a9b73bafecb99561ac716fd1
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace FreelancerCommands.Lambda.Commands;

public class SetProfilePictureCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<SetProfilePictureCommand> _validator;
    private readonly IAmazonS3 _amazonS3;
    private ILambdaContext _context;

    public SetProfilePictureCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new SetProfilePictureCommandValidator();
        _amazonS3 = new AmazonS3Client();
    }

    public SetProfilePictureCommandHandler(IFreelancerRepository freelancerRepository, IValidator<SetProfilePictureCommand> validator, IAmazonS3 amazonS3, ILambdaContext context)
    {
        _freelancerRepository = freelancerRepository;
        _validator = validator;
        _amazonS3 = amazonS3;
        _context = context;
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        if (id != sub)
        {
            context.Logger.LogError("Profile picture setup failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

<<<<<<< HEAD
        var command = JsonSerializer.Deserialize<SetProfilePictureCommand>(request.Body, JsonOptions.Options);
=======
        var command = JsonSerializer.Deserialize<SetProfilePictureCommand>(request.Body);
>>>>>>> 35a2ed2d0888ac63a9b73bafecb99561ac716fd1
        command.FreelancerId = Guid.Parse(sub);

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
<<<<<<< HEAD
            StatusCode = statusCode,
            Headers = Headers.CORS
=======
            StatusCode = statusCode
>>>>>>> 35a2ed2d0888ac63a9b73bafecb99561ac716fd1
        };
    }

    public async Task<Result> CommandHandler(SetProfilePictureCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
            {
                _context.Logger.LogError($"Freelander with {request.FreelancerId} does not exist");

                return Result.Fail("Freelancer does not exist");
            }

            var pictureUrl = await UploadPictureToS3(freelancer.Id, request.PictureBase64);
            if (pictureUrl is null)
                return Result.Fail("Setting profile picture failed");

            freelancer.SetProfilePicture(pictureUrl);

            await _freelancerRepository.SaveAsync(freelancer);

            _context.Logger.LogInformation("Profile picture setup successful");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"Exception - {ex}");

            return Result.Fail("Setting profile picture failed");
        }
    }

    private async Task<string?> UploadPictureToS3(Guid freelancerId, string base64Picture)
    {
        var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");
        var fileName = $"{freelancerId}.jpg";

        var imageBytes = Convert.FromBase64String(base64Picture);
        var stream = new MemoryStream(imageBytes);

        var putRequest = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = stream,
        };

        var res = await _amazonS3.PutObjectAsync(putRequest);

        if (!res.ToResult().IsSuccess)
            return null;

        var url = $"https://{bucketName}.s3.eu-central-1.amazonaws.com/{fileName}";
        return url;
    }
}

public class SetProfilePictureCommand
{
    public Guid FreelancerId { get; set; }
    public string PictureBase64 { get; set; }
}

public class SetProfilePictureCommandValidator : AbstractValidator<SetProfilePictureCommand>
{
    public SetProfilePictureCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.PictureBase64).NotEmpty();
    }

}