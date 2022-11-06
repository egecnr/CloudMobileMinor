using FluentAssertions;
using Moq;
using ShowerShow.Model;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
namespace ShowerShowUnitTest
{
    public class UserStatisticsServiceUnitTest
    {
        private Mock<IUserStatisticsRepository> userStatisticsRepositoryMock = new Mock<IUserStatisticsRepository>();
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private UserService userService;
        private UserStatisticsService userStatisticsService;
        public UserStatisticsServiceUnitTest()
        {
            userService = new UserService(userRepositoryMock.Object);
            userStatisticsService = new UserStatisticsService(userStatisticsRepositoryMock.Object,userService);
        }

        [Fact]
        public async Task GetFriendRankingShouldReturnFriendRanking()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(true);
            Dictionary<Guid,double> friendsRanking = new Dictionary<Guid,double>();
            friendsRanking.Add(Guid.NewGuid(), 10);
            friendsRanking.Add(Guid.NewGuid(), 30);
            userStatisticsRepositoryMock.Setup(u => u.GetFriendRanking(userId,2)).ReturnsAsync(friendsRanking);
            //Act
            var result = await userStatisticsService.GetFriendRanking(userId, 2);
            //Assert
            result.Should().NotBeNull();
            result.ElementAt(0).Value.Should().BeLessThanOrEqualTo(result.ElementAt(1).Value);
            result.Should().BeEquivalentTo(friendsRanking);
        }
        [Fact]
        public async Task GetFriendRankingShouldThrowExceptionBecauseInvalidUser()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(false);
            Dictionary<Guid, double> friendsRanking = new Dictionary<Guid, double>();
            friendsRanking.Add(Guid.NewGuid(), 10);
            friendsRanking.Add(Guid.NewGuid(), 30);
            userStatisticsRepositoryMock.Setup(u => u.GetFriendRanking(userId, 2)).ReturnsAsync(friendsRanking);
            //Act
            Func<Task> exceptionThrown = async () => await userStatisticsService.GetFriendRanking(userId, 2);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");

        }
        [Fact]
        public async Task GetUserDashboardShouldReturnUserDashboard()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(true);
            UserDashboard userDashboard = new UserDashboard()
            {
                UserId = userId,
                TotalWaterUsage = 30.49,
                TotalGasUsage = 20.30,
                TotalPrice = 85,
                TotalOvertime = 54,
                AvgShowerTime = 14,
                AvgShowerLiters = 10,
                AvgShowerGas = 5,
                AvgShowerPrice = 23.3,
                AvgShowerOvertime = 34
            };
            userStatisticsRepositoryMock.Setup(u => u.GetUserDashboard(userId, 7)).ReturnsAsync(userDashboard);
            //Act
            var result = await userStatisticsService.GetUserDashboard(userId, 7);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userDashboard);
           
        }
        [Fact]
        public async Task GetUserDashboardShouldThrowExceptionIfUserInvalid()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(false);
            UserDashboard userDashboard = new UserDashboard()
            {
                UserId = userId,
                TotalWaterUsage = 30.49,
                TotalGasUsage = 20.30,
                TotalPrice = 85,
                TotalOvertime = 54,
                AvgShowerTime = 14,
                AvgShowerLiters = 10,
                AvgShowerGas = 5,
                AvgShowerPrice = 23.3,
                AvgShowerOvertime = 34
            };
            userStatisticsRepositoryMock.Setup(u => u.GetUserDashboard(userId, 7)).ReturnsAsync(userDashboard);
            //Act
            Func<Task> exceptionThrown = async () => await userStatisticsService.GetUserDashboard(userId, 7);
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");
        }
    }
   
}
