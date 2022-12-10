// See https://aka.ms/new-console-template for more information

using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
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
            
            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                DisplayShotGrid(activePlayer);
                
                RecordPlayerShot(activePlayer, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);
                
                if (doesGameContinue == true)
                {
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }
                

            } while (winner == null);

            IdentifyWinner(winner);
            
            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations {winner.UsersName}, you won!");
            Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots to win.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot();
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                if (isValidShot == false)
                {
                    Console.WriteLine("That shot was invalid. Please try again.");
                }

            } while (!isValidShot);

            // Determine the shot results 
            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);
           
            // record shot results  
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

        }

        private static string AskForShot()
        {
            Console.WriteLine("Please enter your shot selection: ");
            string output = Console.ReadLine();
            return output;
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