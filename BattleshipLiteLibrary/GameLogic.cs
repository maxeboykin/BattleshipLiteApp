using System.Diagnostics.SymbolStore;
using BattleshipLiteLibrary.Models;

namespace BattleshipLiteLibrary;

// making a static since it has no need to store data
// magic methods to only do things many times

public static class GameLogic
{
    public static void InitializedGrid(PlayerInfoModel model)
    {
        List<string> letters = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E"
        };

        List<int> numbers = new List<int>
        {
            1,
            2,
            3,
            4,
            5
        };

        foreach (string letter in letters)
        {
            foreach (int number in numbers)
            {
                AddGridSpot(model, letter, number);
            }
        }
    }
    
// we dont want people calling this directly
// so use private 
    private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
    {
        GridSpotModel spot = new GridSpotModel
        {
            SpotLetter = letter,
            SpotNumber = number,
            Status = GridSpotStatus.Empty
        };
        model.ShotGrid.Add(spot);
    }

    public static bool PlaceShip(PlayerInfoModel model, string? location)
    {
           if (location == null)
            {
                return false;
            }
    
            if (location.Length != 2)
            {
                return false;
            }
    
            string letter = location.Substring(0, 1);
            string numberText = location.Substring(1, 1);
    
            if (int.TryParse(numberText, out int number) == false)
            {
                return false;
            }
            if(number < 1 || number > 5)
            {
                return false;
            }
            if(letter.ToLower() != "a" && letter.ToLower() != "b" && letter.ToLower() != "c" && letter.ToLower() != "d" && letter.ToLower() != "e")
            {
                return false;
            }
            if(model.ShipLocations.Any(x => x.SpotLetter == letter && x.SpotNumber == number))
            {
                return false;
            }

            GridSpotModel shipSpot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Ship
            };
            model.ShipLocations.Add(shipSpot);
            return true;
    }

    public static bool PlayerStillActive(PlayerInfoModel opponent)
    {
        int? count = 0;
        foreach (var ship in opponent.ShipLocations)
        {
            if (ship.Status != GridSpotStatus.Sunk)
            {
                count++;
            }
        }

        return count != 5;
    }

    public static int GetShotCount(PlayerInfoModel winner)
    {
        throw new NotImplementedException();
    }

    public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
    {
        string shotLetter = shot[0];
        int shotNumber = int.Parse(shot[1].ToString());
        while (!(shotLetter.ToLower()).Contains(shotLetter))
        {
            Console.WriteLine("Please enter a valid letter");
            shotLetter = Console.ReadLine();
        }
        while (shotNumber < 1 || shotNumber > 5)
        {
            Console.WriteLine("Please enter a valid number");
            shotNumber = int.Parse(Console.ReadLine());
        }

        return (shotLetter, shotNumber);
    }

    public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int column)
    {
        bool isValid = true;
        foreach (var shot in activePlayer.ShotGrid)
        {
            if (shot.SpotLetter == row && shot.SpotNumber == column)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    isValid = false;
                }
            }
        }

        return isValid;
    }

    public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
    {
        bool isHit = false;
        foreach (var ship in opponent.ShipLocations)
        {
            {
                if (ship.SpotLetter == row && ship.SpotNumber == column)
                {
                    isHit = true;
                    ship.Status = GridSpotStatus.Hit;
                }
            }
            return isHit;
        }
    }

    public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int column, bool isAHit)
    {
        foreach (var shot in activePlayer.ShotGrid)
        {
            if (shot.SpotLetter == row && shot.SpotNumber == column)
            {
                if (isAHit)
                {
                    shot.Status = GridSpotStatus.Hit;
                }
                else
                {
                    shot.Status = GridSpotStatus.Miss;
                }
            }
        }
    }
    
}