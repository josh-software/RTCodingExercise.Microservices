using System;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.Domain;
using Catalog.Domain.Interfaces;
using IntegrationEvents.Plates;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests.API.Consumers
{
    public class ReservePlateConsumerTests
    {
        private readonly Mock<IPlateRepository> _mockPlateRepository;
        private readonly Mock<ILogger<ReservePlateConsumer>> _mockLogger;
        private readonly Mock<ConsumeContext<ReservePlateEvent>> _mockConsumeContext;
        private readonly ReservePlateConsumer _consumer;

        public ReservePlateConsumerTests()
        {
            _mockPlateRepository = new Mock<IPlateRepository>();
            _mockLogger = new Mock<ILogger<ReservePlateConsumer>>();
            _mockConsumeContext = new Mock<ConsumeContext<ReservePlateEvent>>();
            _consumer = new ReservePlateConsumer(_mockPlateRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Consume_ShouldReservePlate_WhenPlateIsAvailable()
        {
            // Arrange
            var plateId = Guid.NewGuid();
            var plate = new Plate(plateId, "Plate", 0, 1, isReserved: false);

            _mockPlateRepository.Setup(repo => repo.GetAsync(plateId)).ReturnsAsync(plate);
            _mockConsumeContext.Setup(context => context.Message).Returns(new ReservePlateEvent(plateId));

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockPlateRepository.Verify(repo => repo.UpdateAsync(It.Is<Plate>(p => p.IsReserved)), Times.Once);
            //Cannot moq extension methods directly, so we need to verify the log message
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Plate with id {{plateId}} has been reserved."),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Fact]
        public async Task Consume_ShouldLogWarning_WhenPlateIsAlreadyReserved()
        {
            // Arrange
            var plateId = Guid.NewGuid();
            var plate = new Plate(plateId, "Plate", 0, 1, isReserved: true);

            _mockPlateRepository.Setup(repo => repo.GetAsync(plateId)).ReturnsAsync(plate);
            _mockConsumeContext.Setup(context => context.Message).Returns(new ReservePlateEvent(plateId));

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockPlateRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plate>()), Times.Never);
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Plate with id {{plateId}} is already reserved."),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Fact]
        public async Task Consume_ShouldLogWarning_WhenPlateDoesNotExist()
        {
            // Arrange
            var plateId = Guid.NewGuid();

            _mockPlateRepository.Setup(repo => repo.GetAsync(plateId)).ReturnsAsync((Plate?)null);
            _mockConsumeContext.Setup(context => context.Message).Returns(new ReservePlateEvent(plateId));

            // Act
            await _consumer.Consume(_mockConsumeContext.Object);

            // Assert
            _mockPlateRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Plate>()), Times.Never);
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Plate with id {{plateId}} not found."),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }
    }
}
