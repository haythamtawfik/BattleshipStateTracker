using BattleshipTracker.Services.Enums;
using BattleshipTracker.Services.Models;
using System.Threading.Tasks;

namespace BattleshipTracker.Services.Interfaces
{
    public interface IGameProcessorService
    {
        Task<Game> CreateGame(int boardSize, int shipsNo, int shipLength);
        Task<Ship> CreateShip(Point startPoint, ShipDirection direction);
        Task<GameStatus> GetCurrentGameStatus();
        Task<AttackResult> AttackCell(Point attackedCellPoint);
    }
}
