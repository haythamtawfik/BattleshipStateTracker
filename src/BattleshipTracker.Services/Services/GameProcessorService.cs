using BattleshipTracker.Services.Enums;
using BattleshipTracker.Services.Exceptions;
using BattleshipTracker.Services.Interfaces;
using BattleshipTracker.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleshipTracker.Services.Services
{
    public class GameProcessorService : IGameProcessorService
    {
        private Game _game;
        private readonly ILogger<GameProcessorService> _logger;
        public GameProcessorService(ILogger<GameProcessorService> logger)
        {
            _logger = logger ?? new NullLogger<GameProcessorService>();
        }
        public async Task<Game> CreateGame(int boardSize, int shipsNo, int shipLength)
        {
            _logger.LogInformation($"Creating game with size {boardSize}");
            if (shipLength > boardSize)
                throw new InCreatableGameException("Can't create game with ship Length greater than the board size");

            _game = new Game(boardSize)
            {
                GameId = Guid.NewGuid(),
                Board = new BattleBoard
                {
                    Cells = await BoardCell.CreateCellArray(boardSize),
                    Size = boardSize,
                    MaxShips = shipsNo,
                    ShipLength = shipLength,
                    Ships = new List<Ship>()
                },
                Status = GameStatus.NewGame
            };
            _logger.LogInformation($"Game {_game.GameId.ToString()} was created");
            return _game;
        }

        public async Task<GameStatus> GetCurrentGameStatus()
        {
            if (_game == null)
                throw new NullGameException("The game hasn't been initialised yet");

            return await Task.FromResult(_game.Status);
        }

        public async Task<Ship> CreateShip(Point startPoint, ShipDirection direction)
        {
            _logger.LogInformation($"Creating a ship at the point {startPoint.ToString()}");

            if ((int)_game.Status > 1)
                throw new InCreatableShipException("The game has to be new to add more ships. The ship hasn't been created");

            if (!IsValidPoint(_game.Board, startPoint) || !IsShipCreatable(_game.Board, startPoint, direction))
                throw new InCreatableShipException("Ship can't be created ");

            if (_game.Board.CurrentShips >= _game.Board.MaxShips)
                throw new InCreatableShipException("The ocean is too busy at the moment, can't fit more ships");

            var ship = await FillCells(_game.Board, startPoint, direction);
            _logger.LogInformation($"Created a ship at the point {startPoint.ToString()}");
            _game.Board.CurrentShips++;
            return ship;
        }

        public async Task<AttackResult> AttackCell(Point attackedCellPoint)
        {

            if (_game.Status == GameStatus.GameOver)
                throw new AttackDeniedException("Can't process attack as the game is over already");

            if (_game.Board.CurrentShips != _game.Board.MaxShips)
                throw new AttackDeniedException($"You have to create a total of {_game.Board.MaxShips} ships to start the battle");

            if (_game.Status == GameStatus.NewGame)
                _game.Status = GameStatus.Active;


            if (!IsValidPoint(_game.Board, attackedCellPoint))
                throw new AttackDeniedException($"The attacked cell is invalid X and Y coordinates have to be from 0 to {_game.BoardSize}");

            var attackedCell = _game.Board.Cells[attackedCellPoint.X, attackedCellPoint.Y];

            if (attackedCell.Status == CellStatus.Empty)
                attackedCell.Status = CellStatus.Miss;
            else if (attackedCell.Status == CellStatus.HasShip)
                attackedCell.Status = CellStatus.Hit;

            _logger.LogInformation($"Cell ({attackedCell.X},{attackedCell.Y}) was attacked and the result is {Enum.GetName(typeof(CellStatus), attackedCell.Status)}");

            var allShipsSunk = _game.Board.Ships.All(s => s.HasSunk); //TODO: room for optimisation here

            if (allShipsSunk)
            {
                _game.Status = GameStatus.GameOver;
                _logger.LogInformation("Game Over! All ships has been sunk!");
            }
            return await Task.FromResult(new AttackResult
            {
                CellStatus = attackedCell.Status,
                AllShipsSunk = _game.Board.Ships.All(s => s.HasSunk),
                GameStatus = _game.Status
            });
        }

        private async Task<Ship> FillCells(BattleBoard board, Point startPoint, ShipDirection direction)
        {
            var ship = new Ship
            {
                Direction = direction,
                Cells = new List<BoardCell>()
            };
            if (direction == ShipDirection.Horizontal)
            {
                for (int i = startPoint.X; i < startPoint.X + board.ShipLength; i++)
                {
                    board.Cells[i, startPoint.Y].Status = CellStatus.HasShip;
                    ship.Cells.Add(board.Cells[i, startPoint.Y]);
                }

                board.Ships.Add(ship);
                _logger.LogInformation($"Filled in the cells from cell ({startPoint.X},{startPoint.Y}) to cell ({startPoint.X + board.ShipLength},{startPoint.Y})");
            }
            else
            {
                for (int j = startPoint.Y; j < startPoint.Y + board.ShipLength; j++)
                {
                    board.Cells[startPoint.X, j].Status = CellStatus.HasShip;
                    ship.Cells.Add(board.Cells[startPoint.X, j]);
                }
                board.Ships.Add(ship);
                _logger.LogInformation($"Filled in the cells from cell ({startPoint.X},{startPoint.Y}) to cell ({startPoint.X },{startPoint.Y + board.ShipLength})");
            }
            return await Task.FromResult(ship);
        }

        private bool IsValidPoint(BattleBoard board, Point startPoint)
        {
            if (startPoint.X >= 0 && startPoint.X < board.Size && startPoint.Y >= 0 && startPoint.Y < board.Size)
                return true;

            return false;
        }

        private bool IsShipCreatable(BattleBoard board, Point startPoint, ShipDirection direction)
        {
            return IsInsideBoard(board, startPoint, direction) && AreCellsEmpty(board, startPoint, direction);
        }

        private bool AreCellsEmpty(BattleBoard board, Point startPoint, ShipDirection direction)
        {
            if (direction == ShipDirection.Horizontal)
            {
                for (int i = startPoint.X; i < startPoint.X + board.ShipLength; i++)
                    if (board.Cells[i, startPoint.Y].Status != CellStatus.Empty)
                        return false;
            }
            else
            {
                for (int j = startPoint.Y; j < startPoint.Y + board.ShipLength; j++)
                    if (board.Cells[startPoint.X, j].Status != CellStatus.Empty)
                        return false;
            }
            return true;
        }

        private bool IsInsideBoard(BattleBoard board, Point startPoint, ShipDirection direction)
        {
            if (direction == ShipDirection.Horizontal)
            {
                if ((startPoint.X + board.ShipLength) < board.Size)
                    return true;
            }
            else
            {
                if ((startPoint.Y + board.ShipLength) < board.Size)
                    return true;
            }
            return false;
        }

    }
}
