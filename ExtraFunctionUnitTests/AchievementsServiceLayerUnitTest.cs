using ExtraFunction.Model;
using ExtraFunction.Repository_.Interface;
using ExtraFunction.Service_;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunctionUnitTests
{
    public class AchievementsServiceLayerUnitTest
    {
        //mock repository of achievements
        //use the actual service of the achievements
        //pass it down in the ctor
        //create each test for the endpoint I have

        private Mock<IAchievementRepository> _achievementRepositoryMock = new Mock<IAchievementRepository>();
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>(); 
        private AchievementService _achievementService;

        public AchievementsServiceLayerUnitTest()
        {
            _achievementService = new AchievementService(_achievementRepositoryMock.Object, _userRepositoryMock.Object);
        }


        [Fact]
        public async Task Get_Achievements_By_Id_Should_Return_A_List_Of_Achievements()
        {
            User mockUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Wim",
                UserName = "Java Developer",
                PasswordHash = "123456",
                Email = "wimjavalover@java.com",
                Achievements = new List<Achievement>()
                {
                    new Achievement("Perfect week", "Using sawa as 7 times a week", 0, 7)
                },
                isAccountActive = true,

            };

            _userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(It.IsAny<Guid>())).ReturnsAsync(true);       
            _achievementRepositoryMock.Setup(a => a.GetAchievementsById(mockUser.Id)).ReturnsAsync(mockUser.Achievements); //setup

            List<Achievement> achievements = await _achievementService.GetAchievementsById(mockUser.Id);

            Assert.Equal(mockUser.Achievements, achievements);

        }

        public async Task Get_Achievement_By_Id_And_Title_Should_Return_One_Achievement()
        {

        }


    }
}
