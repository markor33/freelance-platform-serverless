using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;
using ReadModel;

namespace ReadModelStore.Seed;

public class LanguageSeed
{
    public static async Task SeedAsync()
    {
        var client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1); ;
        var context = new DynamoDBContext(client);

        var tasks = new List<Task>()
        {
            context.SaveAsync<LanguageViewModel>(new LanguageViewModel
            {
                Id = 1,
                Name = "English",
                ShortName = "en",
            }),
            context.SaveAsync<LanguageViewModel>(new LanguageViewModel
            {
                Id = 2,
                Name = "Serbian",
                ShortName = "srb",
            }),
            context.SaveAsync<LanguageViewModel>(new LanguageViewModel
            {
                Id = 3,
                Name = "German",
                ShortName = "de",
            }),
            context.SaveAsync<LanguageViewModel>(new LanguageViewModel
            {
                Id = 4,
                Name = "French",
                ShortName = "fr",
            })
        };

        await Task.WhenAll(tasks);
    }
}
