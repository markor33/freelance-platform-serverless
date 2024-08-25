using JobSearch.IndexModel;

namespace JobSearch.Persistence;

public class GetDocResponse
{
    public bool Found { get; set; }
    public Job _source { get; set; }
}
