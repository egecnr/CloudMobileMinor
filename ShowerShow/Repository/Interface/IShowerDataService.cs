using ShowerShow.DTO;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IShowerDataService
    {
        public Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId);
        public Task CreateShowerData(ShowerData shower);
        public Task AddShowerToQueue(CreateShowerDataDTO shower, Guid userId);
    }
}
