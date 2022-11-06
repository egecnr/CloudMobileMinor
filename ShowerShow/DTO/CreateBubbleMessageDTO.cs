using Newtonsoft.Json;

namespace ShowerShow.DTO
{
    public class CreateBubbleMessageDTO
    {
        [JsonRequired]
        public string Message { get; set; }
    }
}
