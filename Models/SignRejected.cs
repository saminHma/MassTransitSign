namespace MassTransitTest.Models
{
    public record SignRejected
    {
        public Guid Id { get; set; }
        public DateTime Time { get; init; }
    }
}
