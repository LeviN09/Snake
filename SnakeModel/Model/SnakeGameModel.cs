using Snake.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Model
{

    public class SnakeGameModel
    {
        #region Fields

        private ISnakeDataAccess _dataAccess;
        private SnakeMap _map;
        private int _score;

        #endregion

        #region Properties

        public int Score { get { return _score; } }

        public SnakeMap Map { get { return _map; } }

        public bool IsGameOver { get { return _map.IsNextKilling(); } }

        #endregion

        #region Events

        public event EventHandler<SnakeEventArgs>? GameStep;
        public event EventHandler<SnakeEventArgs>? GameOver;

        #endregion

        #region Constructor

        public SnakeGameModel(ISnakeDataAccess dataAccess)
        {
            _map = new SnakeMap(10);
            _dataAccess = dataAccess;
        }

        #endregion

        #region Public game methods

        public async Task LoadNewGameAsync(MapType type)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _map = await _dataAccess.LoadAsync(type);
            _score = 0;
        }

        public void StepGame()
        {
            if (IsGameOver)
            {
                OnGameOver();
                return;
            }

            if (_map.IsNextEgg())
            {
                _map.GrowSnake();
                _map.SetNewEggCoords();
                _score++;
            }
            else
            {
                _map.StepSnake();
            }

            OnGameStep();
        }

        public void ChangeDirection(LR dir)
        {
            _map.Turn(dir);
        }

        #endregion

        #region Private event methods

        private void OnGameStep()
        {
            GameStep?.Invoke(this, new SnakeEventArgs(Score, false));
        }

        private void OnGameOver()
        {
            GameOver?.Invoke(this, new SnakeEventArgs(Score, true));
        }

        #endregion
    }
}
