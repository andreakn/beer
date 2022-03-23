using System.Runtime.CompilerServices;

namespace Beer.Contracts;

public class BottleDto
{
    public string Id { get; set; }
    public string TeamId { get; set; }
    public string? BeerType { get; set; }
    public string? Label { get; set; }
    public int Content { get; set; }
    public int MaxContent { get; set; }
    public string State { get; set; } //Good, Broken
    public DateTimeOffset? CorkedTime { get; set; }
    public DateTimeOffset? ConsumedBefore { get; set; }
    public int FermentationSeconds { get; set; }
    public DateTimeOffset? ShippedDate { get; set; }
}

public class CaseDto
{
    public IEnumerable<string> BottleIds { get; set; }
}

public class NotBottleDto
{
    public string? BeerType { get; set; }
    public string? Label { get; set; }
    public string? Content { get; set; }
}

public class ProblemDetailsDto
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public string Instance { get; set; }
}

public class ShipOperationResultDto
{
    public string? Message { get; set; }
    public bool Success { get; set; }
    public int Score { get; set; }
    public DateTimeOffset? ShipmentDate { get; set; }
    public IEnumerable<string> Errors { get; set; }
}

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

public class ShipmentError
{
    public static readonly ShipmentError Undefined = new ShipmentError("Undefined");
    public static readonly ShipmentError BottleIsNotFull = new ShipmentError("BottleIsNotFull");
    public static readonly ShipmentError BottleIsBroken = new ShipmentError("BottleIsBroken");
    public static readonly ShipmentError BottleIsNotCorked = new ShipmentError("BottleIsNotCorked");
    public static readonly ShipmentError FermentationIsNotDone = new ShipmentError("FermentationIsNotDone");
    public static readonly ShipmentError ConsumptionDateIsPassed = new ShipmentError("ConsumptionDateIsPassed");
    public static readonly ShipmentError RecycledFullBottle  = new ShipmentError("RecycledFullBottle");

    private ShipmentError(string value)
    {
        Value = value;
    }
    
    public static ShipmentError Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentNullException("Its null man");

        return value.ToLowerInvariant() switch
        {
            "bottleisnotfull" => BottleIsNotFull,
            "bottleisbroken" => BottleIsBroken,
            "bottleisnotcorked" => BottleIsNotCorked,
            "fermentationisnotdone" => FermentationIsNotDone,
            "comsumptiondateispassed" => ConsumptionDateIsPassed,
            "recycledfullbottle" => RecycledFullBottle,
            _ => Undefined
        };
    }

    public string Value { get; }
}

