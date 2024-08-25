using FeedbackManagement.Persistence;
using JobSearch.IndexModel;
using JobSearch.Persistence;

namespace JobSearch.Tests
{
    public class BuildSearchQuery
    {
        [Fact]
        public async void BuildSearchQueryTest()
        {
            var repo = new FeedbackRepository();
            var res = await repo.GetAverageRatingByClients([Guid.Parse("23041852-3051-70fb-1e7f-e326080ef3f9")]);
        }
    }
}