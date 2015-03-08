using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Orpius.Sokoban.Commands;
using Orpius.Sokoban.Controls;

namespace Orpius.Sokoban
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		readonly CommandManager commandManager = new CommandManager();

		/// <summary>
		/// Gets the game that is defined as a window resource 
		/// in the XAML as <Sokoban:Game x:Key="sokobanGame"/>.
		/// </summary>
		/// <value>The game intance.</value>
		Game Game
		{
			get
			{
				return (Game)TryFindResource("sokobanGame");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the press any key
		/// message is displayed.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the press any key message is displayed; 
		/// otherwise, <c>false</c>.
		/// </value>
		bool ContinuePromptVisible
		{
			get
			{
				return Border_PressAnyKey.Visibility == Visibility.Visible;
			}
			set
			{
				Border_PressAnyKey.Visibility = value ? Visibility.Visible : Visibility.Hidden;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Game.PropertyChanged += game_PropertyChanged;

			try
			{
				/* Load and start the first level of the game. */
				Game.Start();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Problem loading game. " + ex.Message);
			}
		}

		void game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "GameState":
					UpdateGameDisplay();
					break;
			}
		}

		/// <summary>
		/// We set feedback messages and so one here,
		/// using the game's <see cref="GameState"/>.
		/// </summary>
		void UpdateGameDisplay()
		{
			switch (Game.GameState)
			{
				case GameState.Loading:
					FeedbackControl1.Message = new FeedbackMessage { Message = "Loading..." };
					ContinuePromptVisible = false;
					break;
				case GameState.GameOver:
					FeedbackControl1.Message = new FeedbackMessage { Message = "Game Over" };
					ContinuePromptVisible = true;
					break;
				case GameState.Running:
					ContinuePromptVisible = false;
					FeedbackControl1.Message = new FeedbackMessage();
					// Uncomment when/if pause is implemented.
					//if (gameState == GameState.Loading)
					//{
						InitialiseLevel();
					//}
					break;
				case GameState.LevelCompleted:
					FeedbackControl1.Message = new FeedbackMessage { Message = "Level Completed!" };
					MediaElement_LevelComplete.Position = TimeSpan.MinValue;
					MediaElement_LevelComplete.Play();
					ContinuePromptVisible = true;
					break;
				case GameState.GameCompleted:
					FeedbackControl1.Message = new FeedbackMessage { Message = "Well done. \nGame completed! \nEmail dbvaughan \nAT g mail dot com" };
					MediaElement_GameComplete.Position = TimeSpan.MinValue;
					MediaElement_GameComplete.Play();
					break;
			}
		}

		/// <summary>
		/// Handles the KeyDown event of the Window control.
		/// Put this into another class later.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> 
		/// instance containing the event data.</param>
		void Window_KeyDown(object sender, KeyEventArgs e)
		{
			CommandBase command = null;
			Level level = Game.Level;

			if (Game != null)
			{
				if (Game.GameState == GameState.Running)
				{
					switch (e.Key)
					{
						case Key.Up:
							command = new MoveCommand(level, Direction.Up);
							break;
						case Key.Down:
							command = new MoveCommand(level, Direction.Down);
							break;
						case Key.Left:
							command = new MoveCommand(level, Direction.Left);
							break;
						case Key.Right:
							command = new MoveCommand(level, Direction.Right);
							break;
						case Key.Z:
							if (Keyboard.Modifiers == ModifierKeys.Control)
							{
								commandManager.Undo();
							}
							break;
						case Key.Y:
							if (Keyboard.Modifiers == ModifierKeys.Control)
							{
								commandManager.Redo();
							}
							break;
					}
				}
				else
				{
					switch (Game.GameState)
					{
						case GameState.GameOver:
							Game.Start();
							break;
						case GameState.LevelCompleted:
							Game.GotoNextLevel();
							break;
					}
				}
			}
			if (command != null)
			{
				commandManager.Execute(command);
			}
		}

		/// <summary>
		/// Initialises the game grid using the level.
		/// </summary>
		void InitialiseLevel()
		{
			commandManager.Clear();

			grid_Game.Children.Clear();
			grid_Game.RowDefinitions.Clear();
			grid_Game.ColumnDefinitions.Clear();

			for (int i = 0; i < Game.Level.RowCount; i++)
			{
				grid_Game.RowDefinitions.Add(new RowDefinition());
			}

			for (int i = 0; i < Game.Level.ColumnCount; i++)
			{
				grid_Game.ColumnDefinitions.Add(new ColumnDefinition());
			}

			for (int row = 0; row < Game.Level.RowCount; row++)
			{
				for (int column = 0; column < Game.Level.ColumnCount; column++)
				{
					Cell cell = Game.Level[row, column];
					cell.PropertyChanged += cell_PropertyChanged;
					
					Button button = new Button();
					button.Focusable = false;
					/* The cell becomes the datacontext of the button,
					 * and it gets used a lot in the XAML. */
					button.DataContext = cell; 
					button.Padding = new Thickness(0, 0, 0, 0);
					button.Style = (Style)Resources["Cell"];
					button.Click += Cell_Click;
					
					Grid.SetColumn(button, column);
					Grid.SetRow(button, row);
					grid_Game.Children.Add(button);
				}
			}

			textBox_LevelCode.Text = Game.LevelCode;
			label_LevelNumber.Content = Game.Level.LevelNumber + 1 + "/" + Game.LevelCount;
			grid_Main.DataContext = Game.Level;
			/* Rewing the intro clip and play it. Is there a better way? */
			mediaElement_Intro.Position = TimeSpan.MinValue;
			mediaElement_Intro.Play();
			grid_Game.Focus();
		}

		void Cell_Click(object sender, RoutedEventArgs e)
		{
			/* When the user clicks a cell, we want to 
			 have the actor move there. */
			Button button = (Button)e.Source;
			Cell cell = (Cell)button.DataContext;
			CommandBase command;

			if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
			{
				command = new PushCommand(Game.Level, cell.Location);
			}
			else
			{
				command = new JumpCommand(Game.Level, cell.Location);
			}
			commandManager.Execute(command);
		}

		void cell_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Cell cell = (Cell)sender;
			if (e.PropertyName == "CellContents")
			{
				if (cell.CellContents is Actor)
				{
					mediaElement_Footstep.Position = TimeSpan.MinValue;
					mediaElement_Footstep.Play();
				}
				else if (cell.CellContents is Treasure)
				{
					if (cell is GoalCell)
					{
						mediaElement_DingDong.Position = TimeSpan.MinValue;
						mediaElement_DingDong.Play();
					}
					else
					{
						mediaElement_TreasurePush.Position = TimeSpan.MinValue;
						mediaElement_TreasurePush.Play();
					}
				}
			}
		}

		void textBox_LevelCode_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (Game.TryGotoToLevel(textBox_LevelCode.Text.Trim()))
				{
					/* Do some extra level change processing here. */
				}
				UIElement focusedElement = Keyboard.FocusedElement as UIElement;
				if (focusedElement != null)
				{
					focusedElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
				}
				e.Handled = true;
			}
		}

		void button_RestartLevel_Click(object sender, RoutedEventArgs e)
		{
			Game.RestartLevel();
		}

		void textBox_LevelCode_GotFocus(object sender, RoutedEventArgs e)
		{
			textBox_LevelCode.Text = string.Empty;
		}

		void textBox_LevelCode_LostFocus(object sender, RoutedEventArgs e)
		{
			textBox_LevelCode.Text = Game.LevelCode;
		}

		void FeedbackControl1_Click(object sender, FeedbackEventArgs e)
		{
			switch (Game.GameState)
			{
				case GameState.LevelCompleted:
					Game.GotoNextLevel();
					break;
			}
		}

        private void button_nextlevel_click(object sender, RoutedEventArgs e)
        {
            Game.GotoNextLevel();
        }

        private void button_rule_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("想辦法將所有藍色小碎片推到圈圈中，幫助大眼仔脫逃。" + "\n 貼心小提醒：" + "\n 按Ctrl+Z可以回到上一步。" + "\n 按Ctrl+Y可以移到下一步。");
        }

        


	}
}
