using System.Data;
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
        bool output = false;
        (string row, int column) = SplitShotIntoRowAndColumn(location);

        bool isValidLocation = ValidateGridLocation(model, row, column);
        bool isShipAlreadyPlaced = ValidateShipLocation(model, row, column);

            if(isValidLocation && !isShipAlreadyPlaced)
            {
                GridSpotModel shipSpot = new GridSpotModel
                {
                    SpotLetter = row.ToUpper(),
                    SpotNumber = column,
                    Status = GridSpotStatus.Ship
                };
                model.ShipLocations.Add(shipSpot);
                output = true;
            }
            return output;
    }

    private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
    {
        bool isValidShipLocation = true;
        foreach (var ship in model.ShipLocations)
        {
            // doesnt matter if its ship or sunk (cant be sunk anyway since that happens when game starts)
            // if any spot is in shiplocation list then you know a ship has been there and hence return false
            if(ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
            {
                isValidShipLocation = false;
            }
        }
        return isValidShipLocation;
    }
    // test push

    private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
    {
        bool isValidGridLocation = false;
        foreach (var shot in model.ShotGrid)
        {
            if(shot.SpotLetter == row.ToUpper() && shot.SpotNumber == column)
            {
                isValidGridLocation = true;
            }
        }
        return isValidGridLocation;
    }

    public static bool PlayerStillActive(PlayerInfoModel player)
    {
        bool isActive = false;
        foreach (var ship in player.ShipLocations)
        {
            if (ship.Status != GridSpotStatus.Sunk)
            {
                isActive = true;
            }
        }

        return isActive;
    }

    public static int GetShotCount(PlayerInfoModel player)
    {
        int shotCount = 0;

        foreach (var shot in player.ShotGrid)
        {
            if (shot.Status != GridSpotStatus.Empty)
            {
                shotCount += 1;
            }
        }

        return shotCount;
    }

    public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
    {
        string row = "";
        int column = 0;

        if (shot.Length != 2)
        {
            throw new ArgumentException("This was an invalid shot type.", shot);
        }
        
        char[] shotArray = shot.ToArray();

        row = shotArray[0].ToString();
        column = int.Parse(shotArray[1].ToString());
        return (row, column);
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