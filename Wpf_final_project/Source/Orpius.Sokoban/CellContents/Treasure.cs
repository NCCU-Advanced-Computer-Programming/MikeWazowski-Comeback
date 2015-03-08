namespace Orpius.Sokoban
{
	/// <summary>
	/// Represents a treasure in a level grid.
	/// </summary>
	public class Treasure : CellContents
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Treasure"/> class.
		/// </summary>
		/// <param name="location">The location where the treasure is.</param>
		/// <param name="level">The level that the treasure is located.</param>
		public Treasure(Location location, Level level)
			: base("Treasure", location, level)
		{
		}
	}
}
