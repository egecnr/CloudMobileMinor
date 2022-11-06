using FluentAssertions;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Moq;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using System.Web.Helpers;
using DayOfWeek = ShowerShow.Models.DayOfWeek;
namespace ShowerShowUnitTest
{
    public class ShowerThoughtServiceUnitTest
    {
        private Mock<IShowerThoughtRepository> showerThoughtRepositoryMock = new Mock<IShowerThoughtRepository>();
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private UserService userService;
        private ShowerThoughtService thoughtService;
        public ShowerThoughtServiceUnitTest()
        {
            userService = new UserService(userRepositoryMock.Object);
            thoughtService = new ShowerThoughtService(showerThoughtRepositoryMock.Object, userService);
        }

        [Fact]
        public async Task GetShowerThoughtByIdShouldReturnThought()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);
            Guid id = Guid.NewGuid();
            ShowerThought showerThought = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };

            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(id)).ReturnsAsync(true);
            showerThoughtRepositoryMock.Setup(u => u.GetShowerThoughtById(id)).ReturnsAsync(showerThought);
            //Act
            var result = await thoughtService.GetShowerThoughtById(id);
            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(showerThought.Id);
            result.ShowerId.Should().Be(showerThought.ShowerId);
            result.UserId.Should().Be(showerThought.UserId);
        }
        [Fact]
        public async Task GetShowerThoughtByIdShouldThrowExceptionIfThoughtDoesNotExist()
        {

            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(It.IsAny<Guid>())).ReturnsAsync(false);
            //Act
            Func<Task> exceptionThrown = async () => await thoughtService.GetShowerThoughtById(It.IsAny<Guid>());
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The shower thought could not be found");
        }
        [Fact]
        public async Task GetShowerThoughtByDateShouldReturnThought()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);
            Guid thoughtId = Guid.NewGuid();
            DateTime time = DateTime.ParseExact("11-02-2022", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ShowerThought showerThought = new ShowerThought()
            {
                Id = thoughtId,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = time
            };
            ShowerThought showerThought2 = new ShowerThought()
            {
                Id = thoughtId,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = time
            };
            List<ShowerThought> showerThoughts = new List<ShowerThought> { showerThought, showerThought2 };

            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(thoughtId)).ReturnsAsync(true);
            showerThoughtRepositoryMock.Setup(s => s.GetShowerThoughtsByDate(time, id)).ReturnsAsync(showerThoughts);
            //Act
            var result = await thoughtService.GetShowerThoughtsByDate(time, id);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(showerThoughts);
        }
        [Fact]
        public async Task GetShowerThoughtByDateShouldThrowExceptionIfUserDoesNotExist()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(false);
            ShowerThought showerThought = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            List<ShowerThought> showerThoughts = new List<ShowerThought> { showerThought };

            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(id)).ReturnsAsync(true);
            showerThoughtRepositoryMock.Setup(s => s.GetShowerThoughtsByDate(DateTime.Now, id)).ReturnsAsync(showerThoughts);
            //Act
            Func<Task> act = async () => await thoughtService.GetAllShowerThoughtsForUser(id, 2);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");
        }
        [Fact]
        public async Task GetShowerThoughtByContentShouldReturnThought()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);
            ShowerThought showerThought = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought test",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            List<ShowerThought> showerThoughts = new List<ShowerThought> { showerThought };

            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(id)).ReturnsAsync(true);
            showerThoughtRepositoryMock.Setup(s => s.GetThoughtsByContent("test", id)).ReturnsAsync(showerThoughts);
            //Act
            var result = await thoughtService.GetThoughtsByContent("test", id);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(showerThoughts);
        }
        [Fact]
        public async Task GetShowerThoughtByContentShouldThrowExceptionIfUserDoesNotExist()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(false);
            ShowerThought showerThought = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought test",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            List<ShowerThought> showerThoughts = new List<ShowerThought> { showerThought };

            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(id)).ReturnsAsync(true);
            showerThoughtRepositoryMock.Setup(s => s.GetThoughtsByContent("test", id)).ReturnsAsync(showerThoughts);
            //Act
            Func<Task> act = async () => await thoughtService.GetThoughtsByContent("test", id);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");
        }
        [Fact]
        public async Task GetShowerThoughtByShowerIDShouldReturnThought()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);
            Guid id = Guid.NewGuid();
            Guid showerId = Guid.NewGuid();
            ShowerThought showerThought = new ShowerThought()
            {
                Id = id,
                ShowerId = showerId,
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };

            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(id)).ReturnsAsync(true);
            showerThoughtRepositoryMock.Setup(u => u.GetThoughtByShowerId(showerId)).ReturnsAsync(showerThought);
            //Act
            var result = await thoughtService.GetThoughtByShowerId(showerId);
            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(showerThought.Id);
            result.ShowerId.Should().Be(showerThought.ShowerId);
            result.UserId.Should().Be(showerThought.UserId);
        }
        [Fact]
        public async Task CreateShowerThoughtShouldNotThrowException()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);

            ShowerThoughtDTO showerThoughtDTO = new ShowerThoughtDTO()
            {
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            //Act
            Func<Task> exceptionThrown = async () => await thoughtService.CreateShowerThought(showerThoughtDTO, Guid.NewGuid(), id);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task CreateShowerThoughtShouldNotThrowExceptionBecauseUserDoesNotExist()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(false);

            ShowerThoughtDTO showerThoughtDTO = new ShowerThoughtDTO()
            {
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            //Act
            Func<Task> exceptionThrown = async () => await thoughtService.CreateShowerThought(showerThoughtDTO, id, Guid.NewGuid());
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");
        }
        [Fact]
        public async Task UpdateShowerThoughtShouldUpdateTheShowerThoughtInDatabase()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);
            UpdateShowerThoughtDTO updateShowerThoughtDTO = new UpdateShowerThoughtDTO()
            {
                IsFavorite = false,
                ShareWithFriends = false,
            };

            //Act
            showerThoughtRepositoryMock.Setup(s => s.DoesShowerThoughtExist(It.IsAny<Guid>())).ReturnsAsync(true);
            Func<Task> act = async () => await thoughtService.UpdateThought(Guid.NewGuid(), updateShowerThoughtDTO);
            //Assert
            await act.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateShowerThoughtShouldNotUpdateDueToNotExistingShowerThought()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            showerThoughtRepositoryMock.Setup(u => u.DoesShowerThoughtExist(id)).ReturnsAsync(false);

            UpdateShowerThoughtDTO updateShowerThoughtDTO = new UpdateShowerThoughtDTO()
            {
                IsFavorite = false,
                ShareWithFriends = false,
            };
            //Act
            Func<Task> act = async () => await thoughtService.UpdateThought(id, updateShowerThoughtDTO);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The shower thought could not be found");
        }
        [Fact]
        public async Task DeleteShowerThoughtShouldDeleteShowerThought()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            showerThoughtRepositoryMock.Setup(u => u.DoesShowerThoughtExist(id)).ReturnsAsync(true);

            //Act
            Func<Task> act = async () => await thoughtService.DeleteShowerThought(id);
            //Assert
            await act.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task DeleteShowerThoughtShouldNotDeleteDueToNotExistingShowerThought()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            showerThoughtRepositoryMock.Setup(u => u.DoesShowerThoughtExist(It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            Func<Task> act = async () => await thoughtService.DeleteShowerThought(Guid.NewGuid());
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The shower thought could not be found");
        }
        [Fact]
        public async Task GetAllThoughtsForUserShouldReturnListOfShowerThoughts()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);
            ShowerThought showerThought = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            ShowerThought showerThought2 = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            List<ShowerThought> showerThoughts = new List<ShowerThought> { showerThought, showerThought2 };
            showerThoughtRepositoryMock.Setup(u => u.GetAllShowerThoughtsForUser(id, 2)).ReturnsAsync(showerThoughts);
            //Act
            var result = await thoughtService.GetAllShowerThoughtsForUser(id, 2);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(showerThoughts);
        }
        [Fact]
        public async Task GetAllThoughtsForUserShouldNOTReturnListOfThoughtsBecauseUserDoesNotExist()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(false);
            ShowerThought showerThought = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            ShowerThought showerThought2 = new ShowerThought()
            {
                Id = id,
                ShowerId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsFavorite = true,
                Title = "My shower thought",
                Text = "heya",
                ShareWithFriends = true,
                DateTime = DateTime.Now
            };
            List<ShowerThought> showerThoughts = new List<ShowerThought> { showerThought, showerThought2 };
            showerThoughtRepositoryMock.Setup(u => u.GetAllShowerThoughtsForUser(id, 2)).ReturnsAsync(showerThoughts);
            //Act
            Func<Task> act = async () => await thoughtService.GetAllShowerThoughtsForUser(id, 2);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");


        }
    }

}
