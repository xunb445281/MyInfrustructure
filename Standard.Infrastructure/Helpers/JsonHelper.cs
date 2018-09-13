using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Helpers
{
    public static class JsonHelper
    {
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
