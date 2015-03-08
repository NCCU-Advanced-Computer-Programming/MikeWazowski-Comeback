namespace Orpius.Sokoban
{
	/// <summary>
	/// Represents a wall in the <see cref="Level"/> grid.
	/// </summary>
	public class WallCell : Cell
	{
		/// <summary>
		/// Gets a value indicating whether the <see cref="Actor"/> 
		/// or other <see cref="CellContents"/>
		/// instance can enter this cell.
		/// </summary>
		/// <value><c>false</c></value>
		public override bool CanEnter
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WallCell"/> class.
		/// </summary>
		/// <param name="location">The location where this wall is located.</param>
		/// <param name="level">The level where this cell is located.</param>
		public WallCell(Location location, Level level)
			: base("Wall", location, level)
		{
		}
	}
}
