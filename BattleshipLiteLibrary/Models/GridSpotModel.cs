namespace BattleshipLiteLibrary.Models;

public class GridSpotModel
{
    public string SpotLetter  { get; set; }
    public int SpotNumber { get; set; }

    public GridSpotStatus Status { get; set; } = GridSpotStatus.Empty;
    // 0 = empty, 1 = ship, 2 = miss, 3 = hit, 4 = sunk

}
