using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Azure.Storage.Queues;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using HttpMultipartParser;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace ShowerShow.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private DatabaseContext dbContext;
        private IUserRepository userRepository;
        private string profilePicturesContainer = Environment.GetEnvironmentVariable("ContainerProfilePictures");
        private string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private CloudStorageAccount account;


        public BlobStorageRepository(DatabaseContext dbContext, IUserRepository userRepository)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
        }

        public Task DeleteProfilePicture(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseData> GetProfilePictureOfUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseData> GetVoiceSound(Stream requestBody, string filename)
        {
            throw new NotImplementedException();
        }

        public async Task UploadProfilePicture(Stream requestBody, Guid userId)
        {
            if (!await userRepository.CheckIfUserExistAndActive(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            var parsedFormBody = MultipartFormDataParser.ParseAsync(requestBody);
            var file = parsedFormBody.Result.Files[0];


            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(profilePicturesContainer);
            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(userId.ToString() + ".png");
            blockBlob.Properties.ContentType = file.ContentType;

            if (!blockBlob.Properties.ContentType.Contains("image"))
                throw new BadImageFormatException("You must input an image.");

            await blockBlob.UploadFromStreamAsync(file.Data);
        }

        public Task UploadVoiceSound(Stream requestBody)
        {
            throw new NotImplementedException();
        }

    }
}
