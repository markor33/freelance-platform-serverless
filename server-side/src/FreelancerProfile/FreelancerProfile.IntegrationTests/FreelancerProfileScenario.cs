using FluentValidation;
using FreelancerCommands.Lambda.Commands;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Enums;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using Moq;
using Shouldly;

namespace FreelancerProfile.IntegrationTests;

[Collection("Freelancer Profile")]
public class FreelancerProfileScenario : BaseIntegrationTest
{

    public FreelancerProfileScenario(DependecyFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Setup_Profile_ReturnsOk()
    {
        var validatorMock = new Mock<IValidator<SetupProfileCommand>>();
        var setupProfileCommandHandler = new SetupProfileCommandHandler(
            fixture.FreelancerRepository,
            fixture.ProfessionRepository,
            fixture.LanguageRepository,
            validatorMock.Object,
            fixture.LambdaContext);

        var result = await setupProfileCommandHandler.CommandHandler(GetTestSetupProfileCommand());

        result.IsSuccess.ShouldBeTrue();
    }

    private static SetupProfileCommand GetTestSetupProfileCommand()
        => new()
        {
            FreelancerId = DependecyFixture.FreelancerId,
            FirstName = "John",
            LastName = "Johnes",
            Contact = new Contact(new Address("country", "city", "street", "number", "zipcode"), "01231231232"),
            IsProfilePublic = true,
            ProfileSummary = new ProfileSummary("Title", "Desc"),
            HourlyRate = new HourlyRate(50, "USD"),
            Availability = Availability.FULL_TIME,
            ExperienceLevel = ExperienceLevel.MEDIOR,
            ProfessionId = DependecyFixture.ProfessionId,
            LanguageId = DependecyFixture.LanguageId,
            LanguageProficiencyLevel = LanguageProficiencyLevel.NATIVE
        };

}
