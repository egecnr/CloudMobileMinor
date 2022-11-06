using FluentAssertions;
using Moq;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using DayOfWeek = ShowerShow.Models.DayOfWeek;
namespace ShowerShowUnitTest
{
    public class ScheduleServiceUnitTest
    {
        private Mock<IScheduleRepository> scheduleRepositoryMock = new Mock<IScheduleRepository>();
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private UserService userService;
        private ScheduleService scheduleService;
        public ScheduleServiceUnitTest()
        {
            userService = new UserService(userRepositoryMock.Object);
            scheduleService = new ScheduleService(scheduleRepositoryMock.Object,userService);
        }

        [Fact]
        public async Task GetScheduleByIdShouldReturnSchedule()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);
            Guid id = Guid.NewGuid();
            Schedule schedule = new Schedule()
            {
                Id = id,
                UserId = Guid.NewGuid(),
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }


            };
            scheduleRepositoryMock.Setup(s => s.DoesScheduleExist(id)).ReturnsAsync(true);
            scheduleRepositoryMock.Setup(u => u.GetScheduleById(id)).ReturnsAsync(schedule);
            //Act
            var result = await scheduleService.GetScheduleById(id);
            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(schedule.Id);
        }
        [Fact]
        public async Task GetUserByIdShouldThrowExceptionIfUserDoesNotExist()
        {

            scheduleRepositoryMock.Setup(s => s.DoesScheduleExist(It.IsAny<Guid>())).ReturnsAsync(false);
            //Act
            Func<Task> exceptionThrown = async () => await scheduleService.GetScheduleById(It.IsAny<Guid>());
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The schedule does not exist.");
        }

        [Fact]
        public async Task AddScheduleToQueueShouldSendObjectToQueueTrigger()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);

            CreateScheduleDTO sc = new CreateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }

            };
            //Act
            Func<Task> exceptionThrown = async () => await scheduleService.AddScheduleToQueue(sc,Guid.NewGuid());
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task AddUserToQueueShouldFailDueToNotHavingUniqueEmailAddress()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(false);
            CreateScheduleDTO sc = new CreateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }

            };
            //Act
            Func<Task> exceptionThrown = async () => await scheduleService.AddScheduleToQueue(sc,Guid.NewGuid());
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");
        }
        [Fact]
        public async Task UpdateScheduleShouldUpdateTheScheduleInDatabase()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);

            UpdateScheduleDTO sc = new UpdateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }

            };
            //Act
            scheduleRepositoryMock.Setup(s => s.DoesScheduleExist(It.IsAny<Guid>())).ReturnsAsync(true);
            Func<Task> act = async () => await scheduleService.UpdateSchedule(Guid.NewGuid(), sc);
            //Assert
            await act.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateScheduleShouldNotUpdateDueToNotExistingSchedule()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            scheduleRepositoryMock.Setup(u => u.DoesScheduleExist(id)).ReturnsAsync(false);

            UpdateScheduleDTO sc = new UpdateScheduleDTO()
            {
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }

            };
            //Act
            Func<Task> act = async () => await scheduleService.UpdateSchedule(id, sc);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The schedule does not exist.");
        }
        [Fact]
        public async Task DeleteScheduleShouldDeleteSchedule()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            scheduleRepositoryMock.Setup(u => u.DoesScheduleExist(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            Func<Task> act = async () => await scheduleService.DeleteSchedule(Guid.NewGuid());
            //Assert
            await act.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task DeleteScheduleShouldNotDeleteDueToNotExistingSchedule()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            scheduleRepositoryMock.Setup(u => u.DoesScheduleExist(It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            Func<Task> act = async () => await scheduleService.DeleteSchedule(Guid.NewGuid());
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The schedule does not exist.");
        }
        [Fact]
        public async Task GetAllSchedulesShouldReturnListOfSchedules()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);
            Schedule schedule = new Schedule()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }


            };
            Schedule schedule2 = new Schedule()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }


            };
            List<Schedule> schedules = new List<Schedule> { schedule, schedule2 };
            scheduleRepositoryMock.Setup(u => u.GetAllSchedules(It.IsAny<Guid>())).ReturnsAsync(schedules);
            //Act
            var result = await scheduleService.GetAllSchedules(Guid.NewGuid());
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(schedules);
        }
        [Fact]
        public async Task GetAllSchedulesShouldNOTReturnListOfSchedulesBecauseUserDoesNotExist()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(false);
            Schedule schedule = new Schedule()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }


            };
            Schedule schedule2 = new Schedule()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday },
                Tags = new List<ScheduleTag> { new ScheduleTag() { Name = "test", ActivityDuration = 30, IsWaterOn = true, waterTemperature = ShowerShow.Model.WaterTemperature.Hot } }


            };
            List<Schedule> schedules = new List<Schedule> { schedule, schedule2 };
            scheduleRepositoryMock.Setup(u => u.GetAllSchedules(Guid.NewGuid())).ReturnsAsync(schedules);
            //Act
            Func<Task> act = async () => await scheduleService.GetAllSchedules(Guid.NewGuid());
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");


        }
    }
   
}
