using Microsoft.Azure.Functions.Worker.Http;
using ShowerShow.Repository.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private IBlobStorageRepository blobStorageRepository;

        public BlobStorageService(IBlobStorageRepository blobStorageRepository)
        {
            this.blobStorageRepository = blobStorageRepository;
        }

        public async Task DeleteProfilePicture(Guid userId)
        {
            await blobStorageRepository.DeleteProfilePicture(userId);
        }

        public async Task<HttpResponseData> GetProfilePictureOfUser(HttpResponseData response, Guid userId)
        {
            return await blobStorageRepository.GetProfilePictureOfUser(response, userId);
        }

        public async Task<HttpResponseData> GetVoiceSound(HttpResponseData response, Stream requestBody, string filename)
        {
            return await blobStorageRepository.GetVoiceSound(response,requestBody, filename);
        }

        public async Task UploadProfilePicture(Stream requestBody, Guid userId)
        {
            await blobStorageRepository.UploadProfilePicture(requestBody, userId);
        }

        public async Task UploadVoiceSound(Stream requestBody)
        {
            await blobStorageRepository.UploadVoiceSound(requestBody);
        }
    }
}
