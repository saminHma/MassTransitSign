namespace MassTransitTest.Models
{
    public record SignPending
    {
        public Guid Id { get; set; }
        public DateTime Time { get; init; }
    }
}
