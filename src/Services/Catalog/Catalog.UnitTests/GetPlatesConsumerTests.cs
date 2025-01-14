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
                new Plate (Guid.NewGuid(), "Plate1", 0, 1 ),
                new Plate (Guid.NewGuid(), "Plate2", 0, 1 )
            };

            var paginatedPlates = new PaginatedDto<Plate>
            {
                Items = plates,
                Limit = 10,
                Offset = 0,
                Total = 2
            };

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

        [Fact]
        public async Task Consume_ShouldRespondWithEmptyPlates_WhenNoPlatesAvailable()
        {
            // Arrange
            var paginatedPlates = new PaginatedDto<Plate>
            {
                Items = new List<Plate>(),
                Limit = 10,
                Offset = 0,
                Total = 0
            };

            _mockPlateRepository.Setup(repo => repo.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(paginatedPlates);

            _mockConsumeContext.Setup(context => context.Message)
                .Returns(new GetPlatesRequest(10, 0));

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockConsumeContext.Verify(context => context.RespondAsync(It.Is<GetPlatesResponse>(response =>
                response.Response.Items.Count() == 0 &&
                response.Response.Limit == 10 &&
                response.Response.Offset == 0 &&
                response.Response.Total == 0
            )), Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldHandleException_WhenRepositoryFails()
        {
            // Arrange
            _mockPlateRepository.Setup(repo => repo.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Repository error"));

            _mockConsumeContext.Setup(context => context.Message)
                .Returns(new GetPlatesRequest(10, 0));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(_mockConsumeContext.Object));
        }

        [Fact]
        public async Task Consume_ShouldHandleEdgeCase_WhenLimitIsZero()
        {
            // Arrange
            var paginatedPlates = new PaginatedDto<Plate>
            {
                Items = new List<Plate>(),
                Limit = 0,
                Offset = 0,
                Total = 0
            };

            _mockPlateRepository.Setup(repo => repo.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(paginatedPlates);

            _mockConsumeContext.Setup(context => context.Message)
                .Returns(new GetPlatesRequest(0, 0));

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockConsumeContext.Verify(context => context.RespondAsync(It.Is<GetPlatesResponse>(response =>
                response.Response.Items.Count() == 0 &&
                response.Response.Limit == 0 &&
                response.Response.Offset == 0 &&
                response.Response.Total == 0
            )), Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldRespondWithCorrectData_WhenOffsetIsGreaterThanTotal()
        {
            // Arrange
            var plates = new List<Plate>
        {
            new Plate (Guid.NewGuid(), "Plate1", 0, 1 ),
        };

            var paginatedPlates = new PaginatedDto<Plate>
            {
                Items = plates,
                Limit = 10,
                Offset = 15, // Greater than total
                Total = 1
            };

            _mockPlateRepository.Setup(repo => repo.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(paginatedPlates);

            _mockConsumeContext.Setup(context => context.Message)
                .Returns(new GetPlatesRequest(10, 15));

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockConsumeContext.Verify(context => context.RespondAsync(It.Is<GetPlatesResponse>(response =>
                response.Response.Items.Count() == 1 &&
                response.Response.Limit == 10 &&
                response.Response.Offset == 15 &&
                response.Response.Total == 1
            )), Times.Once);
        }
    }
}
