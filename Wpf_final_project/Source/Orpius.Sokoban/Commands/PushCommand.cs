using System;

namespace Orpius.Sokoban.Commands
{
	/// <summary>
	/// Performs a push with the <see cref="Level"/>'s <see cref="Actor"/>
	/// instance. A push is a set of steps that may entail
	/// pushing a <see cref="Treasure"/> to a new location.
	/// </summary>
	public class PushCommand : CommandBase
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
		/// Initializes a new instance of the <see cref="PushCommand"/> class.
		/// </summary>
		/// <param name="level">The level.</param>
		/// <param name="destination">The destination to relocate.</param>
		public PushCommand(Level level, Location destination)
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
			Push push = new Push(Destination);
			level.Actor.DoMove(push);
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
