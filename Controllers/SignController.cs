using MassTransitTest.Validation;
using Microsoft.AspNetCore.Mvc;
using MassTransitTest.Models;
using MassTransit;

namespace MassTransitTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IRequestClient<SignReady> _signReadyClient;

        public SignController(IBus bus, IRequestClient<SignReady> signReadyClient)
        {
            _bus = bus;
            _signReadyClient = signReadyClient;
        }

        [HttpPost("/ReadyForSign")]
        public async Task<IActionResult> ReadyForSign(string nationalId)
        {
            var signResponse = await _signReadyClient.GetResponse<ValidationError, SagaSignModel>(new SignReady()
            {
                Id = Guid.NewGuid(),
                NationalId = nationalId,
                Time = DateTime.Now,
            });

            if (signResponse.Is(out Response<ValidationError> validationError))
            {
                return BadRequest(validationError.Message);
            }

            if (signResponse.Is(out Response<SagaSignModel> signModel))
            {
                return Ok(signModel);
            }


            return Ok();
        }


        [HttpGet("/CompleteSign/{id}")]
        public async Task<IActionResult> SignCompleted(Guid id)
        {
            await _bus.Publish(
                new SignCompleted()
                {
                    Id = id,
                    Time = DateTime.Now,
                });
            return Ok();
        }
    }
}