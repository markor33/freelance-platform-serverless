using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using ReadModel;

namespace ReadModelStore.Seed;

public class ProfessionSeed
{
    public static async Task SeedAsync()
    {
        var client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1); ;
        var context = new DynamoDBContext(client);

        var tasks = new List<Task>()
        {
            context.SaveAsync<ProfessionViewModel>(new ProfessionViewModel()
            {
                Id = Guid.Parse("d6861f65-0950-4c7f-b5b1-de644f923fbb"),
                Name = "Software engineer",
                Description = "Software engineer",
                Skills =
                [
                    new()
                    {
                        Id = Guid.Parse("93098c08-85ff-4c31-994b-5dec79c17d79"),
                        Name = "C#",
                        Description = "Programming language"
                    },
                    new()
                    {
                        Id = Guid.Parse("ea1627e1-2d59-427d-b5b4-13ab7e944c7f"),
                        Name = "ASP.NET CORE",
                        Description = "Web framework"
                    }
                ]
            }),
            context.SaveAsync<ProfessionViewModel>(new ProfessionViewModel()
            {
                Id = Guid.Parse("0c485898-d9f4-45c5-99bc-c2c8dd3e69f0"),
                Name = "Graphic designer",
                Description = "Graphic designer",
                Skills =
                [
                    new()
                    {
                        Id = Guid.Parse("5d741f6a-f024-4dca-8b1f-afccec1f72ea"),
                        Name = "Adobe Illustrator",
                        Description = "Design software"
                    },
                    new()
                    {
                        Id = Guid.Parse("e190ca8a-5252-4b00-8128-f21d9918efaf"),
                        Name = "CorelDRAW Graphics Suite",
                        Description = "Design software"
                    }
                ]
            })
        };

        await Task.WhenAll(tasks);
    }
}