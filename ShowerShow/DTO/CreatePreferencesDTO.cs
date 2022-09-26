using ShowerShow.Models;
using Newtonsoft.Json;
using System;

namespace ShowerShow.DTO
{
    internal class CreatePreferencesDTO
    {
        [JsonRequired]
        public Guid UserId { get; set; }
        public AvailableVoices SelectedVoice { get; set; } = AvailableVoices.Default_Male;
        public AvailableLanguages SelectedLanguage { get; set; } = AvailableLanguages.English;
    }
}
