using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeChallenge.Tests.Integration.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        private readonly static JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public static async Task<T> DeserializeContent<T>(this HttpResponseMessage message)
        {
            T responseObject = default;
            if(message != null)
            {
                var responseJson = await message.Content.ReadAsStringAsync();
                responseObject = JsonSerializer.Deserialize<T>(responseJson, jsonSerializerOptions);
            }

            return responseObject;
        }
    }
}
