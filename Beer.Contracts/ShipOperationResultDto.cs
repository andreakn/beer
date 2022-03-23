namespace Beer.Contracts;

public class ShipOperationResultDto
{
    public string? Message { get; set; }
    public bool Success { get; set; }
    public int Score { get; set; }
    public DateTimeOffset? ShipmentDate { get; set; }
    public IEnumerable<string> Errors { get; set; }
}