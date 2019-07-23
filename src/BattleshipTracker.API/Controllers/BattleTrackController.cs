using BattleshipTracker.API.Models;
using BattleshipTracker.Services;
using BattleshipTracker.Services.Exceptions;
using BattleshipTracker.Services.Interfaces;
using BattleshipTracker.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;

namespace BattleshipTracker.API.Controllers
{
    [Route("api/BattleTrack")]
    [ApiController]
    public class BattleTrackController : ControllerBase
    {
        private readonly ILogger<BattleTrackController> _logger;
        private readonly IGameProcessorService _gameProcessorService;

        public BattleTrackController(ILogger<BattleTrackController> logger, IGameProcessorService gameProcessorService)
        {
            _logger = logger??new NullLogger<BattleTrackController>();
            _gameProcessorService = gameProcessorService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetGameStatus()
        {
            try
            {
                var status = await _gameProcessorService.GetCurrentGameStatus();
                return Ok(new GameStatusResponse { GameStatus = status });
            }
            catch (NullGameException excp)
            {
                _logger.LogError(excp.Message);
                return NotFound(excp); //as it didn't find it initialised so it can be considered Not Found
            }
        }

        [Route("attacks")]
        [HttpPost]
        public async Task<IActionResult> AttackCell([FromBody]Point attackedPoint)
        {
            try
            {
                var attackResult = await _gameProcessorService.AttackCell(attackedPoint);
                return Ok(attackResult);
            }
            catch (AttackDeniedException excp)
            {
                _logger.LogError(excp.Message);
                return Conflict(excp);
            }
            catch (Exception excp)
            {
                _logger.LogError(excp.Message);
                return BadRequest(excp);
            }
        }
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody]CreateGameRequest createGameRequest)
        {
            try
            {
                var game = await _gameProcessorService.CreateGame(createGameRequest.BoardSize, createGameRequest.ShipsNo, createGameRequest.ShipLength);
                return Created(Request.Path, game);
            }
            catch (Exception excp)
            {
                _logger.LogError(excp.Message);
                return BadRequest(excp);
            }
        }

        [Route("ships")]
        [HttpPost]
        public async Task<IActionResult> CreateShip([FromBody]CreateShipRequest createShipRequest)
        {
            try
            {
                var ship = await _gameProcessorService.CreateShip(createShipRequest.StartPoint, createShipRequest.Direction);
                return Created(Request.Path, ship);
            }
            catch (Exception excp)
            {
                _logger.LogError(excp.Message);
                return BadRequest(excp);
            }
        }
    }
}