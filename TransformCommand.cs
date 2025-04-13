namespace tema7
{
  public interface IFigureCommand
  {
    void Execute();
    void Undo();
  }

  public class TransformCommand : IFigureCommand
  {
    private readonly Figure _figure;
    private readonly TransformationState _originalState;
    private readonly TransformationState _newState;

    public TransformCommand(Figure figure, TransformationState originalState)
    {
      _figure = figure;
      _originalState = originalState;
      _newState = figure.GetState();
    }

    public void Execute()
    {
      Console.WriteLine($"Applying transformation state {_newState} to {_figure.GetType().Name}");
      _figure.ApplyState(_newState);
    }

    public void Undo()
    {
      Console.WriteLine($"Restoring original state {_originalState} to {_figure.GetType().Name}");
      _figure.ApplyState(_originalState);
    }
  }
}
