using ShowerShow.DTO;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IShowerDataRepository
    {
        public Task<ShowerData> GetShowerDataByUserId(Guid userId, Guid showerId);
        public Task CreateShowerDataById(CreateShowerDataDTO shower, Guid userId);
    }
}
