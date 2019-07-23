using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using BattleshipTracker.Services.Interfaces;
using BattleshipTracker.Services.Services;
using BattleshipTracker.Services.Exceptions;
using System;
using System.Threading.Tasks;
using BattleshipTracker.Services.Enums;

namespace BattleshipTracker.UnitTests
{
    [TestClass]
    public class GameProcessorServoiceTest
    {
        private ILogger<GameProcessorService> _logger;
        private IGameProcessorService _target;

        [TestInitialize]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<GameProcessorService>>();
            _target = new GameProcessorService(_logger);

        }

        [TestMethod]
        public async Task CanCreateGameSuccessfully()
        {
            var game= await _target.CreateGame(10, 6, 3);

            Assert.AreNotEqual(Guid.Empty, game.GameId);
            Assert.AreEqual(GameStatus.NewGame, game.Status);
            Assert.AreEqual(100, game.Board.Cells.Length);
            Assert.AreEqual(CellStatus.Empty, game.Board.Cells[9, 9].Status);
            Assert.AreEqual(9, game.Board.Cells[9, 9].X);
        }

        [TestMethod]
        public async Task GameWithLargeShipLength_throwsException()
        {
            await Assert.ThrowsExceptionAsync<InCreatableGameException>(()=> _target.CreateGame(10, 6, 30));
        }

        //TODO: Write the rest of the unit tests 
    }
}
