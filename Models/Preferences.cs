using Newtonsoft.Json;
using System;

namespace FunctionApp1.Models
{
    internal class Preferences
    {
        public Guid Id { get; } = Guid.NewGuid();
        [JsonRequired]
        public Guid UserId { get; set; }
        public AvailableVoices SelectedVoice { get; set; } = AvailableVoices.Default_Male;
        public AvailableLanguages SelectedLanguage { get; set; } = AvailableLanguages.English;
    }
}
