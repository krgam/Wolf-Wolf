using System.Collections.Generic;
using TicketSales.Admin.Models;

namespace TicketSales.Admin.Mappers
{
    public interface IModelMapper
    {
        GetConcertsResponse Map(IEnumerable<Core.Domain.Concert> concerts);
    }
}