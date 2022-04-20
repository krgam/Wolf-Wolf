using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSales.Admin.Models;
using TicketSales.Core.DataAccess;
using TicketSales.Messages.Commands;

namespace TicketSales.Admin.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBus publisher;
        private readonly ITicketingRepository repository;

        public AdminService(IBus publisher, ITicketingRepository repository)
        {
            this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task CreateConcert(CreateConcertRequest request)
        {
            var command = new CreateConcert()
            {
                ConcertName = request.ConcertName,
                NumberOfTickets = request.NumberOfTickets,
            };

            await this.publisher.Send(command);
        }

        public async Task<IEnumerable<Core.Domain.Concert>> RetrieveConcerts()
        {
            return await this.repository.GetConcerts();
        }
    }
}
