using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChurchWebsite.Areas.Admin.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings);
        }
    }
}