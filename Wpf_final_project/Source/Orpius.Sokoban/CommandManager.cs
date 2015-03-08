using System.Collections.Generic;

namespace Orpius.Sokoban
{
	/// <summary>
	/// Provides for execution, undoing, and redoing
	/// of <see cref="CommandBase"/> instances.
	/// </summary>
	public class CommandManager
	{
		Stack<CommandBase> commandStack = new Stack<CommandBase>();
		Stack<CommandBase> redoStack = new Stack<CommandBase>();

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandManager"/> class.
		/// </summary>
		public CommandManager()
		{
		}

		/// <summary>
		/// Executes the specified command.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		public void Execute(CommandBase command)
		{
			redoStack.Clear();
			command.Execute();
			commandStack.Push(command);
		}

		/// <summary>
		/// Undoes the execution of a previous <see cref="CommandBase"/>.
		/// </summary>
		public void Undo()
		{
			if (commandStack.Count < 1)
			{
				return;
			}
			CommandBase command = commandStack.Pop();
			command.Undo();
			redoStack.Push(command);
		}

		/// <summary>
		/// Reperforms the execution of a <see cref="CommandBase"/>
		/// instance that has been undone, then places it back
		/// into the command stack.
		/// </summary>
		public void Redo()
		{
			if (redoStack.Count < 1)
			{
				return;
			}
			CommandBase command = redoStack.Pop();
			command.Execute();
			commandStack.Push(command);
		}

		/// <summary>
		/// Clears the undo and redo stacks.
		/// </summary>
		public void Clear()
		{
			commandStack.Clear();
			redoStack.Clear();
		}
	}
}
