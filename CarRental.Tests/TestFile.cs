using Xunit;
using Moq;
using DAL;
using Models;
using DAL.Exceptions;

namespace CarRentalApp.Tests
{
    public class VehicleTests
    {
        [Fact]
        // Requirement 10.1: Test if a vehicle is retrieved successfully
        public void GetVehicle_Success_Test()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleRepository>();
            var expectedVehicle = new Vehicle { VehicleID = 1, Brand = "Toyota", Model = "Camry" };
            mockRepo.Setup(repo => repo.GetById(1)).Returns(expectedVehicle);

            // Act
            var result = mockRepo.Object.GetById(1);

            // Assert
            Assert.Equal("Toyota", result.Brand);
        }

        [Fact]
        // Requirement 10.4: Test if your custom exception is thrown for invalid ID
        public void GetVehicle_ThrowsException_Test()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleRepository>();
            mockRepo.Setup(repo => repo.GetById(999))
                    .Throws(new VehicleNotFoundException("Vehicle not found"));

            // Act & Assert
            Assert.Throws<VehicleNotFoundException>(() => mockRepo.Object.GetById(999));
        }
    }
}