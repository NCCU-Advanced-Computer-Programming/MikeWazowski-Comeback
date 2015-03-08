using System.Collections.Generic;

namespace Orpius.Sokoban
{
	/// <summary>
	/// Represents the token manipulated by the user.
	/// </summary>
	public partial class Actor : CellContents
	{
		readonly Stack<MoveBase> moves = new Stack<MoveBase>();
		int moveCount;
		/* lock object for the DoMove methods. */
		readonly object moveLock = new object();

		public int stepDelay = 100;

		/// <summary>
		/// Gets or sets the step delay for jumps.
		/// </summary>
		/// <value>The step delay in milleseconds imposed
		/// during a jump. The step is executed asynchronously
		/// and the thread is put to sleep for the specified
		/// period.</value>
		public int StepDelay
		{
			get
			{
				return stepDelay;
			}
			set
			{
				stepDelay = value;
				OnPropertyChanged("StepDelay");
			}
		}

		/// <summary>
		/// Gets the move count.
		/// </summary>
		/// <value>The number of moves (or steps)
		/// that the actor has completed.</value>
		public int MoveCount
		{
			get
			{
				return moveCount;
			}
			private set
			{
				moveCount = value;
				if (moveCount < 0)
				{	/* Just in case. */
					moveCount = 0;
				}
				OnPropertyChanged("MoveCount");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Actor"/> class.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="level">The level.</param>
		public Actor(Location location, Level level)
			: base("Actor", location, level)
		{
		}

		/// <summary>
		/// Undoes the last move.
		/// The move may be a single move, or it may be a series
		/// of moves that were taken as part of a <see cref="Jump"/>.
		/// </summary>
		/// <returns></returns>
		public bool UndoMove()
		{
			if (moves.Count < 1)
			{
				return false;
			}
			MoveBase moveBase = moves.Pop();
			Move move = moveBase as Move;
			if (move != null)
			{
				return DoMove(move);
			}
			Jump jump = moveBase as Jump;
			if (jump != null)
			{
				return DoMove(jump);
			}

			Push push = moveBase as Push;
			if (push != null)
			{
				return DoMove(push);
			}
			return false;
		}

	}
}