// See https://aka.ms/new-console-template for more information

using System.Text.Json.Serialization.Metadata;
using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
namespace BattleshipLite
{
    class Program
    {

        static void Main(string[] args)
        {
            WelcomeMessage();
            
            PlayerInfoModel player1 = CreatePlayer("Player 1");
            PlayerInfoModel player2 = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                // Display grid from activeplayer  on where they fired 
                DisplayShotGrid(activePlayer);
                // ask activeplayer for a shot
                // determine if it is a valid shot 
                // determine shot results 
                // determine if the game is over
                // if over, set player1 as the winner
                // else, swap positions (activePlayer to opponent) 

            } while (winner == null);
            
            Console.ReadLine();
        }

        //using it here since its displaying to console 
        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;
            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber}");
                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.Write(" ? ");
                }
            }
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("created by Tim Corey");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");
            
            // Ask the user for their name
            output.UsersName = AskForUsersName();
            
            // load up the shot grid
            GameLogic.InitializedGrid(output);
            // Ask the user for their 5 ship placements
            PlaceShips(output);
            
            // Clear the screen 
            Console.Clear();
            
            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                if (model.ShipLocations.Count == 0)
                {
                    Console.WriteLine("Where do you want to place your first ship: ");
                }
                else
                {
                    Console.WriteLine("Where do you want to place your next ship: ");
                }

                string? location = Console.ReadLine();
                bool isValidLocation = GameLogic.PlaceShip(model, location);
                if (!isValidLocation)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                }

            } while (model.ShipLocations.Count < 5);
        }

        private static string AskForUsersName()
        {
            Console.Write("What is your name: ");
            string output = Console.ReadLine();

            return output;
        }

    }
}