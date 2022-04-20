using FluentAssertions;
using AutoFixture;
using NUnit.Framework;
using TicketSales.Core.Handlers.Builders;
using TicketSales.Messages.Commands;
using TicketSales.Core.Domain;

namespace TicketSales.Core.Handlers.Tests.Builder
{
    [TestFixture]
    public class TicketBuilderTests
    {
        private Fixture fixture = new Fixture();

        [SetUp]
        public void BaseSetUp()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public void CreateDomain_Valid_CreateDomainEnity()
        {
            // Arrange            
            var target = new TicketBuilder();
            var command = this.fixture.Create<BuyTicket>();

            // Act
            var actual = target.CreateDomain(command);

            // Assert
            actual.Should().BeEquivalentTo(new TicketSale(command.UserId, command.ConcertId, command.NumberOfTickets));
        }
    }
}