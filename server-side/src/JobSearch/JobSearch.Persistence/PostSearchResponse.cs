using JobSearch.IndexModel;

namespace JobSearch.Persistence
{
    public class PostSearchResponse
    {
        public HitsInfo hits { get; set; }
    }

    public class ShardsInfo
    {
        public int Total { get; set; }
        public int Successful { get; set; }
        public int Skipped { get; set; }
        public int Failed { get; set; }
    }

    public class HitsInfo
    {
        public List<Hit> hits { get; set; }
    }

    public class TotalInfo
    {
        public int Value { get; set; }
        public string Relation { get; set; }
    }

    public class Hit
    {
        public string _index { get; set; }
        public string _id { get; set; }
        public float _score { get; set; }
        public Job _source { get; set; }
    }
}
