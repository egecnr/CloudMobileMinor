using ShowerShow.Models;
using Newtonsoft.Json;
using System;
using static ShowerShow.Models.Preferences;

namespace ShowerShow.DTO
{
    public class CreatePreferencesDTO
    {
        [JsonRequired]
        public Guid UserId { get; set; }
        public AvailableVoices SelectedVoice { get; set; } = AvailableVoices.Default_Male;
        public AvailableLanguages SelectedLanguage { get; set; } = AvailableLanguages.English;

        public AvailableWaterTypes WaterType;

        public AvailableCurrencies Currency;

        public AvailableThemes Theme;

        public bool Notification { get; set; }
    }
}
