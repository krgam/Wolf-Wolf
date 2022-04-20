using System;
using System.Collections.Generic;
using System.Linq;
using TicketSales.Admin.Models;

namespace TicketSales.Admin.Mappers
{
    public class ModelMapper : IModelMapper
    {
        public GetConcertsResponse Map(IEnumerable<Core.Domain.Concert> concerts)
        {
            if (concerts is null)
            {
                throw new ArgumentNullException(nameof(concerts));
            }

            return new GetConcertsResponse() { Items = concerts.Select(this.Map).ToList() };
        }

        private Concert Map(Core.Domain.Concert item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new Concert()
            {
                Id = item.Id,
                Name = item.Name,
                SoldTickets = item.SoldTickets,
            };
        }
    }
}
