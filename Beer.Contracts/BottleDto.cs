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

    public bool IsFull()
    {
        return MaxContent == Content;
    }

    public bool IsBroken()
    {
        var state = BottleState.Parse(State);
        return state == BottleState.Broken;
    }

    public bool IsReadyToShip()
    {
        if (CorkedTime == null)
        {
            return false;
        }

        var doneTime = CorkedTime + TimeSpan.FromSeconds(FermentationSeconds);
        return doneTime >= DateTimeOffset.UtcNow;
    }

    public bool IsExpired()
    {
        if (ConsumedBefore == null)
        {
            return false;
        }

        return DateTimeOffset.UtcNow > ConsumedBefore;
    }
}