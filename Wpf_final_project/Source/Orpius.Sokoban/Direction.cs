namespace Orpius.Sokoban
{
	/// <summary>
	/// Four directions: up, down, left, right.
	/// </summary>
	public enum Direction
	{
		/// <summary>
		/// North
		/// </summary>
		Up,
		/// <summary>
		/// South
		/// </summary>
		Down,
		/// <summary>
		/// East
		/// </summary>
		Left,
		/// <summary>
		/// West
		/// </summary>
		Right
	}

	/// <summary>
	/// Adds some static auxiliary methods
	/// for the <see cref="Direction"/> enum.
	/// </summary>
	public static class MoveDirectionAux
	{
		/// <summary>
		/// Gets the opposite direction for the direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		public static Direction GetOppositeDirection(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Up:
					return Direction.Down;
				case Direction.Down:
					return Direction.Up;
				case Direction.Left:
					return Direction.Right;
				case Direction.Right:
					return Direction.Left;
				default:
					throw new SokobanException("Invalid direction: " + direction.ToString("G"));
			}
		}
	}
}
