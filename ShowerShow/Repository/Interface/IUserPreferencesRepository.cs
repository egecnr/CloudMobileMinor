using ShowerShow.DTO;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository.Interface
{
    public interface IUserPreferencesRepository
    {
        public Task CreateUserPreferences(Guid userId);
        public Task<PreferencesDTO> GetUserPreferenceById(Guid userId);

        public Task UpdatePreferenceById(Guid userId, PreferencesDTO updatePreferencesDTO);

    }
}
