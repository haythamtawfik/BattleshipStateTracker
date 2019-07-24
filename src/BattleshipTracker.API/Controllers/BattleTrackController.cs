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
            _logger = logger ?? new NullLogger<BattleTrackController>();
            _gameProcessorService = gameProcessorService;
        }

        /// <response code="200">Returns the current game status</response>
        /// <response code="400">When receiving a bad request</response> 
        /// <response code="404">If the game is not found</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
            catch (Exception excp)
            {
                _logger.LogError(excp.Message);
                return BadRequest(excp); //as it didn't find it initialised so it can be considered Not Found
            }
        }

        /// <response code="200">Returns the attack result</response>
        /// <response code="400">When receiving a bad request</response> 
        /// <response code="409">If the attack is denied for any reason</response>
        [Route("attacks")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
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

        /// <response code="201">Returns the created game</response>
        /// <response code="400">When receiving a bad request</response>
        /// <response code="409">If the game can't be created</response>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateGame([FromBody]CreateGameRequest createGameRequest)
        {
            try
            {
                var game = await _gameProcessorService.CreateGame(createGameRequest.BoardSize, createGameRequest.ShipsNo, createGameRequest.ShipLength);
                return Created(Request.Path, game);
            }
            catch (InCreatableGameException excp)
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

        /// <response code="201">Returns the created ship</response>
        /// <response code="400">When receiving a bad request</response>
        /// <response code="409">If the ship can't be created</response>
        [Route("ships")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateShip([FromBody]CreateShipRequest createShipRequest)
        {
            try
            {
                var ship = await _gameProcessorService.CreateShip(createShipRequest.StartPoint, createShipRequest.Direction);
                return Created(Request.Path, ship);
            }
            catch (InCreatableShipException excp)
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
    }
}