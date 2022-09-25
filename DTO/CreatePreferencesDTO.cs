using FunctionApp1.Models;
using Newtonsoft.Json;
using System;

namespace FunctionApp1.DTO
{
    internal class CreatePreferencesDTO
    {
        [JsonRequired]
        public Guid UserId { get; set; }
        public AvailableVoices SelectedVoice { get; set; } = AvailableVoices.Default_Male;
        public AvailableLanguages SelectedLanguage { get; set; } = AvailableLanguages.English;
    }
}
