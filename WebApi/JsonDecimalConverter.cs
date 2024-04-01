using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi
{
    public class JsonDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(
       ref Utf8JsonReader reader,
       Type typeToConvert,
       JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            return string.IsNullOrWhiteSpace(stringValue)
                ? default
                : decimal.Parse(stringValue, CultureInfo.InvariantCulture);
        }

        public override void Write(
            Utf8JsonWriter writer,
            decimal value,
            JsonSerializerOptions options)
        {
            var numberAsString = value.ToString("F2", CultureInfo.InvariantCulture);
            writer.WriteStringValue(numberAsString);
        }
    }
}
