using System.Text.Json;
using System.Text.Json.Serialization;

namespace tema7
{
  public static class SceneSerializer
  {
    private static JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        Converters = 
        {
            new JsonStringEnumConverter(),
            new ColorJsonConverter() // Add this line
        }
    };

    public static void SaveToFile(Scene scene, string filePath)
    {
      var figures = scene.Figures.ToList();
      string json = JsonSerializer.Serialize(figures, options);
      File.WriteAllText(filePath, json);
    }

    public static Scene LoadFromFile(string filePath)
    {
      string json = File.ReadAllText(filePath);
      var figures = JsonSerializer.Deserialize<List<Figure>>(json, options)
                   ?? throw new InvalidDataException("Invalid file format");

      var scene = new Scene();
      foreach (var figure in figures)
      {
        scene.AddFigure(figure);
      }
      return scene;
    }
  }
}