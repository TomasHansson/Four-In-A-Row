using System;
using System.Text;

namespace Four_In_A_Row
{
    class Program
    {
        private static readonly int _gameBoardHeigth = 6;
        private static readonly int _gameBoardWidth = 7;
        private static readonly int _matchesInARowToWin = 4;
        private static int[,] GameBoard { get; set; } = new int[_gameBoardHeigth, _gameBoardWidth];
        private static int CurrentPlayer { get; set; }
        private static int Winner { get; set; }
        private static bool GameOver { get; set; }

        private static void Main(string[] args)
        {
            while (!GameOver)
            {
                NewTurn();
                PlayerInput();
                GameOver = WinnerFound() || BoardIsFull();
            }
            if (Winner == 0)
                DeclareTie();
            else
                DeclareWinner();
        }

        private static void NewTurn()
        {
            Console.Clear();
            CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;
            Console.WriteLine("It's Player " + CurrentPlayer + "'s turn.\n");
            PrintBoard();
            Console.WriteLine("In which column would you like to insert your next marker?");
        }

        private static void PrintBoard()
        {
            WriteHeader();
            for (int i = 0; i < _gameBoardHeigth; i++)
            {
                for (int j = 0; j < _gameBoardWidth; j++)
                    Console.Write(GameBoard[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void WriteHeader()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i <= _gameBoardWidth; i++)
                stringBuilder.Append(i + " ");
            stringBuilder.Append("(Columns)\n");
            Console.WriteLine(stringBuilder);
        }

        private static void PlayerInput()
        {
            int firstEmptyRow = -1;
            int insertColumn = 0;
            while (firstEmptyRow == -1)
            {
                insertColumn = ValidateColumnInput();
                firstEmptyRow = FindFirstEmptyRowInColumn(insertColumn);
                if (firstEmptyRow == -1)
                    Console.WriteLine("That column is already full.");
            }
            SaveInput(firstEmptyRow, insertColumn);
        }

        private static int ValidateColumnInput()
        {
            int insertColumn = 0;
            bool correctInput = false;
            while (!correctInput)
            {
                correctInput = Int32.TryParse(Console.ReadLine(), out insertColumn);
                correctInput = insertColumn > 0 && insertColumn <= _gameBoardWidth;
                insertColumn--; // The array index differs from the visual presentation.
                if (!correctInput)
                    Console.WriteLine("Your need to input a number between 1 and " + _gameBoardWidth + ".");
            }
            return insertColumn;
        }

        private static int FindFirstEmptyRowInColumn(int column)
        {
            for (int i = _gameBoardHeigth - 1; i >= 0; i--)
                if (GameBoard[i, column] == 0)
                    return i;
            return -1;
        }

        private static int LatestInputRow { get; set; }
        private static int LatestInputColumn { get; set; }
        private static void SaveInput(int firstEmptyRow, int insertColumn)
        {
            GameBoard[firstEmptyRow, insertColumn] = CurrentPlayer;
            LatestInputRow = firstEmptyRow;
            LatestInputColumn = insertColumn;
        }

        private static bool WinnerFound()
        {
            bool winnerFound = CheckRow() || CheckColumn() || CheckDiagonaleRising() || CheckDiagonaleFalling();
            if (winnerFound)
                Winner = CurrentPlayer;
            return winnerFound;
        }

        private static bool BoardIsFull()
        {
            foreach (int number in GameBoard)
                if (number == 0)
                    return false;
            return true;
        }

        private static void DeclareTie()
        {
            Console.Clear();
            Console.WriteLine("The board is full and no winner can be determined. The game is a tie.\n");
            PrintBoard();
        }

        private static void DeclareWinner()
        {
            Console.Clear();
            Console.WriteLine("The Winner is Player " + Winner + "!\n");
            PrintBoard();
        }

        private static bool CheckRow()
        {
            int playerMatchCount = 1 + MatchesInRowUpwards() + MatchesInRowDownwards();
            return playerMatchCount >= _matchesInARowToWin;
        }

        private static bool CheckColumn()
        {
            int playerMatchCount = 1 + MatchesInColumnLeft() + MatchesInColumnRight();
            return playerMatchCount >= _matchesInARowToWin;
        }

        private static bool CheckDiagonaleRising()
        {
            int playerMatchCount = 1 + MatchesDiagonaleNE() + MatchesDiagonaleSW();
            return playerMatchCount >= _matchesInARowToWin;
        }

        private static bool CheckDiagonaleFalling()
        {
            int playerMatchCount = 1 + MatchesDiagonaleNW() + MatchesDiagonaleSE();
            return playerMatchCount >= _matchesInARowToWin;
        }

        private static int MatchesInRowUpwards()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputRow - (1 + matches) < 0)
                    break;
                if (GameBoard[LatestInputRow - (1 + matches), LatestInputColumn] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }

        private static int MatchesInRowDownwards()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputRow + (1 + matches) >= _gameBoardHeigth)
                    break;
                if (GameBoard[LatestInputRow + (1 + matches), LatestInputColumn] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }

        private static int MatchesInColumnRight()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputColumn + (1 + matches) >= _gameBoardWidth)
                    break;
                if (GameBoard[LatestInputRow, LatestInputColumn + (1 + matches)] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }

        private static int MatchesInColumnLeft()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputColumn - (1 + matches) < 0)
                    break;
                if (GameBoard[LatestInputRow, LatestInputColumn - (1 + matches)] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }

        private static int MatchesDiagonaleNE()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputRow - (1 + matches) < 0 || LatestInputColumn + (1 + matches) >= _gameBoardWidth)
                    break;
                if (GameBoard[LatestInputRow - (1 + matches), LatestInputColumn + (1 + matches)] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }

        private static int MatchesDiagonaleNW()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputRow - (1 + matches) < 0 || LatestInputColumn - (1 + matches) < 0)
                    break;
                if (GameBoard[LatestInputRow - (1 + matches), LatestInputColumn - (1 + matches)] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }

        private static int MatchesDiagonaleSE()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputRow + (1 + matches) >= _gameBoardHeigth || LatestInputColumn + (1 + matches) >= _gameBoardWidth)
                    break;
                if (GameBoard[LatestInputRow + (1 + matches), LatestInputColumn + (1 + matches)] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }

        private static int MatchesDiagonaleSW()
        {
            int matches = 0;
            while (true)
            {
                if (LatestInputRow + (1 + matches) >= _gameBoardHeigth || LatestInputColumn - (1 + matches) < 0)
                    break;
                if (GameBoard[LatestInputRow + (1 + matches), LatestInputColumn - (1 + matches)] == CurrentPlayer)
                    matches++;
                else
                    break;
            }
            return matches;
        }
    }
}
