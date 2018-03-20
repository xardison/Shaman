using Newtonsoft.Json;

namespace Shaman.Common.Serialize
{
    public static class Serializer
    {
        private static readonly JsonSerializerSettings JsonSettings;

        static Serializer()
        {
            JsonSettings = new JsonSerializerSettings
            {
                //ContractResolver = new SisoJsonDefaultContractResolver();
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Objects
            };
        }

        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, JsonSettings);
        }
        public static T FromJson<T>(string value)
        {
            var type = typeof(T);
            return (T)JsonConvert.DeserializeObject(value, type, JsonSettings);
        }
        public static T MakeCopy<T>(this T value)
        {
            var json = ToJson(value);
            return FromJson<T>(json);
        }
    }
}