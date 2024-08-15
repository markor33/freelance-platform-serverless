using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Entities
{
    public class Question : Entity<Guid>
    {
        public string Text { get; set; }

        public Question()
        {
            Id = Guid.NewGuid();
        }

        public Question(string text)
        {
            Id = Guid.NewGuid();
            Text = text;
        }

        public Question(Guid id, string text)
        {
            Id = id;
            Text = text;
        }

        public void SetText(string text) => Text = text;

    }
}
