using MassTransit;
using MassTransitTest.Models;

namespace MassTransitTest.Services
{
    public class SignStateMachine : MassTransitStateMachine<SagaSignModel>
    {
        public State Ready { get; private set; }
        public State Pending { get; private set; }
        public State Completed { get; private set; }
        public State Rejected { get; private set; }
        public Event<SignReady> SignReady { get; private set; }
        public Event<SignPending> SignPending { get; private set; }
        public Event<SignCompleted> SignCompleted { get; private set; }
        public Event<SignRejected> SignRejected { get; private set; }

        public SignStateMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => SignReady, x => x.CorrelateById(context => context.Message.Id));
            Event(() => SignPending, x => x.CorrelateById(context => context.Message.Id));
            Event(() => SignCompleted, x => x.CorrelateById(context => context.Message.Id));
            Event(() => SignRejected, x => x.CorrelateById(context => context.Message.Id));

            Initially(
                When(SignReady)
                    .Then(x => x.Saga.SignedDateTime = x.Message.Time)
                    .RespondAsync(x =>
                    {
                        Test();
                        return x.Init<SagaSignModel>(x.Saga);
                    })
                    .TransitionTo(Ready)
                    .Then(context => Console.WriteLine("******State******" + context.Saga.CurrentState.ToString())));

            During(Ready,
                When(SignCompleted)
                    .Then(x =>
                    {
                        Test();
                        x.Saga.SignedDateTime = x.Message.Time;
                    })
                    .TransitionTo(Completed)
                    .Then(context => Console.WriteLine("******state******" + context.Saga.CurrentState.ToString())));

            During(Pending,
                When(SignRejected)
                    .Then(x => x.Saga.RejectedDateTime = x.Message.Time)
                    .TransitionTo(Rejected)
                    .Then(context => Console.WriteLine("******State******" + context.Saga.CurrentState.ToString())));
        }

        private static void Test()
        {
            Console.WriteLine("test");
        }
    }
}