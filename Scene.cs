
namespace tema7
{
  public class TransformationInfo
  {
    public Figure TargetFigure { get; }
    public TransformationState OriginalState { get; }
    public Point StartMousePosition { get; }
    private Point LastMousePosition { get; set; }
    public OperationType Operation { get; }

    public TransformationInfo(Figure figure, Point mousePos, OperationType operation)
    {
      TargetFigure = figure;
      OriginalState = figure.GetTransformationState();
      StartMousePosition = mousePos;
      Operation = operation;
      LastMousePosition = mousePos;
    }

    public void ApplyDelta(Point currentMousePos)
    {
      Point delta = new Point(
          currentMousePos.X - LastMousePosition.X,
          currentMousePos.Y - LastMousePosition.Y);

      TargetFigure.ApplyTransform(delta, Operation);
      LastMousePosition = currentMousePos;
    }

    public void RestoreOriginal()
    {
      TargetFigure.Position = OriginalState.Position;
      TargetFigure.Size = OriginalState.Size;
    }
  }
  public class Scene
  {
    public List<Figure> Figures { get; } = new List<Figure>();
    public Figure? SelectedFigure { get; set; }
    public event Action? SceneChanged;

    private Point? dragStartPosition;
    private TransformationInfo? currentTransform;
    private readonly Stack<IFigureCommand> _undoStack = new();
    private readonly Stack<IFigureCommand> _redoStack = new();

    public void ApplyCommand(IFigureCommand command)
    {
      command.Execute();
      _undoStack.Push(command);
      _redoStack.Clear();
      SceneChanged?.Invoke();
    }

    public void Undo()
    {
      if (_undoStack.Count > 0)
      {
        Console.WriteLine("Undoing");
        var cmd = _undoStack.Pop();
        cmd.Undo();
        _redoStack.Push(cmd);
        SceneChanged?.Invoke();
      }
    }

    public void Redo()
    {
      if (_redoStack.Count > 0)
      {
        var cmd = _redoStack.Pop();
        cmd.Execute();
        _undoStack.Push(cmd);
        SceneChanged?.Invoke();
      }
    }

    // Handle mouse down for selection
    public void HandleMouseDown(Point mousePos)
    {
      SelectedFigure = Figures.LastOrDefault(f => f.Contains(mousePos));
      if (SelectedFigure == null)
      {
        Console.WriteLine("No figure selected");
        SceneChanged?.Invoke();
        return;
      }
      Console.WriteLine($"Selected figure at {mousePos}");
      var currentOperation = SelectedFigure.HitTest(mousePos);
      if (currentOperation == OperationType.None)
      {
        Console.WriteLine("No operation selected");
        SceneChanged?.Invoke();
        return;
      }
      else
      {
        currentTransform = new TransformationInfo(SelectedFigure, mousePos, currentOperation);
      }
      dragStartPosition = mousePos;
      SceneChanged?.Invoke();
    }

    // Handle mouse move for dragging
    public void HandleMouseMove(Point mousePos)
    {
      if (currentTransform == null || SelectedFigure == null)
      {
        Console.WriteLine("No transformation in progress or no figure selected");
        return;
      }
      currentTransform.ApplyDelta(mousePos);
      SceneChanged?.Invoke();
    }

    // Handle mouse up for releasing selection
    public void HandleMouseUp(Point mousePos)
    {
      if (currentTransform == null || SelectedFigure == null)
      {
        Console.WriteLine("No transformation in progress or no figure selected");
        return;
      }
      Console.WriteLine($"Saving original state of {SelectedFigure.GetType().Name} at {currentTransform.OriginalState.Position}");
      _undoStack.Push(new TransformCommand(SelectedFigure, currentTransform.OriginalState));
      _redoStack.Clear();
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
      foreach (var figure in Figures)
      {
        figure.Draw(g, figure == SelectedFigure);
      }
    }
  }
}