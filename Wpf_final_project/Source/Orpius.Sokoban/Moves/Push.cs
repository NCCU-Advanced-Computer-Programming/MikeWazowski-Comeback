namespace Orpius.Sokoban
{
	/// <summary>
	/// Indicates the set of steps, or route,
	/// that an <see cref="Actor"/> will take
	/// to relocate to a destination on a <see cref="Level"/>.
	/// This is used in conjuction with <see cref="PushPathFinder"/>
	/// to locate a valid route between the <see cref="Actor"/>
	/// instance and the <see cref="Push.Destination"/>.
	/// </summary>
	class Push : MoveBase
	{
		/// <summary>
		/// Gets or sets the location of the destination
		/// for the push.
		/// </summary>
		/// <value>The destination.</value>
		public Location Destination
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the route from an initial location
		/// to a predefined <see cref="Destination"/>.
		/// </summary>
		/// <value>The route to the predefined <see cref="Destination"/>.
		/// May be null.</value>
		public Move[] Route
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Push"/> class.
		/// </summary>
		/// <param name="destination">The destination of the push. 
		/// <seealso cref="Destination"/></param>
		public Push(Location destination)
		{
			this.Destination = destination;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Push"/> class.
		/// Use to perform an undo.
		/// </summary>
		/// <param name="route">The route that was previously calculated.</param>
		public Push(Move[] route)
		{
			Route = route;
		}
	}
}
