using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace TicketSales.Core.Domain.Tests
{
    [TestFixture]
    public class ConcertTests
    {
        private Fixture fixture = new Fixture();

        [SetUp]
        public void BaseSetUp()
        {
            fixture = new Fixture();
        }

        public void Concert_Valid_InitializeProperties()
        {
            // Arrange
            var id = fixture.Create<int>();
            var name = fixture.Create<string>();
            var numberOfTickets = fixture.Create<int>();

            // Act
            var actual = new Concert(id, name, numberOfTickets);

            // Assert
            using (new AssertionScope())
            {
                actual.Id.Should().Be(id);
                actual.Name.Should().Be(name);
                actual.NumberOfTickets.Should().Be(numberOfTickets);
                actual.SoldTickets.Should().Be(0);
            }
        }

        public void SellTicket_Valid_IncrementSoldTicketsAndDecrementNumberOfTickets()
        {
            // Arrange
            var id = fixture.Create<int>();
            var name = fixture.Create<string>();
            var numberOfTickets = fixture.Create<int>();
            var soldTickets = fixture.Create<int>();

            var actual = new Concert(id, name, numberOfTickets);

            // Act
            actual.SellTicket(soldTickets);

            // Assert
            using (new AssertionScope())
            {
                actual.Id.Should().Be(id);
                actual.Name.Should().Be(name);
                actual.NumberOfTickets.Should().Be(numberOfTickets - soldTickets);
                actual.SoldTickets.Should().Be(soldTickets);
            }
        }
    }
}