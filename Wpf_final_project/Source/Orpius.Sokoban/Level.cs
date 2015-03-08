using System;
using System.Collections.Generic;
using System.IO;

namespace Orpius.Sokoban
{
	/// <summary>
	/// Represents a single stage within a game.
	/// A level instance is able to load itself
	/// from a map resource.
	/// </summary>
	public class Level
	{
		Cell[][] cells;
		List<GoalCell> goals = new List<GoalCell>();

		/// <summary>
		/// Gets the level number.
		/// </summary>
		/// <value>The level number.</value>
		public int LevelNumber
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the game that this level is located.
		/// </summary>
		/// <value>The game that this level is located.</value>
		protected Game Game
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the single <see cref="Actor"/> instance
		/// located on each level.
		/// </summary>
		/// <value>The actor. (The user moveable guy) </value>
		public Actor Actor
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the <see cref="Orpius.Sokoban.Cell"/> with the specified row number
		/// and column number.
		/// </summary>
		/// <value>The cell at the specified row number and column number.</value>
		public Cell this[int rowNumber, int columnNumber]
		{
			get
			{
				return cells[rowNumber][columnNumber];
			}
		}

		/// <summary>
		/// Gets the <see cref="Orpius.Sokoban.Cell"/> with the specified location.
		/// </summary>
		/// <value>The cell at the specified location.</value>
		public Cell this[Location location]
		{
			get
			{
				if (InBounds(location))
				{
					return this[location.RowNumber, location.ColumnNumber];
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the number of rows in the level.
		/// </summary>
		/// <value>The number of rows in the level.</value>
		public int RowCount
		{
			get
			{
				return cells != null ? cells.Length : 0;
			}
		}

		/// <summary>
		/// Gets the number of columns in the level.
		/// </summary>
		/// <value>The number of columns in the level.</value>
		public int ColumnCount
		{
			get
			{
				return cells != null && cells.Length > 0 ? cells[0].Length : 0;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Level"/> class.
		/// </summary>
		/// <param name="game">The game that the level belongs.</param>
		/// <param name="levelNumber">The level number for this level.</param>
		public Level(Game game, int levelNumber)
		{
			Game = game;
			LevelNumber = levelNumber;
		}

		/// <summary>
		/// Loads the level data from the specified map stream.
		/// </summary>
		/// <param name="mapStream">The map stream to load the level.</param>
		public void Load(TextReader mapStream)
		{
			if (mapStream == null)
			{
				throw new ArgumentNullException("mapStream");
			}

			List<List<Cell>> rows = new List<List<Cell>>();

			string gridRowText;
			int rowCount = 0;
			while ((gridRowText = mapStream.ReadLine()) != null && gridRowText.Trim() != string.Empty)
			{
				rows.Add(BuildCells(gridRowText, rowCount++));
			}

			cells = new Cell[rowCount][];
			for (int i = 0; i < rowCount; i++)
			{
				cells[i] = rows[i].ToArray();
			}
		}

		List<Cell> BuildCells(string rowText, int rowNumber)
		{
			List<Cell> row = new List<Cell>(rowText.Length);

			int columnNumber = 0;

			foreach (char c in rowText)
			{
				Location location = new Location(rowNumber, columnNumber++);
				switch (c)
				{
					case '#': /* Wall. */
						row.Add(new WallCell(location, this));
						break;
					case ' ': /* Empty. */
						row.Add(new FloorCell(location, this));
						break;
					case '$': /* Treasure in a square. */
						row.Add(new FloorCell(location, this, new Treasure(location, this)));
						break;
					case '*': /* Treasure in a Goal. */
						GoalCell goalCellWithTreasure = new GoalCell(location, this, new Treasure(location, this));
						goalCellWithTreasure.CompletedGoalChanged += new EventHandler(GoalCell_CompletedGoalChanged);
						goals.Add(goalCellWithTreasure);
						row.Add(goalCellWithTreasure);
						break;
					case '.': /* Goal. */
						GoalCell goalCell = new GoalCell(location, this);
						goalCell.CompletedGoalChanged += new EventHandler(GoalCell_CompletedGoalChanged);
						goals.Add(goalCell);
						row.Add(goalCell);
						break;
					case '@': /* Actor in a floor cell. */
						Actor actor = new Actor(location, this);
						row.Add(new FloorCell(location, this, actor));
						Actor = actor;
						break;
					case '!': /* Space. */
						row.Add(new SpaceCell(location, this));
						break;
					default:
						throw new FormatException("Invalid Level symbol found: " + c);
				}
			}
			return row;
		}

		void GoalCell_CompletedGoalChanged(object sender, EventArgs e)
		{
			foreach (GoalCell goal in goals)
			{
				if (!goal.HasTreasure)
				{
					return;
				}
			}
			OnLevelCompleted(EventArgs.Empty);
		}

		/// <summary>
		/// Tests whether the specified location is within 
		/// the Level grid.
		/// </summary>
		/// <param name="location">The location to test
		/// whether it is within the level grid.</param>
		/// <returns><code>true</code> if the location
		/// is within the <see cref="Level"/>; 
		/// <code>false</code> otherwise.</returns>
		public bool InBounds(Location location)
		{
			return (location.RowNumber >= 0
				&& location.RowNumber < RowCount
				&& location.ColumnNumber >= 0
				&& location.ColumnNumber < ColumnCount);
		}
		#region LevelCompleted event

		event EventHandler levelCompleted;

		/// <summary>
		/// Occurs when a level has been completed successfully.
		/// </summary>
		public event EventHandler LevelCompleted
		{
			add
			{
				levelCompleted += value;
			}
			remove
			{
				levelCompleted -= value;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:LevelCompleted"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void OnLevelCompleted(EventArgs e)
		{
			if (levelCompleted != null)
			{
				levelCompleted(this, e);
			}
		}
		#endregion
	}
}
