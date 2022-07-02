using System.Text.Json.Serialization;

namespace DevInSales.Core.Data.DTOs.IdentityDTOs
{
    public class LoginResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccessToken { get; private set; }


        public LoginResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
