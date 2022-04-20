using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSales.User.Models;

namespace TicketSales.User.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Core.Domain.Concert>> RetrieveAvailableConcerts();

        Task<ServiceResponse> BuyTicket(BuyTicketRequest request);

        Task<IEnumerable<Core.Domain.SoldTicketView>> RetrieveUserTickets(int userId);
    }
}