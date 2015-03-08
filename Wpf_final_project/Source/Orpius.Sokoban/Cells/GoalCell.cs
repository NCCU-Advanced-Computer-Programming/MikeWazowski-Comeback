using System;

namespace Orpius.Sokoban
{
	/// <summary>
	/// Represents a goal on the level grid
	/// where <see cref="Treasure"/>s must be pushed
	/// in order to complete the <see cref="Level"/>.
	/// </summary>
	public class GoalCell : Cell
	{
		const string cellName = "Goal";

		/// <summary>
		/// Gets a value indicating whether this instance has a treasure in it.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has a treasure; otherwise, <c>false</c>.
		/// </value>
		public bool HasTreasure
		{
			get
			{
				return CellContents is Treasure;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GoalCell"/> class.
		/// </summary>
		/// <param name="location">The location of the goal
		/// on the level grid.</param>
		/// <param name="level">The level grid where this cell
		/// is located.</param>
		public GoalCell(Location location, Level level)
			: base(cellName, location, level)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GoalCell"/> class.
		/// </summary>
		/// <param name="location">The location of the goal
		/// on the level grid.</param>
		/// <param name="level">The level grid where this cell
		/// is located.</param>
		/// <param name="contents">The contents of this goal cell.</param>
		public GoalCell(Location location, Level level, CellContents contents)
			: base(cellName, location, level, contents)
		{
		}

		/// <summary>
		/// Tries the set the contents of this goal.
		/// </summary>
		/// <param name="contents">The contents to set.</param>
		/// <returns><code>true</code> if the contents 
		/// was successfully placed on the goal, <code>false</code> otherwise.
		/// </returns>
		public override bool TrySetContents(CellContents contents)
		{
			if (base.TrySetContents(contents))
			{
				if (contents is Treasure)
				{
					OnCompletedGoalChanged(EventArgs.Empty);
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the <see cref="CellContents"/> contents from this goal.
		/// </summary>
		public override void RemoveContents()
		{
			/* Check for the removal of a treasure from goal square. */
			if (CellContents != null && CellContents is Treasure)
			{
				OnCompletedGoalChanged(EventArgs.Empty);
			}
			base.RemoveContents();
		}

		#region CompletedGoalChanged event
		event EventHandler completedGoalChanged;

		/// <summary>
		/// Occurs when a <see cref="Treasure"/> instance
		/// is either removed from or placed in this goal.
		/// </summary>
		public event EventHandler CompletedGoalChanged
		{
			add
			{
				completedGoalChanged += value;
			}
			remove
			{
				completedGoalChanged -= value;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:CompletedGoalChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void OnCompletedGoalChanged(EventArgs e)
		{
			if (completedGoalChanged != null)
			{
				completedGoalChanged(this, e);
			}
		}
		#endregion
	}
}
