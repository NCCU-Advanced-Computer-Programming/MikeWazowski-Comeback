namespace Orpius.Sokoban
{
	/// <summary>
	/// Describes a single step that an <see cref="Actor"/> will take
	/// to relocate to a destination on a <see cref="Level"/>.
	/// </summary>
	class Move : MoveBase
	{
		/// <summary>
		/// Gets the direction in which to relocate.
		/// </summary>
		/// <value>The direction to relocate.</value>
		public Direction Direction
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the contents that were moved
		/// as part of the relocation. This is used for undo's.
		/// </summary>
		/// <value>The contents that were pushed 
		/// as part of the relocation.</value>
		public CellContents PushedContents
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Move"/> class.
		/// </summary>
		/// <param name="direction">The direction in an <see cref="Actor"/> 
		/// should relocate. <seealso cref="Direction"/></param>
		public Move(Direction direction)
		{
			Direction = direction;
		}
	}
}
