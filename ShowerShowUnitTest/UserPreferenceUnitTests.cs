using FluentAssertions;
using Moq;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShowerShow.Models.Preferences;

namespace ShowerShowUnitTest
{
    public class UserPreferenceUnitTests
    {
        private Mock<IUserService> userServiceMock = new Mock<IUserService>();
        private Mock<IUserPreferencesRepository> userPreferenceRepository = new Mock<IUserPreferencesRepository>();
        private UserPreferencesService sut;

        private PreferencesDTO mockdto;
        Guid testid = Guid.NewGuid();
        public UserPreferenceUnitTests()
        {

            sut = new UserPreferencesService(userPreferenceRepository.Object, userServiceMock.Object);
            mockdto = new PreferencesDTO()
            {
                Currency = AvailableCurrencies.EUR,
                WaterType = AvailableWaterTypes.Liters,
                SelectedLanguage = AvailableLanguages.English,
                SelectedVoice = "voiceid",
                Notification = true,
                Theme = AvailableThemes.Dark
            };
        }


        [Fact]
        public async Task GetPreferenceByUserIdShouldReturnPreferenceDTO()
        {
            //Arrange
            userServiceMock.Setup(u => u.CheckIfUserExistAndActive(testid)).ReturnsAsync(true);
            userPreferenceRepository.Setup(u => u.GetUserPreferenceById(testid)).ReturnsAsync(mockdto);

            //Act
            var result = await sut.GetUserPreferencesById(testid);
            //Assert
            result.Should().NotBeNull();
            result.Currency.Should().Be(AvailableCurrencies.EUR);
            result.WaterType.Should().Be(AvailableWaterTypes.Liters);
            result.SelectedLanguage.Should().Be(AvailableLanguages.English);
        }
        [Fact]
        public async Task GetPreferenceByUserIdShouldNotReturnPreferenceDTODueToUserNotBeingActiveOrNotExisting()
        {
            //Arrange
            userServiceMock.Setup(u => u.CheckIfUserExistAndActive(testid)).ReturnsAsync(false);
            userPreferenceRepository.Setup(u => u.GetUserPreferenceById(testid)).ReturnsAsync(mockdto);

            //Act
            Func<Task> exceptionThrown = async () => await sut.GetUserPreferencesById(testid);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User doesn't exist");
        }


        [Fact]
        public async Task UpdatePreferenceByUserIdShouldReturnPreferenceDTO()
        {
            //Arrange
            userServiceMock.Setup(u => u.CheckIfUserExistAndActive(testid)).ReturnsAsync(true);

            //Assert
            Func<Task> exceptionThrown = async () => await sut.UpdatePreferenceById(testid, mockdto);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task UpdatePreferenceByUserIdShouldNotReturnPreferenceDTODueToUserNotBeingActiveOrNotExisting()
        {
            //Arrange
            userServiceMock.Setup(u => u.CheckIfUserExistAndActive(testid)).ReturnsAsync(false);

            //Act
            Func<Task> exceptionThrown = async () => await sut.UpdatePreferenceById(testid,mockdto);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("User doesn't exist");
        }

    }
}
