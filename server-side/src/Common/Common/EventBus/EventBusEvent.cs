namespace Common.Layer.EventBus;

public class EventBusEvent<TDetail>
{
    public string Version { get; set; }
    public string Id { get; set; }
    public string DetailType { get; set; }
    public string Source { get; set; }
    public string Account { get; set; }
    public string Time { get; set; }
    public string Region { get; set; }
    public List<string> Resources { get; set; }
    public TDetail Detail { get; set; }
}
