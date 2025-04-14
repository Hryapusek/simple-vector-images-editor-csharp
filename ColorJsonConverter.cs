using System.Text.Json;
using System.Text.Json.Serialization;

namespace tema7
{
  public class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            return Color.FromArgb(
                root.GetProperty("a").GetByte(),
                root.GetProperty("r").GetByte(),
                root.GetProperty("g").GetByte(),
                root.GetProperty("b").GetByte());
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        Color value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("a", value.A);
        writer.WriteNumber("r", value.R);
        writer.WriteNumber("g", value.G);
        writer.WriteNumber("b", value.B);
        writer.WriteEndObject();
    }
}
}