using System.Text.Json;
using System.Text.Json.Serialization;

namespace tema7
{
  public class FigureJsonConverter : JsonConverter<Figure>
  {
    public override Figure Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
      using JsonDocument doc = JsonDocument.ParseValue(ref reader);
      var root = doc.RootElement;

      string typeName = root.GetProperty("Type").GetString();
      return typeName switch
      {
        "Square" => JsonSerializer.Deserialize<Square>(root.GetRawText(), options),
        // Add other figure types here
        _ => throw new JsonException($"Unknown figure type: {typeName}")
      };
    }

    public override void Write(
        Utf8JsonWriter writer,
        Figure value,
        JsonSerializerOptions options)
    {
      writer.WriteStartObject();
      writer.WriteString("Type", value.GetType().Name);

      // Serialize all properties
      foreach (var prop in value.GetType().GetProperties())
      {
        if (prop.CanRead)
        {
          writer.WritePropertyName(prop.Name);
          JsonSerializer.Serialize(writer, prop.GetValue(value), options);
        }
      }

      writer.WriteEndObject();
    }
  }
}