using AutoFixture;
using Moq;
using NUnit.Framework;
using TicketSales.Core.DataAccess;
using TicketSales.Core.Handlers.Builders;
using MassTransit;
using TicketSales.Core.Domain;
using System.Threading.Tasks;
using TicketSales.Messages.Events;
using TicketSales.Core.Handlers.EventHandlers;

namespace TicketSales.Core.Handlers.Tests.EventHandlers
{
    [TestFixture]
    public class TicketsSoldHandlerTests
    {
        private Fixture fixture = new Fixture();

        [SetUp]
        public void BaseSetUp()
        {
            fixture = new Fixture();
        }

        [Test]
        public async Task Consume_Valid_CallsRepositoryMethods()
        {
            // Arrange
            var repositoryMock = new Mock<ITicketingRepository>();
            var builderMock = new Mock<ITicketBuilder>();

            var target = new TicketsSoldHandler(builderMock.Object, repositoryMock.Object);
            var consumeCtxMock = new Mock<ConsumeContext<TicketsSold>>();


            var command = fixture.Create<TicketsSold>();
            var viewEntity = fixture.Create<SoldTicketView>();

            consumeCtxMock.Setup(it => it.Message).Returns(command);

            builderMock.Setup(it => it.CreateSoldTicketView(command)).Returns(viewEntity);

            repositoryMock.Setup(it => it.InsertSoldTicketView(viewEntity)).Returns(Task.CompletedTask);

            // Act
            await target.Consume(consumeCtxMock.Object);

            // Assert            
            repositoryMock.Verify(it => it.InsertSoldTicketView(viewEntity), Times.Once);
        }
    }
}