using BattleshipTracker.Services.Enums;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleshipTracker.Services.Models
{
    public class BoardCell
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CellStatus Status { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public static async Task<BoardCell[,]> CreateCellArray(int size)
        {
            BoardCell[,] cells = new BoardCell[size, size];
            await Task.Run(() =>
            {
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        cells[i, j] = new BoardCell
                        {
                            X = i,
                            Y = j,
                            Status = CellStatus.Empty
                        };
                    }
            });
            return cells;
        }
    }
}
