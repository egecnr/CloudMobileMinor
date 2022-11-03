using FluentAssertions;
using Moq;
using ShowerShow.DTO;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShowUnitTest
{
    public class UserFriendServiceUnitTests
    {
        private Mock<IUserFriendRepository> userFriendRepositoryMock = new Mock<IUserFriendRepository>();
        private Mock<IUserService> userServiceMock = new Mock<IUserService>();
        Guid mockid1 = Guid.NewGuid();
        Guid mockid2 = Guid.NewGuid();

        private UserFriendService sut;
        public UserFriendServiceUnitTests()
        {
            sut = new UserFriendService(userFriendRepositoryMock.Object, userServiceMock.Object);
        }

        [Fact]
        public async Task AcceptFriendRequestShouldUpdateUserFriendObjectInDatabase()
        {
            //Arrange
        
            userFriendRepositoryMock.Setup(u =>  u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u =>  u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u =>  u.CheckFriendStatusIsResponseRequired(mockid1, mockid2)).ReturnsAsync(true);
           
            //Act
            Func<Task> exceptionThrown = async () => await sut.AcceptFriendRequest(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task AcceptFriendRequestShouldNotUpdateUserFriendObjectInDatabaseDueToOneOfUsersNotExisting()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(false);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckFriendStatusIsResponseRequired(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.AcceptFriendRequest(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User don't exist.");
        }
        [Fact]
        public async Task AcceptFriendRequestShouldNotUpdateUserFriendObjectInDatabaseDueToUsersBeingAlreadyFriend()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid2, mockid1)).ReturnsAsync(false);
            userFriendRepositoryMock.Setup(u => u.CheckFriendStatusIsResponseRequired(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.AcceptFriendRequest(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Users dont have pending requests with each other");
        }
        [Fact]
        public async Task AcceptFriendRequestShouldNotUpdateUserFriendObjectInDatabaseDueToWrongUserTryingToAcceptRequest()
        {
            //Arrange
           
            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckFriendStatusIsResponseRequired(mockid1, mockid2)).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.AcceptFriendRequest(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage($"Only the user with id {mockid2} can accept friend requests since user with id {mockid1} sent the request");
        }
        [Fact]
        public async Task AddFriendToQueueSuccessfullyAddsAFriendToQueue()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.AddFriendToQueue(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task AddFriendToQueueDontAddFriendToQueueDueToUserNotExisting()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(false);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.AddFriendToQueue(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User don't exist.");
        }
        [Fact]
        public async Task AddFriendToQueueDontAddFriendToQueueDueToUsersBeingFriends()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.AddFriendToQueue(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Users are already friends or request already sent");
        }

        [Fact]
        public async Task ChangeFavoriteStateOfFriendShouldChangeFavoriteStateOfFriend()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.ChangeFavoriteStateOfFriend(mockid1, mockid2,true);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task ChangeFavoriteStateOfFriendShouldNotChangeFavoriteStateOfFriendDueToUsersNotBeingFriends()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.ChangeFavoriteStateOfFriend(mockid1, mockid2, true);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Users dont have pending requests with each other");
        }
        [Fact]
        public async Task ChangeFavoriteStateOfFriendShouldNotChangeFavoriteStateOfFriendDueToUsersNotExisting()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(false);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.ChangeFavoriteStateOfFriend(mockid1, mockid2, true);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User don't exist.");
        }

        //D
        [Fact]
        public async Task DeleteFriendShouldDeleteFriend()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.DeleteFriend(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task DeleteFriendShouldNotDeleteFriendDueToUsersNotBeingFriends()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.DeleteFriend(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Users are not friends with each other");
        }
        [Fact]
        public async Task DeleteFriendShouldNotDeleteFriendDueToUsersNotExisting()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(false);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.DeleteFriend(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User don't exist.");
        }

        [Fact]
        public async Task GetAllFriendsOfUserShouldReturnAnArrayOfUsers()
        {
            //Arrange
            List<GetUserFriendDTO> dtos = new List<GetUserFriendDTO>() { 
            new GetUserFriendDTO(){ 
                MainUserId =mockid2,
                FriendId=mockid1
            },new()
            };

            userServiceMock.Setup(u => u.CheckIfUserExistAndActive(mockid1)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.GetAllFriendsOfUser(mockid1)).ReturnsAsync(dtos);

            //Act
            List<GetUserFriendDTO> result = (List<GetUserFriendDTO>)await sut.GetAllFriendsOfUser(mockid1);
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.First().MainUserId.Should().Be(mockid2);

            result.First().FriendId.Should().Be(mockid1);
            //Assert
        }
        [Fact]
        public async Task GetAllFriendsOfUserShouldThrowExceptionDueToUserNotExisting()
        {
            //Arrange
            

            userServiceMock.Setup(u => u.CheckIfUserExistAndActive(mockid1)).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.GetAllFriendsOfUser(mockid1);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Active user dont exist.");
            //Assert
        }
        [Fact]
        public async Task GetFriendByIdShouldReturnFriend()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.GetUserFriendsById(mockid1, mockid2)).ReturnsAsync(new GetUserFriendDTO()
            {
                MainUserId = mockid2,
                FriendId = mockid1
            });

            //Act
           GetUserFriendDTO result = await sut.GetUserFriendsById(mockid1,mockid2);
            result.Should().NotBeNull();
            result.MainUserId.Should().Be(mockid2);

            result.FriendId.Should().Be(mockid1);
        }
        [Fact]
        public async Task GetFriendByIdFailsDueToUsersNotBeingFriends()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(true);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.GetUserFriendsById(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("Users are not friends with each other");
        }
        [Fact]
        public async Task GetFriendByIdFailsDueToUsersNotExisting()
        {
            //Arrange

            userFriendRepositoryMock.Setup(u => u.CheckIfBothUsersExist(mockid1, mockid2)).ReturnsAsync(false);
            userFriendRepositoryMock.Setup(u => u.CheckIfUserIsAlreadyFriend(mockid1, mockid2)).ReturnsAsync(true);

            //Act
            Func<Task> exceptionThrown = async () => await sut.GetUserFriendsById(mockid1, mockid2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User don't exist.");
        }





    }
}
