using System;

namespace Orpius.Sokoban
{
	class SokobanException : ApplicationException
	{
		public SokobanException()
		{
		}

		public SokobanException(string message)
			: base(message)
		{
		}
	}
}