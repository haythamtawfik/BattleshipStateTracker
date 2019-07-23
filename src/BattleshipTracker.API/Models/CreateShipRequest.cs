using BattleshipTracker.Services;
using BattleshipTracker.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleshipTracker.API.Models
{
    public class CreateShipRequest
    {
        public Point StartPoint { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ShipDirection Direction { get; set; }
    }
}
