using System.Drawing;
using System.Drawing.Drawing2D;

namespace tema7
{
  public class Square : Figure
  {
    private int _size = 100; // Default size
    private const int HandleSize = 10; // Selection handle size
    private const int HitTestMargin = 15; // Area around handles for easier selection

    public override Size Size 
{
    get => new Size(_size, _size);
    set {
      _size = Math.Max(10, value.Width); // Ignore height, enforce minimum
      NotifyChanged();
    }
}

    public override void Draw(Graphics g, bool isSelected)
    {
      // Draw the square
      using (var pen = new Pen(StrokeColor, StrokeWidth))
      {
        g.DrawRectangle(pen, Position.X, Position.Y, _size, _size);
      }

      // Draw selection handles if selected
      if (isSelected)
      {
        DrawSelectionHandles(g);
      }
    }

    public override bool Contains(Point point)
    {
      var rect = new Rectangle(Position.X, Position.Y, _size, _size);
      return rect.Contains(point);
    }

    public override OperationType HitTest(Point point)
    {
      // Check resize handles first
      var topLeft = new Rectangle(
          Position.X - HitTestMargin / 2,
          Position.Y - HitTestMargin / 2,
          HitTestMargin, HitTestMargin);

      var bottomRight = new Rectangle(
          Position.X + _size - HitTestMargin / 2,
          Position.Y + _size - HitTestMargin / 2,
          HitTestMargin, HitTestMargin);

      if (topLeft.Contains(point)) return OperationType.ResizeTopLeft;
      if (bottomRight.Contains(point)) return OperationType.ResizeBottomRight;
      if (Contains(point)) return OperationType.Move;

      return OperationType.None;
    }

    public override void Move(int dx, int dy)
    {
      Position = new Point(Position.X + dx, Position.Y + dy);
      NotifyChanged();
    }

    public override void ApplyTransform(Point delta, OperationType operation)
    {
      switch (operation)
      {
        case OperationType.ResizeTopLeft:
          Position = new Point(Position.X + delta.X, Position.Y + delta.X);
          _size = Math.Max(10, _size - delta.X);
          break;

        case OperationType.ResizeBottomRight:
          _size = Math.Max(10, _size + delta.X);
          break;

        case OperationType.ResizeTopRight:
          Position = new Point(Position.X, Position.Y + delta.Y);
          _size = Math.Max(10, _size + delta.X);
          break;

        case OperationType.ResizeBottomLeft:
          Position = new Point(Position.X + delta.X, Position.Y);
          _size = Math.Max(10, _size + delta.Y);
          break;
        
        case OperationType.Move:
          Move(delta.X, delta.Y);
          break;
      }
      NotifyChanged();
    }

    public override TransformationState GetTransformationState()
    {
        return new TransformationState 
        {
            Position = this.Position,
            Size = (this as Figure).Size
        };
    }

    public override void Resize(int dx, int dy, OperationType resizeMode)
    {
      int delta = Math.Max(Math.Abs(dx), Math.Abs(dy));
      if (dx < 0 || dy < 0) delta = -delta;

      switch (resizeMode)
      {
        case OperationType.ResizeTopLeft:
          Position = new Point(Position.X + delta, Position.Y + delta);
          _size = Math.Max(10, _size - delta);
          break;

        case OperationType.ResizeBottomRight:
          _size = Math.Max(10, _size + delta);
          break;

        case OperationType.ResizeTopRight:
          Position = new Point(Position.X, Position.Y + delta);
          _size = Math.Max(10, _size - delta);
          break;

        case OperationType.ResizeBottomLeft:
          Position = new Point(Position.X + delta, Position.Y);
          _size = Math.Max(10, _size - delta);
          break;
      }
      NotifyChanged();
    }

    public override Figure Clone()
    {
      return new Square
      {
        Position = this.Position,
        Size = this.Size,
        StrokeColor = this.StrokeColor,
        StrokeWidth = this.StrokeWidth,
        IsSelected = this.IsSelected
      };
    }

    private void DrawSelectionHandles(Graphics g)
    {
      using (var brush = new SolidBrush(Color.Blue))
      {
        // Top-left handle
        g.FillRectangle(brush,
            Position.X - HandleSize / 2,
            Position.Y - HandleSize / 2,
            HandleSize, HandleSize);

        // Bottom-right handle
        g.FillRectangle(brush,
            Position.X + _size - HandleSize / 2,
            Position.Y + _size - HandleSize / 2,
            HandleSize, HandleSize);
      }
    }
  }
}