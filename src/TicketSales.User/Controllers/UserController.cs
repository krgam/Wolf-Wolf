using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Threading.Tasks;
using TicketSales.User.Mappers;
using TicketSales.User.Models;
using TicketSales.User.Services;

namespace TicketSales.User.Controllers
{
    [Route("user-api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IModelMapper mapper;

        public UserController(IUserService service, IModelMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("available-concerts")]
        [ProducesResponseType(typeof(GetAvailableConcertsResponse), StatusCodes.Status200OK)]
        [OpenApiOperation("RetrieveAvailableConcerts")]
        public async Task<IActionResult> RetrieveAvailableConcerts()
        {
            var svcResult = await service.RetrieveAvailableConcerts();

            var response = mapper.Map(svcResult);

            return Ok(response);
        }

        [HttpPost]
        [Route("tickets-buying")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [OpenApiOperation("BuyTicket")]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicketRequest request)
        {
            var svcResponse = await service.BuyTicket(request);

            if (!svcResponse.IsSuccess)
            {
                return BadRequest(new ValidationErrorResponse() { ErrorCode = svcResponse.ErrorCode.Value });
            }

            return Accepted();
        }

        [HttpGet]
        [Route("users/{userId}/tickets")]
        [ProducesResponseType(typeof(GetUserTicketsResponse), StatusCodes.Status200OK)]
        [OpenApiOperation("RetrieveUserTickets")]
        public async Task<IActionResult> RetrieveUserTickets([FromRoute] int userId)
        {
            var svcResult = await service.RetrieveUserTickets(userId);

            var response = mapper.Map(svcResult);

            return Ok(response);
        }
    }
}
