using BattleshipTracker.Services.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace BattleshipTracker.Services.Models
{
    public class Game
    {
        public Guid GameId { get; set; }
        public BattleBoard Board { get; set; }
        public int BoardSize { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GameStatus Status { get; set; }
        public Game(int boardSize)
        {
            BoardSize = boardSize;
        }
    }
}
