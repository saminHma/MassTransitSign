using ValidationForMassTransit;

namespace MassTransitTest.Validation;

public class ValidationFailurePipe<TMessage> : ValidationFailurePipeBase<TMessage>
    where TMessage : class
{
    public override async Task Send(ValidationFailureContext<TMessage> context)
    {
        var validationProblems = context.ValidationProblems;
        await context.InnerContext.RespondAsync(new ValidationError()
        {
            ValidationFailures = validationProblems
        });
    }
}