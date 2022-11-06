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
        private IUserService userService;

        public ShowerDataService(IShowerDataRepository _showerDataRepository,IUserService userService)
        {
            this._showerDataRepository = _showerDataRepository;
            this.userService = userService;
        }

        public async Task AddShowerToQueue(CreateShowerDataDTO shower, Guid userId)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new Exception("Invalid user.");

                await _showerDataRepository.AddShowerToQueue(shower, userId);
        }

        public async Task CreateShowerData(ShowerData shower)
        {
            await _showerDataRepository.CreateShowerData(shower);
        }

        public async Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId)
        {
            if (!await userService.CheckIfUserExistAndActive(userId))
                throw new Exception("Invalid user.");

            return await _showerDataRepository.GetShowerDataByUserId(userId, showerId);

        }

    }
}
