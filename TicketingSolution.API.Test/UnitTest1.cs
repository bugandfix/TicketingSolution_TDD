using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq;
using TicketingSolution.API.Controllers;
using Xunit;

namespace TicketingSolution.API.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Should_Return_Forcast_Results()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);
            //Act
            var result = controller.Get();

            //Assert            
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
            

        }
    }
}