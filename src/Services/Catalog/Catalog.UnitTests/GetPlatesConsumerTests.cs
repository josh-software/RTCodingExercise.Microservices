using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.Domain;
using Catalog.Domain.Interfaces;
using Contracts.Plates;
using DTOs.Common;
using MassTransit;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
    public class GetPlatesConsumerTests
    {
        private readonly Mock<IPlateRepository> _mockPlateRepository;
        private readonly Mock<ConsumeContext<GetPlatesRequest>> _mockConsumeContext;
        private readonly GetPlatesConsumer _consumer;

        public GetPlatesConsumerTests()
        {
            _mockPlateRepository = new Mock<IPlateRepository>();
            _mockConsumeContext = new Mock<ConsumeContext<GetPlatesRequest>>();
            _consumer = new GetPlatesConsumer(_mockPlateRepository.Object);
        }

        [Fact]
        public async Task Consume_ShouldRespondWithPlates()
        {
            // Arrange
            var plates = new List<Plate>
            {
                new Plate { Id = Guid.NewGuid(), Registration = "Plate1", PurchasePrice = 0, SalePrice = 1 },
                new Plate { Id = Guid.NewGuid(), Registration = "Plate2", PurchasePrice = 0, SalePrice = 1 }
            };

            var paginatedPlates = new PaginatedDto<Plate>
            {
                Items = plates,
                Limit = 10,
                Offset = 0,
                Total = 2
            };

            _mockPlateRepository.Setup(repo => repo.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
            _mockPlateRepository.Setup(repo => repo.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(paginatedPlates);

            _mockConsumeContext.Setup(context => context.Message)
                .Returns(new GetPlatesRequest(10, 0));

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockConsumeContext.Verify(context => context.RespondAsync(It.Is<GetPlatesResponse>(response =>
                response.Response.Items.Count() == 2 &&
                response.Response.Limit == 10 &&
                response.Response.Offset == 0 &&
                response.Response.Total == 2
            )), Times.Once);
        }
    }
}
