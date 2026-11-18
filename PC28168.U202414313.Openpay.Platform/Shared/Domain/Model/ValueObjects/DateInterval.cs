namespace PC28168.U202414313.Openpay.Platform.Shared.Domain.Model.ValueObjects;

public sealed record DateInterval
{
    private DateInterval()
    {
    }
    public DateInterval(DateOnly scheduledFrom, DateOnly scheduledTo)
    {
        if (scheduledTo <= scheduledFrom)
            throw new ArgumentException("ScheduledTo must be strictly greater than ScheduledFrom.", nameof(scheduledTo));

        ScheduledFrom = scheduledFrom;
        ScheduledTo = scheduledTo;
    }
    public DateOnly ScheduledFrom { get; }
    public DateOnly ScheduledTo { get; }
    public bool HasElapsed(DateOnly referenceDate) => referenceDate > ScheduledTo;
}