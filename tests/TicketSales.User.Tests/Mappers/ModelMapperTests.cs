using FluentAssertions;
using AutoFixture;
using NUnit.Framework;
using TicketSales.User.Mappers;
using FluentAssertions.Execution;
using System.Linq;
using TicketSales.User.Models;

namespace TicketSales.User.Tests.Mappers
{
    [TestFixture]
    public class ModelMapperTests
    {
        private Fixture fixture = new Fixture();

        [SetUp]
        public void BaseSetUp()
        {
            fixture = new Fixture();
        }

        [Test]
        public void Map_Valid_ReturnsMapped()
        {
            // Arrange            
            var target = new ModelMapper();
            var input = fixture.CreateMany<Core.Domain.Concert>();


            // Act
            var actual = target.Map(input);

            // Assert
            // Assert
            using (new AssertionScope())
            {
                actual.Items.Count.Should().Be(input.Count());

                for (int i = 0; i < input.Count(); i++)
                {
                    var item = input.ElementAt(i);
                    var modelItem = actual.Items[i];

                    modelItem.Should().BeEquivalentTo(new Concert() { Id = item.Id, Name = item.Name });
                }
            }
        }
    }
}