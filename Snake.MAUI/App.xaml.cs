using Snake.Persistence;
using Snake.Model;
using Snake.MAUI.ViewModel;
using Snake.MAUI.Persistence;

namespace Snake.MAUI
{
    public partial class App : Application
    {
        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly ISnakeDataAccess _snakeDataAccess;
        private readonly SnakeGameModel _snakeGameModel;
        private readonly SnakeViewModel _snakeViewModel;

        public App()
        {
            InitializeComponent();

            _snakeDataAccess = new SnakeMauiDataAccess();

            _snakeGameModel = new SnakeGameModel(_snakeDataAccess);
            _snakeViewModel = new SnakeViewModel(_snakeGameModel);

            _appShell = new AppShell(_snakeDataAccess, _snakeGameModel, _snakeViewModel)
            {
                BindingContext = _snakeViewModel
            };
            MainPage = _appShell;
        }
    }
}