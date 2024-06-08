using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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