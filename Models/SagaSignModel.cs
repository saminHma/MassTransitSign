using MassTransit;

namespace MassTransitTest.Models
{
    public class SagaSignModel : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get ; set; }
        public string CurrentState { get; set; }
        public int Version { get; set; }
        public DateTime SignedDateTime { get; set; }
        public DateTime RejectedDateTime { get; set; }
    }
}
