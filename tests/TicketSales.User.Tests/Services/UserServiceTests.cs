using AutoFixture;
using Moq;
using NUnit.Framework;
using FluentAssertions.Execution;
using TicketSales.Core.DataAccess;
using MassTransit;
using TicketSales.Messages.Commands;
using System.Threading.Tasks;
using TicketSales.User.Services;
using TicketSales.User.Models;
using FluentAssertions;
using System.Threading;

namespace TicketSales.User.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Fixture fixture = new Fixture();

        [SetUp]
        public void BaseSetUp()
        {
            fixture = new Fixture();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BuyTicket_Valid_PublishCommand(bool notEnoughTicketsForConcert)
        {
            // Arrange
            var repositoryMock = new Mock<ITicketingRepository>();
            var publisherMock = new Mock<IPublishEndpoint>();

            var target = new UserService(publisherMock.Object, repositoryMock.Object);

            var numberOfTicket = fixture.Create<int>();
            var numberOfTicketToBuy = notEnoughTicketsForConcert ? numberOfTicket + 1 : numberOfTicket - 1;

            var request = fixture.Build<BuyTicketRequest>().With(it => it.NumberOfTickets, numberOfTicketToBuy).Create();
            var concert = new Core.Domain.Concert(fixture.Create<string>(), numberOfTicket);
            var user = fixture.Create<Core.Domain.User>();

            var command = new BuyTicket()
            {
                UserId = request.UserId,
                ConcertId = request.ConcertId,
                NumberOfTickets = request.NumberOfTickets,
            };

            publisherMock.Setup(it => it.Publish<BuyTicket>(It.IsAny<BuyTicket>(), default)).Returns(Task.CompletedTask);
            repositoryMock.Setup(it => it.GetConcertById(request.ConcertId)).ReturnsAsync(concert);
            repositoryMock.Setup(it => it.GetUserById(request.UserId)).ReturnsAsync(user);

            // Act
            var actual = await target.BuyTicket(request);

            // Assert
            using (new AssertionScope())
            {
                if (notEnoughTicketsForConcert)
                {
                    actual.Should().BeEquivalentTo(new ServiceResponse(ErrorCode.NotEnoughTicketsForConcert));
                    publisherMock.Verify(it => it.Publish(It.IsAny<BuyTicket>(), default), Times.Never);
                }
                else
                {
                    actual.Should().BeEquivalentTo(new ServiceResponse());
                    publisherMock.Verify(it => it.Publish(It.IsAny<BuyTicket>(), default), Times.Once);
                }
            }
        }
    }
}