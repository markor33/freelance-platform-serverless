using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using JobManagement.API.GrpcServices;
using JobManagement.API.Security;
using JobManagement.Application;
using JobManagement.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var awsOptions = builder.Configuration.GetSection("AWS:Cognito");
var userPoolId = awsOptions["UserPoolId"];
var clientId = awsOptions["ClientId"];
var region = awsOptions["Region"];

var cognitoAuthority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = cognitoAuthority;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = cognitoAuthority,
        ValidateAudience = false,
        RoleClaimType = "cognito:groups"
    };
});
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient(typeof(IIdentityService), typeof(IdentityService));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonDynamoDB>();

builder.Services.AddSingleton<IDynamoDBContext>(provider =>
{
    var dynamoDbClient = provider.GetRequiredService<IAmazonDynamoDB>();
    return new DynamoDBContext(dynamoDbClient);
});

//builder.Services.AddGrpc(options =>
//{
//    options.EnableDetailedErrors = true;
//});

//builder.WebHost.UseKestrel(options => {
//    options.Listen(IPAddress.Any, 80, listenOptions => {
//        listenOptions.Protocols = HttpProtocols.Http1;
//    });

//    options.Listen(IPAddress.Any, 5000, listenOptions => {
//        listenOptions.Protocols = HttpProtocols.Http2;
//    });
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapGrpcService<ProposalGrpcService>();
//app.MapGrpcService<JobGrpcService>();
//app.MapGrpcService<ContractGrpcService>();

app.Run();

public partial class Program { }
