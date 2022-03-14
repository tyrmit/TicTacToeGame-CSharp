using System;

namespace TicTacToe
{
	internal class Program
	{
		/// <summary>
		/// Main method of the program. Drives the instance of the TicTacToeGame object to step through each turn of the game,
		/// and handle win or draw states.
		/// </summary>
		static void Main()
		{
			TicTacToeGame game = new();

			// Infinite game loop
			while (true)
			{
				// Loop through each turn of the game until either someone wins or it's a draw
				do
					game.RunGameTurn();
				while (!game.PlayerHasWon && !game.DrawHasBeenReached);

				// Paint the game to the screen one last time for the updated win/draw state
				game.PaintGameToScreen();

				// Need an extra line before the win/draw alert
				Console.WriteLine();

				// Let the player know they've either won or it's a draw
				if (game.PlayerHasWon)
					Console.WriteLine($"Player {game.Player} has won!");
				else if (game.DrawHasBeenReached)
					Console.WriteLine("It's a draw! Better luck next time");

				// Now prompt to reset the game for another go
				Console.WriteLine("Press any Key to Reset the Game");
				Console.ReadKey();

				game.ResetGame();
			}

		}
	}
}
