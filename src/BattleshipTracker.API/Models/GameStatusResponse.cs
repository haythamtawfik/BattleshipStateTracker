using BattleshipTracker.Services.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleshipTracker.API.Models
{
    public class GameStatusResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public GameStatus GameStatus { get; set; }
    }
}
