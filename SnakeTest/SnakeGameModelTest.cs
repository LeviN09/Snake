using Snake.Model;
using Snake.Persistence;
using Moq;

namespace SnakeTest
{
    [TestClass]
    public class SnakeGameModelTest
    {
        private SnakeGameModel _game = null!;
        private SnakeMap _map = null!;
        private Mock<ISnakeDataAccess> _mock = null!;

        [TestInitialize]
        public void Initialize()
        {
            _map = new SnakeMap(9);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    _map.SetType(i, j, "Empty");
                }
            }
            List<(int, int)> snake = new List<(int, int)>();
            snake.Add((0, 5));
            snake.Add((1, 5));
            snake.Add((2, 5));
            snake.Add((3, 5));
            snake.Add((4, 5));
            _map.InitSnake(snake, "South");
            _map.SetNewEggCoords();

            _mock = new Mock<ISnakeDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_map));

            _game = new SnakeGameModel(_mock.Object);

            _game.GameStep += new EventHandler<SnakeEventArgs>(Model_GameStep);
            _game.GameOver += new EventHandler<SnakeEventArgs>(Model_GameOver);
        }

        [TestMethod]
        public async Task SnakeGameModelLoadNewGameAsyncTest()
        {
            await _game.LoadNewGameAsync("path");

            Assert.IsFalse(_game.IsGameOver);
            Assert.AreEqual(Direction.South, _game.Map.SnakeDir);
            Assert.AreEqual(CoordType.Empty, _game.Map[0, 0]);
            Assert.AreEqual(0, _game.Score);

            _mock.Verify(dataAccess => dataAccess.LoadAsync("path"), Times.Once());
        }

        [TestMethod]
        public async Task SnakeGameModelStepGameTest()
        {
            await _game.LoadNewGameAsync("path");

            Assert.AreEqual((0, 5), _game.Map.SnakeCoords[0]);

            _game.StepGame();

            Assert.AreEqual((0, 6), _game.Map.SnakeCoords[0]);
            Assert.IsFalse(_game.Map.IsNextKilling());
            Assert.AreEqual(Direction.South, _game.Map.SnakeDir);

            _game.StepGame();

            Assert.AreEqual((0, 7), _game.Map.SnakeCoords[0]);
            Assert.IsFalse(_game.Map.IsNextKilling());

            _mock.Verify(dataAccess => dataAccess.LoadAsync("path"), Times.Once());
        }

        [TestMethod]
        public async Task SnakeGameModelChangeDirectionTest()
        {
            await _game.LoadNewGameAsync("path");

            Assert.AreEqual(Direction.South, _game.Map.SnakeDir);

            _game.ChangeDirection(LR.Left);

            Assert.AreEqual(Direction.East, _game.Map.SnakeDir);

            _mock.Verify(dataAccess => dataAccess.LoadAsync("path"), Times.Once());
        }

        private void Model_GameStep(Object? sender, SnakeEventArgs e)
        {
            Assert.IsFalse(_game.IsGameOver);
            Assert.AreEqual(_game.Score, e.Score);
            Assert.IsFalse(e.IsOver);
        }
        private void Model_GameOver(Object? sender, SnakeEventArgs e)
        {
            Assert.IsTrue(_game.IsGameOver);
            Assert.IsTrue(e.IsOver);
        }
    }
}