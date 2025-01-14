using System;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.Domain;
using Catalog.Domain.Interfaces;
using DTOs;
using IntegrationEvents.Plates;
using MassTransit;
using Moq;
using Xunit;

namespace Catalog.UnitTests.API.Consumers
{
    public class UpsertPlateConsumerTests
    {
        private readonly Mock<IPlateRepository> _mockPlateRepository;
        private readonly Mock<ConsumeContext<UpsertPlateEvent>> _mockConsumeContext;
        private readonly UpsertPlateConsumer _consumer;

        public UpsertPlateConsumerTests()
        {
            _mockPlateRepository = new Mock<IPlateRepository>();
            _mockConsumeContext = new Mock<ConsumeContext<UpsertPlateEvent>>();
            _consumer = new UpsertPlateConsumer(_mockPlateRepository.Object);
        }

        [Fact]
        public async Task Consume_ShouldAddNewPlate_WhenPlateDoesNotExist()
        {
            // Arrange
            var plateDto = new PlateDto { Id = Guid.NewGuid(), Registration = "New Plate" };
            var upsertEvent = new UpsertPlateEvent(plateDto);
            _mockConsumeContext.Setup(context => context.Message).Returns(upsertEvent);

            _mockPlateRepository.Setup(repo => repo.GetAsync(plateDto.Id))
                .ReturnsAsync((Plate?)null); //Simulating that the plate doesn't exist

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockPlateRepository.Verify(repo => repo.AddAsync(It.IsAny<Plate>()), Times.Once);
            _mockPlateRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plate>()), Times.Never);
        }

        [Fact]
        public async Task Consume_ShouldUpdateExistingPlate_WhenPlateExists()
        {
            // Arrange
            var plateDto = new PlateDto { Id = Guid.NewGuid(), Registration = "Existing Plate" };
            var upsertEvent = new UpsertPlateEvent(plateDto);
            var existingPlate = new Plate(plateDto.Id, plateDto.Registration, 0, 1);
            _mockConsumeContext.Setup(context => context.Message).Returns(upsertEvent);

            _mockPlateRepository.Setup(repo => repo.GetAsync(plateDto.Id))
                .ReturnsAsync(existingPlate); //Simulating that the plate exists

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockPlateRepository.Verify(repo => repo.AddAsync(It.IsAny<Plate>()), Times.Never);
            _mockPlateRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plate>()), Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldHandleException_WhenRepositoryFails()
        {
            // Arrange
            var plateDto = new PlateDto { Id = Guid.NewGuid(), Registration = "Plate1" };
            var upsertEvent = new UpsertPlateEvent(plateDto);
            _mockConsumeContext.Setup(context => context.Message).Returns(upsertEvent);

            _mockPlateRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(_mockConsumeContext.Object));
        }
    }
}
