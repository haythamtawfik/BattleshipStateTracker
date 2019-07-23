using System.Collections.Generic;

namespace BattleshipTracker.Services.Models
{
    public class BattleBoard
    {
        public BoardCell[,] Cells { get; set; }
        public int Size { get; set; }
        public int MaxShips { get; set; }
        public int CurrentShips { get; set; }
        public int ShipLength { get; set; }
        public IList<Ship> Ships { get; set; }
    }
}
