using AutoFixture;
using Moq;
using NUnit.Framework;
using FluentAssertions.Execution;
using System.Threading.Tasks;
using TicketSales.User.Controllers;
using TicketSales.User.Services;
using TicketSales.User.Mappers;
using TicketSales.User.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace TicketSales.User.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Fixture fixture = new Fixture();

        [SetUp]
        public void BaseSetUp()
        {
            fixture = new Fixture();
        }

        [Test]
        public async Task BuyTicket_Valid_CallsServiceAndReturnsAcceptedResult()
        {
            // Arrange
            var serviceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IModelMapper>();

            var target = new UserController(serviceMock.Object, mapperMock.Object);


            var request = fixture.Create<BuyTicketRequest>();
            var serviceResponse = new ServiceResponse();

            serviceMock.Setup(it => it.BuyTicket(request)).ReturnsAsync(serviceResponse);
            // Act
            var actual = await target.BuyTicket(request);

            // Assert
            using (new AssertionScope())
            {
                serviceMock.Verify(it => it.BuyTicket(request), Times.Once);
                actual.Should().BeOfType<AcceptedResult>();
            }
        }

        [Test]
        public async Task BuyTicket_NotValid_ReturnsBAdRequest()
        {
            // Arrange
            var serviceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IModelMapper>();

            var target = new UserController(serviceMock.Object, mapperMock.Object);

            var errorCode = fixture.Create<ErrorCode>();
            var request = fixture.Create<BuyTicketRequest>();
            var serviceResponse = new ServiceResponse(errorCode);

            serviceMock.Setup(it => it.BuyTicket(request)).ReturnsAsync(serviceResponse);
            // Act
            var actual = await target.BuyTicket(request);

            // Assert
            using (new AssertionScope())
            {
                serviceMock.Verify(it => it.BuyTicket(request), Times.Once);
                actual.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new ValidationErrorResponse() { ErrorCode = errorCode });
            }
        }
    }
}