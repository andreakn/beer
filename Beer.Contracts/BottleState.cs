namespace Beer.Contracts;

public class BottleState
{
    public static readonly BottleState Good = new BottleState("Good");
    public static readonly BottleState Broken= new BottleState("Broken");
    
    private BottleState(string value)
    {
        Value = value;
    }

    public static BottleState Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentNullException("Its null man");

        return value.ToLowerInvariant() switch
        {
            "good" => Good,
            _ => Broken
        };
    }

    public string Value { get; }
}