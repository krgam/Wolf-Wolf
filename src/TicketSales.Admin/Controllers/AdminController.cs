using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Threading.Tasks;
using TicketSales.Admin.Mappers;
using TicketSales.Admin.Models;
using TicketSales.Admin.Services;

namespace TicketSales.Admin.Controllers
{
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService service;
        private readonly IModelMapper mapper;

        public AdminController(IAdminService service, IModelMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route("concerts")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [OpenApiOperation("CreateConcert")]
        public async Task<IActionResult> CreateConcert([FromBody] CreateConcertRequest request)
        {
            await this.service.CreateConcert(request);

            return Accepted();
        }

        [HttpGet]
        [Route("concerts")]
        [ProducesResponseType(typeof(GetConcertsResponse), StatusCodes.Status200OK)]
        [OpenApiOperation("GetConcerts")]
        public async Task<IActionResult> GetConcerts()
        {
            var svcResult = await this.service.RetrieveConcerts();

            var response = this.mapper.Map(svcResult);

            return Ok(response);
        }
    }
}
