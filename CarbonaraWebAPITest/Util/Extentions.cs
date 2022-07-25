using Newtonsoft.Json;
using System.Text;

namespace CarbonaraWebAPITest.Util
{
    public static class Extentions
    {
        public static StringContent ConvertToStringBody(this Object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<T> ConvertToTypeFromStringAsync<T>(this HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
