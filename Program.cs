using MassTransit;
using FluentValidation;
using System.Reflection;
using MassTransitTest.Models;
using MassTransitTest.Services;
using ValidationForMassTransit;
using MassTransitTest.Validation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IValidationFailurePipe<>), typeof(ValidationFailurePipe<>));

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.UseConsumeFilter(typeof(ValidationForMassTransit.FluentValidationFilter<>), context);
        cfg.ConfigureEndpoints(context);
    });
    x.AddSagaStateMachine<SignStateMachine, SagaSignModel>().InMemoryRepository();
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();