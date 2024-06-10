using Newtonsoft.Json;

namespace ClothesStoreAPI.Models
{
    public partial class SuccessResponse
    {
        [JsonProperty("message")]
        public string Msg { get; set; }

        public SuccessResponse(string msg)
        {
            this.Msg = msg;
        }
    }
}