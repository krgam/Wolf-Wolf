using AutoFixture;
using Moq;
using NUnit.Framework;
using FluentAssertions.Execution;
using TicketSales.Core.DataAccess;
using TicketSales.Core.Handlers.CommandHandlers;
using TicketSales.Core.Handlers.Builders;
using MassTransit;
using TicketSales.Messages.Commands;
using TicketSales.Core.Domain;
using System.Threading.Tasks;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Handlers.Tests.CommandHandlers
{
    [TestFixture]
    public class BuyTicketHandlerTests
    {
        private Fixture fixture = new Fixture();

        [SetUp]
        public void BaseSetUp()
        {
            this.fixture = new Fixture();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Consume_Valid_CallsRepositoryMethodsAndPublishEvent(bool notEnoughTicketsForConcert)
        {
            // Arrange
            var repositoryMock = new Mock<ITicketingRepository>();
            var builderMock = new Mock<ITicketBuilder>();

            var target = new BuyTicketHandler(builderMock.Object, repositoryMock.Object);
            var consumeCtxMock = new Mock<ConsumeContext<BuyTicket>>();

            var command = this.fixture.Create<BuyTicket>();
            var user = this.fixture.Create<User>();

            var numberOfTicket = fixture.Create<int>();
            var numberOfTicketToBuy = notEnoughTicketsForConcert ? numberOfTicket + 1 : numberOfTicket - 1;

            var concert = new Core.Domain.Concert(fixture.Create<string>(), numberOfTicket);
            var ticketSale = new Core.Domain.TicketSale(fixture.Create<int>(), fixture.Create<int>(), numberOfTicketToBuy);

            var @event1 = this.fixture.Create<TicketsSold>();
            var @event2 = this.fixture.Create<TicketsNotSold>();
            event2.Reason = TicketsNotSoldReason.NotEnoughTicketsForConcert;

            builderMock.Setup(it => it.CreateDomain(command)).Returns(ticketSale);
            builderMock.Setup(it => it.CreateTicketsSoldEvent(concert, user, ticketSale.NumberOfTickets)).Returns(@event1);
            builderMock.Setup(it => it.CreateTicketsNotSoldEvent(concert, user, TicketsNotSoldReason.NotEnoughTicketsForConcert)).Returns(@event2);

            repositoryMock.Setup(it => it.GetUserById(ticketSale.UserId)).ReturnsAsync(user);
            repositoryMock.Setup(it => it.GetConcertById(ticketSale.ConcertId)).ReturnsAsync(concert);

            consumeCtxMock.Setup(it => it.Message).Returns(command);
            consumeCtxMock.Setup(it => it.Publish(@event1, default)).Returns(Task.CompletedTask);
            consumeCtxMock.Setup(it => it.Publish(@event2, default)).Returns(Task.CompletedTask);

            // Act
            await target.Consume(consumeCtxMock.Object);

            // Assert
            using (new AssertionScope())
            {
                if (notEnoughTicketsForConcert)
                {
                    repositoryMock.Verify(it => it.InsertTicketSale(ticketSale), Times.Never);
                    repositoryMock.Verify(it => it.UpdateConcert(concert), Times.Never);
                    consumeCtxMock.Verify(it => it.Publish(@event2, default), Times.Once);
                }
                else
                {
                    repositoryMock.Verify(it => it.InsertTicketSale(ticketSale), Times.Once);
                    repositoryMock.Verify(it => it.UpdateConcert(concert), Times.Once);
                    consumeCtxMock.Verify(it => it.Publish(@event1, default), Times.Once);
                }

            }
        }
    }
}