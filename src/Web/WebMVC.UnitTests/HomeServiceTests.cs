using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Plates;
using DTOs;
using DTOs.Common;
using IntegrationEvents.Plates;
using MassTransit;
using Moq;
using WebMVC.Services;
using Xunit;

namespace WebMVC.UnitTests.Services
{
    public class HomeServiceTests
    {
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
        private readonly Mock<IRequestClient<GetPlatesRequest>> _mockGetPlatesClient;
        private readonly HomeService _homeService;

        public HomeServiceTests()
        {
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();
            _mockGetPlatesClient = new Mock<IRequestClient<GetPlatesRequest>>();
            _homeService = new HomeService(_mockPublishEndpoint.Object, _mockGetPlatesClient.Object);
        }

        [Fact]
        public async Task GetPlatesAsync_ReturnsPaginatedResponse()
        {
            // Arrange
            const int pageNumber = 1;
            const int pageSize = 10;
            var expectedPlates = new List<PlateDto>
            {
                new PlateDto { Id = Guid.NewGuid(), Registration = "Plate1", PurchasePrice = 0, SalePrice = 1 }
            };
            var paginatedResponse = new PaginatedDto<PlateDto>
            {
                Limit = pageSize,
                Offset = (pageNumber - 1) * pageSize,
                Total = 1,
                Items = expectedPlates
            };

            var platesResponse = new GetPlatesResponse { Response = paginatedResponse };
            var responseMock = new Mock<Response<GetPlatesResponse>>();
            responseMock.Setup(r => r.Message).Returns(platesResponse);

            _mockGetPlatesClient
                .Setup(c => c.GetResponse<GetPlatesResponse>(It.IsAny<GetPlatesRequest>(), default, default))
                .ReturnsAsync(responseMock.Object);

            // Act
            var result = await _homeService.GetPlatesAsync(pageNumber, pageSize, "Id", "Asc");

            // Assert
            Assert.NotNull(result);
            var resultItem = result.Items.FirstOrDefault();
            Assert.NotNull(resultItem);
            Assert.Equal(expectedPlates.Count(), result.Items.Count());
            Assert.Equal(expectedPlates[0].Id, result.Items.First().Id);
            Assert.Equal(expectedPlates[0].Registration, result.Items.First().Registration);
            Assert.Equal(paginatedResponse.Total, result.Total);
        }

        [Fact]
        public async Task UpsertPlateAsync_PublishesUpsertPlateEvent()
        {
            // Arrange
            var plateDto = new PlateDto
            {
                Id = Guid.NewGuid(),
                Registration = "Plate123",
                PurchasePrice = 100m,
                SalePrice = 120m
            };

            // Act
            await _homeService.UpsertPlateAsync(plateDto);

            // Assert
            _mockPublishEndpoint.Verify(p => p.Publish(It.Is<UpsertPlateEvent>(e =>
                e.Plate.Id == plateDto.Id &&
                e.Plate.Registration == plateDto.Registration &&
                e.Plate.PurchasePrice == plateDto.PurchasePrice &&
                e.Plate.SalePrice == plateDto.SalePrice
            ), default), Times.Once);
        }

    }
}