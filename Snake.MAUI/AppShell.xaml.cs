using Snake.Model;
using Snake.MAUI.ViewModel;
using Snake.Persistence;

namespace Snake.MAUI
{
    public partial class AppShell : Shell
    {
        private ISnakeDataAccess _snakeDataAccess;
        private readonly SnakeGameModel _snakeGameModel;
        private readonly SnakeViewModel _snakeViewModel;

        private readonly IDispatcherTimer _timer;

        public AppShell(ISnakeDataAccess snakeDataAccess,
        SnakeGameModel snakeGameModel,
        SnakeViewModel snakeViewModel)
        {
            InitializeComponent();

            _snakeDataAccess = snakeDataAccess;
            _snakeGameModel = snakeGameModel;
            _snakeViewModel = snakeViewModel;

            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += (_, _) => TimerTick();

            _snakeGameModel.GameOver += SnakeGameModel_GameOver;

            _snakeViewModel.LoadNewGame += SnakeViewModel_NewGame;
            _snakeViewModel.PauseGame += SnakeViewModel_PauseGame;
            _snakeViewModel.TurnLeft += SnakeViewModel_TurnLeft;
            _snakeViewModel.TurnRight += SnakeViewModel_TurnRight;
        }


        internal void StartTimer() => _timer.Start();
        internal void StopTimer() => _timer.Stop();

        private void TimerTick()
        {
            _snakeGameModel.StepGame();
            _snakeViewModel.ColorMap();
        }

        private async void SnakeGameModel_GameOver(object? sender, SnakeEventArgs e)
        {
            StopTimer();

            await DisplayAlert("Snake", "Vesztettél! Pontod: " + e.Score, "OK");
        }

        private async void SnakeViewModel_NewGame(object? sender, NewGameEventArgs e)
        {
            await _snakeGameModel.LoadNewGameAsync(e.Type);
            _snakeViewModel.GenerateMap();
            StartTimer();
        }

        private void SnakeViewModel_PauseGame(object? sender, EventArgs e)
        {
            if (_timer.IsRunning)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }

        private void SnakeViewModel_TurnLeft(object? sender, EventArgs e)
        {
            if (_timer.IsRunning)
            {
                _snakeGameModel.ChangeDirection(LR.Right);
            }
        }

        private void SnakeViewModel_TurnRight(object? sender, EventArgs e)
        {
            if (_timer.IsRunning)
            {
                _snakeGameModel.ChangeDirection(LR.Left);
            }
        }
    }
}