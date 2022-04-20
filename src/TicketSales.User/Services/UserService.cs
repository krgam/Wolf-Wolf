using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSales.Admin.Models;
using TicketSales.Core.DataAccess;
using TicketSales.Core.Domain;
using TicketSales.Messages.Commands;
using TicketSales.User.Models;

namespace TicketSales.User.Services
{
    public class UserService : IUserService
    {
        private readonly IPublishEndpoint publisher;
        private readonly ITicketingRepository repository;

        public UserService(IPublishEndpoint publisher, ITicketingRepository repository)
        {
            this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ServiceResponse> BuyTicket(BuyTicketRequest request)
        {
            var getUserTask = this.repository.GetUserById(request.UserId);
            var getConcertTask = this.repository.GetConcertById(request.ConcertId);

            await Task.WhenAll(getUserTask, getConcertTask);

            var user = await getUserTask;
            var concert = await getConcertTask;

            if (user == null)
            {
                return new ServiceResponse(ErrorCode.UserNotFound);
            }

            if (concert == null)
            {
                return new ServiceResponse(ErrorCode.ConcertNotFound);
            }

            if (concert.NumberOfTickets < request.NumberOfTickets)
            {
                return new ServiceResponse(ErrorCode.NotEnoughTicketsForConcert);
            }

            var command = new BuyTicket()
            {
                UserId = request.UserId,
                ConcertId = request.ConcertId,
                NumberOfTickets = request.NumberOfTickets,
            };

            await this.publisher.Publish(command);

            return new ServiceResponse();
        }

        public async Task<IEnumerable<Core.Domain.Concert>> RetrieveAvailableConcerts()
        {
            return await this.repository.GetAvailableConcerts();
        }

        public async Task<IEnumerable<SoldTicketView>> RetrieveUserTickets(int userId)
        {
            return await this.repository.GetSoldTicketsByUser(userId);
        }
    }
}
