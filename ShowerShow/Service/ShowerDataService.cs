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

        public Task CreateShowerDataById(CreateShowerDataDTO shower, Guid userId)
        {
            return _showerDataRepository.CreateShowerDataById(shower, userId);

        }

        public Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId)
        {
            return _showerDataRepository.GetShowerDataByUserId(userId, showerId);

        }

    }
}
