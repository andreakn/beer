namespace Beer.Contracts;

public class ConveyorBeltState
{
    public static readonly ConveyorBeltState Stopped = new ConveyorBeltState("Stopped");
    public static readonly ConveyorBeltState Started = new ConveyorBeltState("Started");
    public static readonly ConveyorBeltState Crashed = new ConveyorBeltState("Crashed");
    
    private ConveyorBeltState(string value)
    {
        Value = value;
    }

    public static ConveyorBeltState Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentNullException("Its null man");

        return value.ToLowerInvariant() switch
        {
            "stopped" => Stopped,
            "started" => Started,
            _ => Crashed
        };
    }

    public string Value { get; }
}