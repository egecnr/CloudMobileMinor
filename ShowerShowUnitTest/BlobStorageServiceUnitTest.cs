using FluentAssertions;
using Moq;
using ShowerShow.Repository.Interface;
using ShowerShow.Service;
namespace ShowerShowUnitTest
{
    public class BlobStorageServiceUnitTest
    {
        private Mock<IBlobStorageRepository> blobStorageRepositoryMock= new Mock<IBlobStorageRepository>();
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private UserService userService;
        private BlobStorageService blobStorageService;
        public BlobStorageServiceUnitTest()
        {
            userService = new UserService(userRepositoryMock.Object);
            blobStorageService = new BlobStorageService(blobStorageRepositoryMock.Object,userService);
        }

        [Fact]
        public async Task UploadProfilePictureShouldUploadPicture()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(true);

            Stream stream = new MemoryStream();
            //Act
            Func<Task> exceptionThrown = async () => await blobStorageService.UploadProfilePicture(stream,userId);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task UploadProfilePictureShouldThrowErrorInexistentuser()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(false);

            Stream stream = new MemoryStream();
            //Act
            Func<Task> exceptionThrown = async () => await blobStorageService.UploadProfilePicture(stream, userId);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");
        }
        [Fact]
        public async Task DeleteProfilePictureShouldDeletePicture()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(true);

            Stream stream = new MemoryStream();
            //Act
            Func<Task> exceptionThrown = async () => await blobStorageService.DeleteProfilePicture(userId);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task DeleteProfilePictureShouldThrowExceptionIfInvalidUser()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            userRepositoryMock.Setup(u => u.CheckIfUserExistAndActive(userId)).ReturnsAsync(false);

            Stream stream = new MemoryStream();
            //Act
            Func<Task> exceptionThrown = async () => await blobStorageService.DeleteProfilePicture(userId);
            //Assert
            await exceptionThrown.Should().ThrowAsync<Exception>().WithMessage("The user does not exist or is inactive.");
        }
        [Fact]
        public async Task UploadVoiceSoundShouldUploadSound()
        {
            Stream stream = new MemoryStream();
            //Act
            Func<Task> exceptionThrown = async () => await blobStorageService.UploadVoiceSound(stream);
            //Assert
            await exceptionThrown.Should().NotThrowAsync<Exception>();
        }
    }
   
}
