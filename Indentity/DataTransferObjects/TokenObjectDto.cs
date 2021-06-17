using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DataTransferObjects
{

    /// <summary>
    /// The model for creating a tokenobject
    /// </summary>
    public class TokenObjectDto
    {
        // This holds the actual string
        [JsonProperty(PropertyName = "access_token")]
        public string AcessToken { get; set; }

        // Token type will be "bearer"
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        // This is the number of seconds until the token expires
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        // This will be the users emailaddress
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        // This is the exact time the token is issued
        [JsonProperty(PropertyName = ".issued")]
        public string TokenIssued { get; set; }

        // This is the exact time the token will expire
        [JsonProperty(PropertyName = ".expires")]
        public string TokenExpires { get; set; }
    }
}
