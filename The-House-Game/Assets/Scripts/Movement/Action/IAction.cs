using Units.Settings;

public interface IAction
{
	bool IsDone { get; }
	public Cell from { get; set; }
	public Cell to { get; set; }
	public Unit unit { get; set; }

	void Execute();
}
