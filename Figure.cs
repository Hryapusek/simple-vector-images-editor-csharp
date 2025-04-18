using System.Text.Json.Serialization;

namespace tema7
{
  public enum OperationType
  {
    None,
    Move,
    ResizeTopLeft,
    ResizeTopRight,
    ResizeBottomLeft,
    ResizeBottomRight
  }

  [JsonPolymorphic]
  [JsonDerivedType(typeof(Square), "Square")]
  [JsonDerivedType(typeof(RegularHexagon), "RegularHexagon")]
  [JsonDerivedType(typeof(RegularPentagon), "RegularPentagon")]
  [JsonDerivedType(typeof(RegularTriangle), "RegularTriangle")]
  public abstract class Figure
  {
    private Point position;
    public Point Position
    {
      get => position;
      set
      {
        position = value;
        NotifyChanged();
      }
    }

    [JsonConverter(typeof(ColorJsonConverter))]
    private Color strokeColor = Color.Black;
    public Color StrokeColor
    {
      get => strokeColor;
      set
      {
        strokeColor = value;
        NotifyChanged();
      }
    }

    private float strokeWidth = 10f;
    public float StrokeWidth
    {
      get => strokeWidth;
      set
      {
        strokeWidth = value;
        NotifyChanged();
      }
    }

    private bool isSelected;
    public bool IsSelected
    {
      get => isSelected;
      set
      {
        isSelected = value;
        NotifyChanged();
      }
    }
    public event Action? FigureChanged;
    public abstract Size Size { get; set; }

    public abstract void Draw(Graphics g, bool isSelected);
    public abstract bool Contains(Point point);
    public abstract OperationType HitTest(Point point);
    public abstract void Move(int dx, int dy);
    public abstract void Resize(int dx, int dy, OperationType resizeMode);
    public abstract Figure Clone();
    public void NotifyChanged() => FigureChanged?.Invoke();
    public abstract void ApplyTransform(Point delta, OperationType operation);
    public abstract TransformationState GetTransformationState();
    public abstract TransformationState GetState();
    public abstract void ApplyState(TransformationState state);
    public abstract Figure DeepClone();
  }

  public struct TransformationState
  {
    public Point Position;
    public Size Size;
  }
}