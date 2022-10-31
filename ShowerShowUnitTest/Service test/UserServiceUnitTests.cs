using FluentAssertions;
using Moq;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using System.Web.Helpers;

namespace ShowerShowUnitTest
{
    public class UserServiceUnitTests
    {
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private UserService sut;
        public UserServiceUnitTests()
        {
            sut = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserByIdShouldReturnUserDTO()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);
            Guid id = Guid.NewGuid();
            GetUserDTO getUserDTO = new GetUserDTO()
            {
                Id = id,
                UserName = "DRPEpper",
                Email = "0",
                Name = "DR"
            };
            userRepositoryMock.Setup(u => u.GetUserById(id)).ReturnsAsync(getUserDTO);
            //Act
            var result = await sut.GetUserById(id);
            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(getUserDTO.Id);
            result.Email.Should().Be(getUserDTO.Email);
            result.UserName.Should().Be(getUserDTO.UserName);
        }
        [Fact]
        public async Task GetUserByIdShouldThrowExceptionIfUserDoesNotExist()
        {
            //Arrange
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.GetUserById(It.IsAny<Guid>());
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User does not exist");
        }

        [Fact]
        public async Task AddUserToQueueShouldSendObjectToQueueTrigger()
        {
            //Arrange
            string email = "mockEmail";
            string username = "mockname";
            userRepositoryMock.Setup(u => u.CheckIfEmailExist(email)).ReturnsAsync(false);
            userRepositoryMock.Setup(u => u.CheckIfUserNameExist(username)).ReturnsAsync(false);
            CreateUserDTO us = new CreateUserDTO()
            {
                Email = email,
                PasswordHash = "SuperSecretPassword",
                UserName = username
            };

            //Act
            Func<Task> exceptionThrown = async () => await sut.AddUserToQueue(us);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task AddUserToQueueShouldFailDueToNotHavingUniqueEmailAddress()
        {
            //Arrange
            string email = "mockEmail";
            string username = "mockname";
            userRepositoryMock.Setup(u => u.CheckIfEmailExist(email)).ReturnsAsync(true);
            CreateUserDTO us = new CreateUserDTO()
            {
                Email = email,
                PasswordHash = "SuperSecretPassword",
                UserName = username
            };

            //Act
            Func<Task> exceptionThrown = async () => await sut.AddUserToQueue(us);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Please pick a unique email address");
        }
        [Fact]
        public async Task AddUserToQueueShouldFailDueToNotHavingUniqueUsername()
        {
            //Arrange
            string email = "mockEmail";
            string username = "mockname";
            userRepositoryMock.Setup(u => u.CheckIfUserNameExist(username)).ReturnsAsync(true);

            CreateUserDTO us = new CreateUserDTO()
            {
                Email = email,
                PasswordHash = "SuperSecretPassword",
                UserName = username
            };

            //Act
            Func<Task> exceptionThrown = async () => await sut.AddUserToQueue(us);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Please pick a unique username");
        }
        [Fact]
        public async Task UpdateUserShouldUpdateTheUserInDatabase()
        {
            //Arrange
            string email = "mockEmail";
            string username = "mockname";
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);
            userRepositoryMock.Setup(u => u.CheckIfEmailExist(email)).ReturnsAsync(false);
            userRepositoryMock.Setup(u => u.CheckIfUserNameExist(username)).ReturnsAsync(false);

            UpdateUserDTO us = new UpdateUserDTO()
            {
                Email=email,
                UserName = username
            };
            //Act
            Func<Task> act = async () => await sut.UpdateUser(id,us);
            //Assert
            await act.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateUserShouldNotUpdateDueToNotExistingUser()
        {
            //Arrange
            string email = "mockEmail";
            string username = "mockname";
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(false);
            userRepositoryMock.Setup(u => u.CheckIfEmailExist(email)).ReturnsAsync(false);
            userRepositoryMock.Setup(u => u.CheckIfUserNameExist(username)).ReturnsAsync(false);

            UpdateUserDTO us = new UpdateUserDTO()
            {
                Email = email,
                UserName = username
            };
            //Act
            Func<Task> act = async () => await sut.UpdateUser(id, us);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("User does not exist");
        }
        [Fact]
        public async Task UpdateUserShouldNotUpdateDueToNotHavingUniqueEmail()
        {
            //Arrange
            string email = "mockEmail";
            string username = "mockname";
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);
            userRepositoryMock.Setup(u => u.CheckIfEmailExist(id,email)).ReturnsAsync(true);
            userRepositoryMock.Setup(u => u.CheckIfUserNameExist(username)).ReturnsAsync(false);

            UpdateUserDTO us = new UpdateUserDTO()
            {
                Email = email,
                UserName = username
            };
            //Act
            Func<Task> act = async () => await sut.UpdateUser(id, us);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Email has to be unique");
        }
        [Fact]
        public async Task UpdateUserShouldNotUpdateDueToNotHavingUniqueUserName()
        {
            //Arrange
            string email = "mockEmail";
            string username = "mockname";
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(id)).ReturnsAsync(true);
            userRepositoryMock.Setup(u => u.CheckIfUserNameExist(id,username)).ReturnsAsync(true);

            UpdateUserDTO us = new UpdateUserDTO()
            {
                Email = email,
                UserName = username
            };
            //Act
            Func<Task> act = async () => await sut.UpdateUser(id, us);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Username has to be unique");
        }
        [Fact]
        public async Task DeactivateUserAccountShouldDeactivateAccountInDatabase()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExist(id)).ReturnsAsync(true);
            //Act
            Func<Task> act = async () => await sut.DeactivateUserAccount(id,false);
            //Assert
            await act.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task DeactivateUserAccountShouldThrowExceptionDueToNotExistingUser()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExist(id)).ReturnsAsync(false);
            //Act
            Func<Task> act = async () => await sut.DeactivateUserAccount(id, false);
            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("User does not exist");
        }

        [Fact]
        public async Task GetUserByNameShouldThrowExceptionIfUsernameisNull()
        {
            //Arrange
            string username = null;
            //Act
            Func<Task> exceptionThrown = async () => await sut.GetUsersByName(username);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Please input a username");
        }

        [Fact]
        public async Task GetUserByNameShouldNotThrowExceptionIfUsernameisNotNull()
        {
            //Arrange
            string username = "tom";
            List<GetUserDTO> users = new List<GetUserDTO>() {   new GetUserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "tommie",
                Email = "0",
                Name = "DR"
            } };
        
            userRepositoryMock.Setup(u => u.GetUsersByName(username)).ReturnsAsync(users);
            //Act
            var result = await sut.GetUsersByName(username);
            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.First().UserName.Should().Be("tommie");
        }
    }
}

