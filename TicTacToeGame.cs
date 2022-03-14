using System;

namespace TicTacToe
{

	/// <summary>
	/// Game engine for the Tic Tac Toe game
	/// </summary>
	internal class TicTacToeGame
	{
		// Represents coordinates on the game board
		private struct Coords
		{
			public byte X;
			public byte Y;
		};

		// Determines which token each player has
		private const char PLAYER1_TOKEN = 'X';
		private const char PLAYER2_TOKEN = 'O';

		// Private member fields
		private char[,] gameBoard;
		private byte player;
		private bool playerHasWon, drawHasBeenReached;
		private byte turns;

		// Read-Only public properties
		public bool PlayerHasWon
		{
			get { return playerHasWon; }
		}

		public bool DrawHasBeenReached
		{
			get { return drawHasBeenReached; }
		}

		public byte Player
		{
			get { return player; }
		}

		/// <summary>
		/// Constructor. Initializes the gameBoard array and then calls ResetGame() to set everything up.
		/// </summary>
		public TicTacToeGame()
		{
			gameBoard = new char[3, 3];
			ResetGame();
		}

		/// <summary>
		/// Runs one turn of the game. A turn consists of:
		/// <ul>
		///		<li>Increase the turn counter by 1</li>
		///		<li>Paint the game board to the screen</li>
		///		<li>Get input from the player</li>
		///		<li>Mark the number that the player selected with their token</li>
		///		<li>Check whether the player won</li>
		///		<li>Prep for next player's turn</li>
		/// </ul>
		/// </summary>
		public void RunGameTurn()
		{
			byte selection;
			Coords lastCellCoords;

			// Increment the turn counter
			turns++;
			if (turns == 9)
				drawHasBeenReached = true;

			PaintGameToScreen();

			// Get the player's input for their turn
			selection = GetPlayerSelection();
			lastCellCoords = MarkCellAsTaken(selection);

			// Check if the player won, and if not then prep for next player's turn
			playerHasWon = HasPlayerWon(lastCellCoords) != 0;
			if (!playerHasWon)
				NextPlayersTurn();
		}

		/// <summary>
		/// Resets the game back to the initial state. Should be called once a player has won or after a draw.
		/// </summary>
		public void ResetGame()
		{
			byte cellValue = 1;

			// Loop through the game board cells to reset to their original value
			for (int i = 0; i < gameBoard.GetLength(0); i++)
			{
				for (int j = 0; j < gameBoard.GetLength(1); j++)
				{
					gameBoard[i, j] = cellValue.ToString()[0];
					cellValue++;
				}
			}

			playerHasWon = false;
			drawHasBeenReached = false;
			player = 1;
			turns = 0;

			PaintGameToScreen();
		}

		/// <summary>
		/// Paints the game board to the console screen
		/// </summary>
		public void PaintGameToScreen()
		{
			Console.Clear();
			Console.WriteLine("   _________________ ");
			for (int i = 0; i < gameBoard.GetLength(0); i++)
			{
				Console.WriteLine("  |     |     |     |");
				Console.WriteLine("  |  {0}  |  {1}  |  {2}  |", gameBoard[i, 0], gameBoard[i, 1], gameBoard[i, 2]);
				Console.WriteLine("  |_____|_____|_____|");
			}
		}

		/// <summary>
		/// Switches to the other player
		/// </summary>
		private void NextPlayersTurn()
		{
			player = (byte)(player % 2 + 1);
		}

		/// <summary>
		/// Checks whether a player has won based on the last move made.
		/// </summary>
		/// <param name="lastCellCoords">The coordinates of the last move made</param>
		/// <returns>The winning player number if they have won, otherwise 0</returns>
		private byte HasPlayerWon(Coords lastCellCoords)
		{
			byte playerToCheck = player;
			char playerToken = playerToCheck == 1 ? PLAYER1_TOKEN : PLAYER2_TOKEN;

			// Check if horizontal is taken
			if (
				gameBoard[lastCellCoords.Y, 0] == playerToken &&
				gameBoard[lastCellCoords.Y, 1] == playerToken &&
				gameBoard[lastCellCoords.Y, 2] == playerToken
			)
				return playerToCheck;

			// Check if vertical is taken
			if (
				gameBoard[0, lastCellCoords.X] == playerToken &&
				gameBoard[1, lastCellCoords.X] == playerToken &&
				gameBoard[2, lastCellCoords.X] == playerToken
			)
				return playerToCheck;

			// Check if diagonal is taken
			if ((gameBoard[0, 0] == playerToken &&
				gameBoard[1, 1] == playerToken &&
				gameBoard[2, 2] == playerToken) ||
				(gameBoard[0, 2] == playerToken &&
				gameBoard[1, 1] == playerToken &&
				gameBoard[2, 0] == playerToken)
			)
				return playerToCheck;

			// If we get here, no one has won
			return 0;
		}

		/// <summary>
		/// Replaces the number at the designated cell with the player's token
		/// </summary>
		/// <param name="cell">The value that the player selected (number from 1 to 9)</param>
		/// <returns>Coordinates of the cell on the game board</returns>
		private Coords MarkCellAsTaken(byte cell)
		{
			Coords coords = GetGameCoords(cell);
			gameBoard[coords.Y, coords.X] = player == 1 ? PLAYER1_TOKEN : PLAYER2_TOKEN;
			return coords;
		}

		/// <summary>
		/// Prompts the player to enter their chosen number for their move and checks whether it is a valid entry. Uses recursion (via HandleInvalidInput() method) 
		/// to re-prompt the user if their input is not valid
		/// </summary>
		/// <returns>The number the player chose</returns>
		private byte GetPlayerSelection()
		{
			Console.Write($"\nPlayer {this.player}: Choose your number! ");
			char keyEntered = Console.ReadKey().KeyChar;
			byte selection;
			bool validType = byte.TryParse(keyEntered.ToString(), out selection);

			bool validInput = false;
			if (validType)
				validInput = PlayerSelectionIsValid(selection);

			if (validInput)
				return selection;
			else
				return HandleInvalidInput();
		}

		/// <summary>
		/// Checks whether the selection by the player is an available number on the board (eg. between 1-9 and has not already been taken)
		/// </summary>
		/// <param name="selection">The number the player selected</param>
		/// <returns>True if valid, otherwise False</returns>
		private bool PlayerSelectionIsValid(byte selection)
		{
			Coords coords = GetGameCoords(selection);
			return selection > 0 && selection < 10 && gameBoard[coords.Y, coords.X] != 'X' && gameBoard[coords.Y, coords.X] != 'O';
		}

		/// <summary>
		/// Returns the coordinates on the game board for the number selection provided.
		/// </summary>
		/// <param name="selection">The number the user selected</param>
		/// <returns>Coordinates of the selection on the game board</returns>
		private Coords GetGameCoords(byte selection)
		{
			byte i = (byte)((selection - 1) / 3);
			byte j = (byte)((selection - (i * 3)) - 1);
			Coords coords = new Coords
			{
				X = j,
				Y = i
			};
			return coords;
		}

		/// <summary>
		/// Outputs a message to the user that they must enter a proper number selection, and recalls GetPlayerSelection() method. Note that
		/// this is recursion as this method is only called via GetPlayerSelection().
		/// </summary>
		/// <returns>The selected number as returned by the recursive GetPlayerSelection() method</returns>
		private byte HandleInvalidInput()
		{
			Console.WriteLine("\nPlease enter a number that is available on the game board!\n");
			return GetPlayerSelection();
		}
	}
}
