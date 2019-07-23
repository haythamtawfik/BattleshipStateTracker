namespace BattleshipTracker.API.Models
{
    public class CreateGameRequest
    {
        public int BoardSize { get; set; }
        public int ShipsNo { get; set; }
        public int ShipLength { get; set; }
    }
}
