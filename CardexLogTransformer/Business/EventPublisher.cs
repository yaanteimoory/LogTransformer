namespace CardexLogTransformer.Business;

public class PublisherEventArgs
{
    public DateTime TriggerTime { get; private set; }
    public Thread Thread { get; private set; }
    public string Data { get; init; }

    public PublisherEventArgs(string data = "")
    {
        TriggerTime = DateTime.Now;
        Thread = Thread.CurrentThread;
        Data = data;
    }
        
}
public delegate void OnEventPublished(object sender, PublisherEventArgs e);
public class EventPublisher<T> where T : EventPublisher<T>
{
    
    

    public static event OnEventPublished Before = (_, _) => { };
    public static event OnEventPublished After = (_, _) => {};

    protected static void TriggerBefore(object sender, PublisherEventArgs e)
    {
        
        Before.Invoke(sender, e);
    }
    
    
    protected static void TriggerAfter(object sender, PublisherEventArgs e)
    {
        After.Invoke(sender, e);
    }
}