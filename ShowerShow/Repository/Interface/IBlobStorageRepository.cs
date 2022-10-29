using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Threading.Tasks;
using System.IO;

namespace ShowerShow.Repository.Interface
{
    public interface IBlobStorageRepository
    {
        public Task UploadProfilePicture(Stream requestBody, Guid userId);
        public Task DeleteProfilePicture(Guid userId);
        public Task<HttpResponseData> GetProfilePictureOfUser(Guid userId);
        public Task UploadVoiceSound(Stream requestBody);
        public Task<HttpResponseData> GetVoiceSound(Stream requestBody, string filename);
    }
}
