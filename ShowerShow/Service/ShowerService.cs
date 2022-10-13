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
    public class ShowerService : IShowerService
    {

        private IShowerDataRepository _showerDataRepository;

        public ShowerService(IShowerDataRepository _showerDataRepository)
        {
            this._showerDataRepository = _showerDataRepository;
        }

        public Task CreateShower(CreateShowerDataDTO shower)
        {
            return _showerDataRepository.CreateShower(shower);
        }

        public Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId)
        {
            return _showerDataRepository.GetShowerDataByUserId(userId, showerId);
            
        }

    }
}
