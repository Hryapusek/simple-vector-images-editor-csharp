using System.Drawing;
using System.Drawing.Drawing2D;

namespace tema7
{
  public class Square : Figure
  {
    private int _size = 100; // Default size
    private const int HandleSize = 10; // Selection handle size
    private const int HitTestMargin = 15; // Area around handles for easier selection

    public int Size
    {
      get => _size;
      set
      {
        if (_size != value && value > 0)
        {
          _size = value;
          NotifyChanged();
        }
      }
    }

    public override void Draw(Graphics g, bool isSelected)
    {
      // Draw the square
      using (var pen = new Pen(StrokeColor, StrokeWidth))
      {
        g.DrawRectangle(pen, Position.X, Position.Y, Size, Size);
      }

      // Draw selection handles if selected
      if (isSelected)
      {
        DrawSelectionHandles(g);
      }
    }

    public override bool Contains(Point point)
    {
      var rect = new Rectangle(Position.X, Position.Y, Size, Size);
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
          Position.X + Size - HitTestMargin / 2,
          Position.Y + Size - HitTestMargin / 2,
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

    public override void Resize(int dx, int dy, OperationType resizeMode)
    {
      int delta = Math.Max(Math.Abs(dx), Math.Abs(dy));
      if (dx < 0 || dy < 0) delta = -delta;

      switch (resizeMode)
      {
        case OperationType.ResizeTopLeft:
          Position = new Point(Position.X + delta, Position.Y + delta);
          Size = Math.Max(10, Size - delta);
          break;

        case OperationType.ResizeBottomRight:
          Size = Math.Max(10, Size + delta);
          break;

        case OperationType.ResizeTopRight:
          Position = new Point(Position.X, Position.Y + delta);
          Size = Math.Max(10, Size - delta);
          break;

        case OperationType.ResizeBottomLeft:
          Position = new Point(Position.X + delta, Position.Y);
          Size = Math.Max(10, Size - delta);
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
            Position.X + Size - HandleSize / 2,
            Position.Y + Size - HandleSize / 2,
            HandleSize, HandleSize);
      }
    }
  }
}