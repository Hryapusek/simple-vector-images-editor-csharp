namespace tema7
{
  public class ChangeColorCommand : IFigureCommand
  {
    private readonly Figure _figure;

    private readonly Color _originalColor;

    private readonly Color _newColor;

    public ChangeColorCommand(Figure figure, Color prevColor, Color newColor)
    {
      _figure = figure;
      _originalColor = prevColor;
      _newColor = newColor;
    }

    public void Execute()
    {
      Console.WriteLine($"Changing color from {_originalColor} to {_newColor} for {_figure.GetType().Name}");
      _figure.StrokeColor = _newColor;
    }

    public void Undo()
    {
      Console.WriteLine($"Restoring original color {_originalColor} for {_figure.GetType().Name}");
      _figure.StrokeColor = _originalColor;
    }
  }
}
