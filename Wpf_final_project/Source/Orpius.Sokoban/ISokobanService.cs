namespace Orpius.Sokoban
{
	/// <summary>
	/// A Sokoban service provides a service for the game
	/// of sokoban, including retrieval maps, and level counting.
	/// </summary>
	public interface ISokobanService
	{
		/// <summary>
		/// Gets the number of levels in the game.
		/// </summary>
		/// <value>The number of levels in the game.</value>
		int LevelCount
		{
			get;
		}

		/// <summary>
		/// Gets the string representing the map 
		/// with the specified level number.
		/// </summary>
		/// <param name="levelNumber">The level number of the map to retrieve.</param>
		/// <returns>The map in string format.
		/// <example>!!!!!!!!!!!########
		///!!!!!!!!!!!#  ....#
		///############  ....#
		///#    #  $ $   ....#
		///# $$$#$  $ #  ....#
		///#  $     $ #  ....#
		///# $$ #$ $ $########
		///#  $ #     #!!!!!!!
		///## #########!!!!!!!
		///#    #    ##!!!!!!!
		///#     $   ##!!!!!!!
		///#  $$#$$  @#!!!!!!!
		///#    #    ##!!!!!!!
		///###########!!!!!!!!</example>
		/// </returns>
		string GetMap(int levelNumber);
	}
}
