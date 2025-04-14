using System.Drawing.Drawing2D;
using System.Text.Json.Serialization;

namespace tema7
{
  [JsonDerivedType(typeof(RegularTriangle), typeDiscriminator: "triangle")]
  public class RegularTriangle : Figure
  {
    private int _size = 100; // Length of sides
    private const float AngleIncrement = (float)(2 * Math.PI / 3); // 120 degrees in radians
    private const int SelectionHandleSize = 8;

    [JsonInclude]
    public override Size Size
    {
      get => new Size(_size, (int)(_size * Math.Sqrt(3) / 2)); // Height = (√3/2)*side
      set => _size = Math.Max(10, Math.Max(value.Width, (int)(value.Height * 2 / Math.Sqrt(3))));
    }

    public override void Draw(Graphics g, bool isSelected)
    {
      PointF[] points = GetTrianglePoints();

      using (var pen = new Pen(StrokeColor, StrokeWidth))
      {
        g.DrawPolygon(pen, points);
      }

      if (isSelected)
      {
        DrawSelectionHandles(g, points);
      }
    }

    public override bool Contains(Point point)
    {
      using (var path = new GraphicsPath())
      {
        path.AddPolygon(GetTrianglePoints());
        using (var pen = new Pen(Color.Empty, StrokeWidth + 5))
        {
          return path.IsOutlineVisible(point, pen) || path.IsVisible(point);
        }
      }
    }

    public override OperationType HitTest(Point point)
    {
      PointF[] points = GetTrianglePoints();
      var hitTestMargin = SelectionHandleSize * 2;

      // Check each vertex handle
      for (int i = 0; i < 3; i++)
      {
        var handleRect = new Rectangle(
            (int)points[i].X - hitTestMargin / 2,
            (int)points[i].Y - hitTestMargin / 2,
            hitTestMargin, hitTestMargin);

        if (handleRect.Contains(point))
        {
          return OperationType.ResizeTopLeft + i;
        }
      }

      // Check for move operation
      if (Contains(point))
      {
        return OperationType.Move;
      }

      return OperationType.None;
    }

    public override void ApplyTransform(Point delta, OperationType operation)
    {
      if (operation == OperationType.Move)
      {
        Position = new Point(Position.X + delta.X, Position.Y + delta.Y);
      }
      else
      {
        // Uniform scaling for equilateral triangle
        int sizeChange = (Math.Abs(delta.X) + Math.Abs(delta.Y)) / 2;
        _size += sizeChange;
      }
      NotifyChanged();
    }

    private PointF[] GetTrianglePoints()
    {
      PointF[] points = new PointF[3];
      float centerX = Position.X + _size / 2;
      float centerY = Position.Y + (float)(_size * Math.Sqrt(3) / 3);

      for (int i = 0; i < 3; i++)
      {
        float angle = (float)(Math.PI / 2) + i * AngleIncrement;
        points[i] = new PointF(
            centerX + (_size / 2) * (float)Math.Cos(angle),
            centerY - (_size / 2) * (float)Math.Sin(angle));
      }

      return points;
    }

    private void DrawSelectionHandles(Graphics g, PointF[] points)
    {
      using (var brush = new SolidBrush(Color.Blue))
      {
        foreach (var point in points)
        {
          g.FillRectangle(brush,
              point.X - SelectionHandleSize / 2,
              point.Y - SelectionHandleSize / 2,
              SelectionHandleSize,
              SelectionHandleSize);
        }
      }
    }

    // ... Standard overrides (same pattern as other shapes) ...
    public override Figure Clone() => new RegularTriangle
    {
      Position = this.Position,
      _size = this._size,
      StrokeColor = this.StrokeColor,
      StrokeWidth = this.StrokeWidth,
      IsSelected = this.IsSelected
    };

    public override TransformationState GetTransformationState() => new()
    {
      Position = this.Position,
      Size = this.Size
    };

    public override TransformationState GetState() => GetTransformationState();

    public override void ApplyState(TransformationState state)
    {
      Position = state.Position;
      Size = state.Size;
    }

    public override Figure DeepClone() => Clone();

    public override void Move(int dx, int dy)
    {
      Position = new Point(Position.X + dx, Position.Y + dy);
      NotifyChanged();
    }

    public override void Resize(int dx, int dy, OperationType resizeMode)
    {
      // Uniform scaling for equilateral triangle
      int delta = (Math.Abs(dx) + Math.Abs(dy)) / 2;
      _size += delta;
      NotifyChanged();
    }
  }
}