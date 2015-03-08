namespace Orpius.Sokoban
{
	/// <summary>
	/// Represents a point on a <see cref="Level"/> grid.
	/// </summary>
	public class Location
	{
		/// <summary>
		/// Gets the row number.
		/// </summary>
		/// <value>The row number.</value>
		public int RowNumber
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the column number.
		/// </summary>
		/// <value>The column number.</value>
		public int ColumnNumber
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Location"/> class.
		/// </summary>
		/// <param name="rowNumber">The row number.</param>
		/// <param name="columnNumber">The column number.</param>
		public Location(int rowNumber, int columnNumber)
		{
			RowNumber = rowNumber;
			ColumnNumber = columnNumber;
		}

		/// <summary>
		/// Gets the location of an adjacent location 
		/// that is in the specified direction.
		/// </summary>
		/// <param name="direction">The direction of the adjacent location.</param>
		/// <returns></returns>
		public Location GetAdjacentLocation(Direction direction)
		{
			switch (direction)
			{
				case Direction.Up:
					return new Location(RowNumber - 1, ColumnNumber);
				case Direction.Down:
					return new Location(RowNumber + 1, ColumnNumber);
				case Direction.Left:
					return new Location(RowNumber, ColumnNumber - 1);
				case Direction.Right:
					return new Location(RowNumber, ColumnNumber + 1);
				default:
					throw new SokobanException("Unkown direction. " + direction.ToString("G"));
			}
		}

		/// <summary>
		/// Determines whether the specified location is adjacent to this instance.
		/// That is, whether it is located one point to the left, right, above, or below.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>
		/// 	<c>true</c> if the specified location is adjacent; otherwise, <c>false</c>.
		/// </returns>
		public bool IsAdjacentLocation(Location location)
		{
			return !location.Equals(this)
				   && ((ColumnNumber == location.ColumnNumber
						&& RowNumber <= location.RowNumber + 1
						&& RowNumber >= location.RowNumber - 1)
					   ||
					   (RowNumber == location.RowNumber
						&& ColumnNumber <= location.ColumnNumber + 1
						&& ColumnNumber >= location.ColumnNumber - 1)
					  );
		}

		/// <summary>
		/// Gets the direction of an adjancent location 
		/// relative to the current instance.
		/// </summary>
		/// <param name="location">The location of an adjacent location.</param>
		/// <returns></returns>
		/// <exception cref="SokobanException">If the location is
		/// not adjacent to the current instance.</exception>
		public Direction GetDirection(Location location)
		{
			if (!IsAdjacentLocation(location))
			{
				throw new SokobanException("location is not adjacent.");
			}

			if (location.ColumnNumber > ColumnNumber)
			{
				return Direction.Right;
			}
			else if (location.ColumnNumber < ColumnNumber)
			{
				return Direction.Left;
			}
			else if (location.RowNumber > RowNumber)
			{
				return Direction.Down;
			}

			return Direction.Up;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> 
		/// is equal to the current <see cref="T:Location"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> 
		/// to compare with the current <see cref="T:Location"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> 
		/// is equal to the current <see cref="T:Location"/>; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Location loc = obj as Location;
			if (loc == null)
			{
				return false;
			}
			return loc.RowNumber == RowNumber && loc.ColumnNumber == ColumnNumber;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:Location"/>.
		/// The XOR of the <see cref="RowNumber"/> and <see cref="ColumnNumber"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return RowNumber ^ ColumnNumber;
		}

		/// <summary>
		/// Returns a string representation of the location.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			return string.Format("Column {0}, Row {1}", ColumnNumber, RowNumber);
		}
	}
}
