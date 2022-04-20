using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSales.Core.DataAccess.DbContexts;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess
{
    public class TicketingRepository : ITicketingRepository
    {
        private readonly TicketingDbContext dbContext;

        public TicketingRepository(TicketingDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> GetUserById(int id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<Concert> GetConcertById(int id)
        {
            return await dbContext.Concerts.FindAsync(id);
        }

        public async Task<IEnumerable<Concert>> GetConcerts()
        {
            return await this.dbContext.Concerts.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Concert>> GetAvailableConcerts()
        {
            return await this.dbContext.Concerts.AsNoTracking().Where(it => it.NumberOfTickets > 0).ToListAsync();
        }

        public async Task<int> InsertConcert(Concert concert)
        {
            this.dbContext.Concerts.Add(concert);

            await this.dbContext.SaveChangesAsync();

            return concert.Id;
        }

        public async Task UpdateConcert(Concert concert)
        {
            this.dbContext.Attach(concert);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task InsertTicketSale(TicketSale ticket)
        {
            this.dbContext.SoldTickets.Add(ticket);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task InsertSoldTicketView(SoldTicketView soldTicketView)
        {
            this.dbContext.SoldTicketsView.Add(soldTicketView);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<SoldTicketView>> GetSoldTicketsByUser(int userId)
        {
            return await this.dbContext.SoldTicketsView.AsNoTracking().Where(it => it.UserId > userId).ToListAsync();
        }
    }
}

