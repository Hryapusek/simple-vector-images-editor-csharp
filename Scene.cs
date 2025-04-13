
namespace tema7
{
  public class Scene
  {
    public List<Figure> Figures { get; } = new List<Figure>();
    public Figure? SelectedFigure { get; set; }
    public event Action? SceneChanged;

    private Point? dragStartPosition;
    private OperationType currentOperation;

    // Handle mouse down for selection
    public void HandleMouseDown(Point mousePos)
    {
      Console.WriteLine($"Testing for selection at {mousePos}");
      SelectedFigure = Figures.LastOrDefault(f => f.Contains(mousePos));
      if (SelectedFigure == null)
      {
        Console.WriteLine("No figure selected");
        SceneChanged?.Invoke();
        return;
      }
      Console.WriteLine($"Selected figure at {mousePos}");
      currentOperation = SelectedFigure.HitTest(mousePos);
      dragStartPosition = mousePos;
      SceneChanged?.Invoke();
    }

    // Handle mouse move for dragging
    public void HandleMouseMove(Point mousePos)
    {
      Console.WriteLine($"Mouse move at {mousePos}");
      if (currentOperation == OperationType.None)
      {
        Console.WriteLine("No operation in progress");
        return;
      }
      if (SelectedFigure != null)
      {
        Console.WriteLine($"Current operation: {currentOperation}");
        if (currentOperation == OperationType.Move)
        {
          var dx = mousePos.X - dragStartPosition!.Value.X;
          var dy = mousePos.Y - dragStartPosition.Value.Y;
          Console.WriteLine($"Moving figure by ({dx}, {dy})");
          SelectedFigure.Move(dx, dy);
        }
        else
        {
          var dx = mousePos.X - dragStartPosition!.Value.X;
          var dy = mousePos.Y - dragStartPosition.Value.Y;
          Console.WriteLine($"Resizing figure by ({dx}, {dy}) using {currentOperation}");
          SelectedFigure.Resize(dx, dy, currentOperation);
        }
        dragStartPosition = mousePos;
        SceneChanged?.Invoke();
      }
    }

    // Handle mouse up for releasing selection
    public void HandleMouseUp(Point mousePos)
    {
    }

    // Add/remove figures
    public void AddFigure(Figure figure)
    {
      Figures.Add(figure);
      figure.FigureChanged += () => SceneChanged?.Invoke();
      SceneChanged?.Invoke();
    }

    public void RemoveFigure(Figure figure)
    {
      Figures.Remove(figure);
      SceneChanged?.Invoke();
    }

    // Draw all figures
    public void Draw(Graphics g)
    {
      Console.WriteLine($"Drawing {Figures.Count} figures");
      foreach (var figure in Figures)
      {
        figure.Draw(g, figure == SelectedFigure);
      }
    }
  }
}