using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace EnterpriseApplicationIntegration.Core.Converters
{
    public class DashContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            // Count capital letters
            int upperCount = propertyName.Skip(1).Count(x => char.IsUpper(x));
            // Create character array for new name
            char[] newName = new char[propertyName.Length + upperCount];
            // Copy over the first character
            newName[0] = char.ToLowerInvariant(propertyName[0]);

            // Fill the character, and an extra dash for every upper letter
            int iChar = 1;
            for (int iProperty = 1; iProperty < propertyName.Length; iProperty++)
            {
                if (char.IsUpper(propertyName[iProperty]))
                {
                    // Insert dash and then lower-cased char
                    newName[iChar++] = '-';
                    newName[iChar++] = char.ToLowerInvariant(propertyName[iProperty]);
                }
                else
                    newName[iChar++] = propertyName[iProperty];
            }

            return new string(newName, 0, iChar);
        }
    }

    public class DateTimeConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);

                if (item["users"] != null)
                {
                    return DateTime.Now;
                }
            }
            else
            {
                JArray array = JArray.Load(reader);

                return DateTime.Now;
            }

            // This should not happen. Perhaps better to throw exception at this point?
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}