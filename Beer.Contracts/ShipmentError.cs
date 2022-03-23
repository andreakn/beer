namespace Beer.Contracts;

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