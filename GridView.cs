using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace tema7
{
  public class GridView : Control
  {
    // Grid properties
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    // Grid properties
    public bool ShowGrid { get; set; } = true;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int GridSize { get; set; } = 20;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color GridColor { get; set; } = Color.FromArgb(240, 240, 240);
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color MajorGridLineColor { get; set; } = Color.FromArgb(220, 220, 220);
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int MajorGridLineInterval { get; set; } = 5;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool SnapToGrid { get; set; } = true;

    // Background color
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    // Background color
    public Color BackgroundColor { get; set; } = Color.White;

    public Scene scene;

    public GridView()
    {
      this.DoubleBuffered = true; // Prevent flickering
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.AllPaintingInWmPaint |
                   ControlStyles.UserPaint, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      // Clear with background color
      e.Graphics.Clear(BackgroundColor);

      if (ShowGrid)
      {
        DrawGrid(e.Graphics);
      }

      if (scene != null)
      {
        scene.Draw(e.Graphics);
      }
    }

    private void DrawGrid(Graphics g)
    {
      // Draw minor grid lines
      using (var gridPen = new Pen(GridColor))
      {
        // Vertical lines
        for (int x = 0; x < Width; x += GridSize)
        {
          g.DrawLine(gridPen, x, 0, x, Height);
        }

        // Horizontal lines
        for (int y = 0; y < Height; y += GridSize)
        {
          g.DrawLine(gridPen, 0, y, Width, y);
        }
      }

      // Draw major grid lines
      using (var majorGridPen = new Pen(MajorGridLineColor, 1.5f))
      {
        int majorGridSize = GridSize * MajorGridLineInterval;

        // Vertical lines
        for (int x = 0; x < Width; x += majorGridSize)
        {
          g.DrawLine(majorGridPen, x, 0, x, Height);
        }

        // Horizontal lines
        for (int y = 0; y < Height; y += majorGridSize)
        {
          g.DrawLine(majorGridPen, 0, y, Width, y);
        }
      }
    }

    public Point SnapPoint(Point point)
    {
      if (!SnapToGrid) return point;

      return new Point(
          (int)(Math.Round((float)point.X / GridSize) * GridSize),
          (int)(Math.Round((float)point.Y / GridSize) * GridSize));
    }

    public Rectangle SnapRectangle(Rectangle rect)
    {
      if (!SnapToGrid) return rect;

      Point snappedLocation = SnapPoint(rect.Location);
      int width = (int)(Math.Round((float)rect.Width / GridSize) * GridSize);
      int height = (int)(Math.Round((float)rect.Height / GridSize) * GridSize);

      return new Rectangle(snappedLocation, new Size(width, height));
    }
  }
}