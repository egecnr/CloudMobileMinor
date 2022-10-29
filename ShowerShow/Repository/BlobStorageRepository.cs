using ShowerShow.Repository.Interface;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using HttpMultipartParser;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace ShowerShow.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private IUserRepository userRepository;
        private string profilePicContainerString = Environment.GetEnvironmentVariable("ContainerProfilePictures");
        private string voicesContainerString = Environment.GetEnvironmentVariable("ContainerDefaultVoices");

        private string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private CloudStorageAccount account;
        private CloudBlobClient blobClient;
        private CloudBlobContainer profilePicContainer;
        private CloudBlobContainer voicesContainer;

        public BlobStorageRepository(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            account = CloudStorageAccount.Parse(connection);
            blobClient = account.CreateCloudBlobClient();
            profilePicContainer = blobClient.GetContainerReference(profilePicContainerString);
            voicesContainer = blobClient.GetContainerReference(voicesContainerString);

        }
        public async Task DeleteProfilePicture(Guid userId)
        {
            if (!await userRepository.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            CloudBlockBlob blockBlob = profilePicContainer.GetBlockBlobReference(userId.ToString() + ".png");
            if (!await profilePicContainer.ExistsAsync() || !await blockBlob.ExistsAsync())
                throw new Exception("The specified container does not exist.");

            await blockBlob.DeleteIfExistsAsync();
        }

        public async Task<HttpResponseData> GetProfilePictureOfUser(HttpResponseData response, Guid userId)
        {
            if (!await userRepository.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");


            CloudBlockBlob blockBlob = profilePicContainer.GetBlockBlobReference(userId.ToString() + ".png");
            if (!await profilePicContainer.ExistsAsync())
                throw new Exception("The specified container does not exist.");

            if (!await blockBlob.ExistsAsync())
            {
                blockBlob = profilePicContainer.GetBlockBlobReference("defaultpicture.png");
                if (!await blockBlob?.ExistsAsync())
                    throw new Exception("Unable to load profile picture.");

            }
            return GetDownloadResponseData(response, blockBlob, "image/jpeg").Result;
        }

        public async Task<HttpResponseData> GetVoiceSound(HttpResponseData response, Stream requestBody, string filename)
        {

            CloudBlockBlob blockBlob = voicesContainer.GetBlockBlobReference(filename);
            if (!await voicesContainer.ExistsAsync() || !await blockBlob.ExistsAsync())
                throw new Exception("The specified container does not exist.");

           return GetDownloadResponseData(response, blockBlob, "audio/mpeg").Result;
        }

        public async Task UploadProfilePicture(Stream requestBody, Guid userId)
        {
            if (!await userRepository.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            var parsedFormBody = MultipartFormDataParser.ParseAsync(requestBody);
            var file = parsedFormBody.Result.Files[0];


            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(profilePicContainerString);
            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(userId.ToString() + ".png");
            blockBlob.Properties.ContentType = file.ContentType;

            if (!blockBlob.Properties.ContentType.Contains("image"))
                throw new BadImageFormatException("You must input an image file.");

            await blockBlob.UploadFromStreamAsync(file.Data);
        }

        public async Task UploadVoiceSound(Stream requestBody)
        {
            var parsedFormBody = MultipartFormDataParser.ParseAsync(requestBody);
            var file = parsedFormBody.Result.Files[0];

            await voicesContainer.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = voicesContainer.GetBlockBlobReference(file.FileName);
            blockBlob.Properties.ContentType = file.ContentType;

            if (!blockBlob.Properties.ContentType.Contains("audio"))
                throw new BadImageFormatException("You must input an audio file.");

            await blockBlob.UploadFromStreamAsync(file.Data);
        }
        public async Task<HttpResponseData> GetDownloadResponseData(HttpResponseData responseData, CloudBlockBlob blockBlob, string ContentType)
        {
            byte[] content = null;
            using (MemoryStream ms = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(ms);
                content = ms.ToArray();
                responseData.WriteBytes(content);
                responseData.Headers.Add("Content-Type", ContentType);
                responseData.Headers.Add("Accept-Ranges", $"bytes");
                responseData.Headers.Add("Content-Disposition", $"attachment; filename={blockBlob.Name}; filename*=UTF-8'{blockBlob.Name}");
            }
            return responseData;
        }
    }
}
