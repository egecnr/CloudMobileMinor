using FluentAssertions;
using Microsoft.AspNetCore.Components.Routing;
using Moq;
using FluentAssertions;
using Moq;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using System.Web.Helpers;
using ShowerShow.Model;
using ShowerShow.Models;

namespace ShowerShowUnitTest
{
    public class ShowerDataServiceUnitTest
    {
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private Mock<IShowerDataRepository> showerDataRepositoryMock = new Mock<IShowerDataRepository>();
        private ShowerDataService showerDataService;
        private UserService userService;
        public ShowerDataServiceUnitTest()
        {
            userService = new UserService(userRepositoryMock.Object);
            showerDataService = new ShowerDataService(showerDataRepositoryMock.Object, userService);
        }

        [Fact]
        public async Task GetShowerDataByIdShouldReturnShowerData()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Guid showerId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);
            ShowerData showerData = new ShowerData()
            {
                UserId = id,
                Duration = 30,
                WaterUsage = 15,
                WaterCost = 10,
                GasUsage = 10,
                GasCost = 10,
                Date = DateTime.Now,
                ScheduleId = Guid.NewGuid(),
                Overtime = 15
            };


            showerDataRepositoryMock.Setup(u => u.GetShowerDataByUserId(id, showerId)).ReturnsAsync(showerData);
            //Act
            var result = await showerDataService.GetShowerDataByUserId(id, showerId);
            //Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(showerData.UserId);
        }
        [Fact]
        public async Task GethowerDataByIdShouldThrowExceptionIfUserDoesNotExist()
        {

            userRepositoryMock.Setup(s => s.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(false);
            //Act
            Func<Task> exceptionThrown = async () => await showerDataService.GetShowerDataByUserId(Guid.NewGuid(),Guid.NewGuid());
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Invalid user.");
        }
        [Fact]
        public async Task AddShowerDataToQueueShouldSendObjectToQueueTrigger()
        {
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);

            //Arrange
            CreateShowerDataDTO showerDataDTO = new CreateShowerDataDTO()
            {
                Duration = 30,
                WaterUsage = 15,
                WaterCost = 10,
                GasUsage = 10,
                GasCost = 10,
                Date = DateTime.Now,
                ScheduleId = Guid.NewGuid(),
                Overtime = 15
            };

            //Act
            Func<Task> exceptionThrown = async () => await showerDataService.AddShowerToQueue(showerDataDTO,id);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task AddShowerDataToQueueShouldFailDueToInvalidUser()
        {
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(false);

            //Arrange
            CreateShowerDataDTO showerDataDTO = new CreateShowerDataDTO()
            {
                Duration = 30,
                WaterUsage = 15,
                WaterCost = 10,
                GasUsage = 10,
                GasCost = 10,
                Date = DateTime.Now,
                ScheduleId = Guid.NewGuid(),
                Overtime = 15
            };

            //Act
            Func<Task> exceptionThrown = async () => await showerDataService.AddShowerToQueue(showerDataDTO,id);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Invalid user.");
        }
    }
}
