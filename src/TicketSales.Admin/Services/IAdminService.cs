using System.Collections.Generic;
using System.Threading.Tasks;
using TicketSales.Admin.Models;

namespace TicketSales.Admin.Services
{
    public interface IAdminService
    {
        Task CreateConcert(CreateConcertRequest request);

        Task<IEnumerable<Core.Domain.Concert>> RetrieveConcerts();
    }
}