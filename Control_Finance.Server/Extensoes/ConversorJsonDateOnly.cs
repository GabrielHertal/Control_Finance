using System.Text.Json;
using System.Text.Json.Serialization;

namespace Control_Finance.Server.Extensoes
{
    public class ConversorJsonDateOnly : JsonConverter<DateOnly>
    {
        private readonly string _format = "dd-MM-yyyy";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
