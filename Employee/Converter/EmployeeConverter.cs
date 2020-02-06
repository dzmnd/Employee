using EmployeeBusiness.AbstractClasses;
using EmployeeBusiness.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace EmployeeBusiness.Converter
{
    public class EmployeeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(BaseEmployee));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo.GetValue("TypeEmployee", StringComparison.OrdinalIgnoreCase)?.Value<string>() == "Employee1")
                return  jo.ToObject<Employee1>(serializer);

            if (jo.GetValue("TypeEmployee", StringComparison.OrdinalIgnoreCase)?.Value<string>() == "Employee2")
                return jo.ToObject<Employee2>(serializer);

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
