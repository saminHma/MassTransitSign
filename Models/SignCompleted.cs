namespace MassTransitTest.Models
{
    public record SignCompleted 
    {
        public Guid Id { get; set; }
        public DateTime Time { get; init; }
    }
}
