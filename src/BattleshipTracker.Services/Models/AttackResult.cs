using BattleshipTracker.Services.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleshipTracker.Services.Models
{
    public class AttackResult
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CellStatus CellStatus { get; set; }
        public bool AllShipsSunk { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public GameStatus GameStatus { get; set; }
    }
}
