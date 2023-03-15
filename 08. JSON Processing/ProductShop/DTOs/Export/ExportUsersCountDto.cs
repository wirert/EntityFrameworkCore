using Newtonsoft.Json;
namespace ProductShop.DTOs.Export
{
    public class ExportUsersCountDto
    {
        [JsonProperty("usersCount")]
        public int UsersCount => Users.Length;

        [JsonProperty("users")]
        public ExportUserWithProductsDto[] Users { get; set; }
    }
}
