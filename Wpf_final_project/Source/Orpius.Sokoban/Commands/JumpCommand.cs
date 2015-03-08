using System;

namespace Orpius.Sokoban.Commands
{
	/// <summary>
	/// Performs a jump with the <see cref="Level"/>'s <see cref="Actor"/>
	/// instance. A jump is a series of <see cref="Move"/>s.
	/// </summary>
	public class JumpCommand : CommandBase
	{
		readonly Level level;

		/// <summary>
		/// Gets or sets the location of the intended 
		/// destination on the <see cref="Level"/>.
		/// </summary>
		/// <value>The location on the <see cref="Level"/>.</value>
		public Location Destination
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JumpCommand"/> class.
		/// </summary>
		/// <param name="level">The level where this jump is to occur.</param>
		/// <param name="destination">The location of the intended destination. 
		/// I.e. where we are jumping.</param>
		public JumpCommand(Level level, Location destination)
		{
			if (level == null)
			{
				throw new ArgumentNullException("level");
			}
			Destination = destination;
			this.level = level;
		}

		/// <summary>
		/// Moves the <see cref="Actor"/> instance
		/// to the <see cref="E:Destination"/>.
		/// </summary>
		public override void Execute()
		{
			Location manLocation = level.Actor.Location;
			if (manLocation.IsAdjacentLocation(Destination))
			{/* This is adjacent, hence we do not need to jump.
			  * Instead we just move. */
				Direction direction = manLocation.GetDirection(Destination);
				Move move = new Move(direction);
				level.Actor.DoMove(move);
			}
			else
			{
				Jump jump = new Jump(Destination);
				level.Actor.DoMove(jump);
			}
		}

		/// <summary>
		/// Undoes that which was performed with <see cref="E:Execute"/>.
		/// Returns the <see cref="Actor"/> instance to the initial
		/// location before the command was executed.
		/// </summary>
		public override void Undo()
		{
			level.Actor.UndoMove();
		}

		/// <summary>
		/// Redoes this command after it has been undone. <see cref="E:Undo"/>.
		/// </summary>
		public override void Redo()
		{
			Execute();
		}
	}
}