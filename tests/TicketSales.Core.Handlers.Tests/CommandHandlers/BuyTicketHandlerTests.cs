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
        public async Task Consume_Valid_CallsRepositoryMethodsAndPublishEvent()
        {
            // Arrange
            var repositoryMock = new Mock<ITicketingRepository>();
            var builderMock = new Mock<ITicketBuilder>();

            var target = new BuyTicketHandler(builderMock.Object, repositoryMock.Object);
            var consumeCtxMock = new Mock<ConsumeContext<BuyTicket>>();


            var command = this.fixture.Create<BuyTicket>();
            var ticketSale = this.fixture.Create<TicketSale>();
            var user = this.fixture.Create<User>();
            var concert = this.fixture.Create<Concert>();
            var @event = this.fixture.Create<TicketsSold>();

            builderMock.Setup(it => it.CreateDomain(command)).Returns(ticketSale);
            builderMock.Setup(it => it.CreateTicketsSoldEvent(concert, user, ticketSale.NumberOfTickets)).Returns(@event);

            repositoryMock.Setup(it => it.GetUserById(ticketSale.UserId)).ReturnsAsync(user);
            repositoryMock.Setup(it => it.GetConcertById(ticketSale.ConcertId)).ReturnsAsync(concert);

            consumeCtxMock.Setup(it => it.Message).Returns(command);
            consumeCtxMock.Setup(it => it.Publish(@event, default)).Returns(Task.CompletedTask);

            // Act
            await target.Consume(consumeCtxMock.Object);

            // Assert
            using (new AssertionScope())
            {
                repositoryMock.Verify(it => it.InsertTicketSale(ticketSale), Times.Once);
                repositoryMock.Verify(it => it.UpdateConcert(concert), Times.Once);
                consumeCtxMock.Verify(it => it.Publish(@event, default), Times.Once);
            }
        }
    }
}