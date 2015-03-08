namespace Orpius.Sokoban
{
	/// <summary>
	/// Represents a cell containing nothing. 
	/// This is not a cell that is used to place <see cref="CellContents"/>.
	/// </summary>
	public class SpaceCell : Cell
	{
		public SpaceCell(Location location, Level level)
			: base("Space", location, level)
		{
		}
	}
}
