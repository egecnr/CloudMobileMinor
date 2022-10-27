using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Service
{
    public class ShowerDataService : IShowerDataService
    {
        private IShowerDataRepository _showerDataRepository;

        public ShowerDataService(IShowerDataRepository _showerDataRepository)
        {
            this._showerDataRepository = _showerDataRepository;
        }

        public async Task AddShowerToQueue(CreateShowerDataDTO shower, Guid userId)
        {
            await _showerDataRepository.AddShowerToQueue(shower, userId);
        }

        public Task CreateShowerData(ShowerData shower)
        {
            return _showerDataRepository.CreateShowerData(shower);

        }

        public Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId)
        {
            return _showerDataRepository.GetShowerDataByUserId(userId, showerId);

        }

    }
}
