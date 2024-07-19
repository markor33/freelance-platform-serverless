namespace FreelancerProfile.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<DependecyFixture>
{
    protected DependecyFixture fixture;

    public BaseIntegrationTest(DependecyFixture fixture)
    {
        this.fixture = fixture;
    }
}
