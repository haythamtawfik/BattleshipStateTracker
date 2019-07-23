using BattleshipTracker.Services.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipTracker.Services.Models
{
    public class Ship
    {
        public IList<BoardCell> Cells { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ShipDirection Direction { get; set; }
        public bool HasSunk { get { return Cells.All(c => c.Status == CellStatus.Hit); } }
    }
}
