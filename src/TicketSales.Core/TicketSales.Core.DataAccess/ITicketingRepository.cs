using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess
{
    public interface ITicketingRepository
    {
        Task<User> GetUserById(int id);

        Task<Concert> GetConcertById(int id);

        Task<IEnumerable<Concert>> GetConcerts();

        Task<IEnumerable<Concert>> GetAvailableConcerts();

        Task<int> InsertConcert(Concert concert);

        Task UpdateConcert(Concert concert);

        Task InsertTicketSale(TicketSale ticket);

        Task InsertSoldTicketView(SoldTicketView soldTicketView);

        Task<IEnumerable<SoldTicketView>> GetSoldTicketsByUser(int userId);
    }
}

