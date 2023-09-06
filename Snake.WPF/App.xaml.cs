using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using Snake.Model;
using Snake.Persistence;
using Snake.WPF.ViewModel;

namespace Snake.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private ISnakeDataAccess _dataAccess = null!;
        private SnakeGameModel _model = null!;
        private SnakeViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;

        #endregion

        #region Constructor

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _dataAccess = new SnakeFileDataAccess();
            _model = new SnakeGameModel(_dataAccess);
            _model.GameStep += new EventHandler<SnakeEventArgs>(Model_GameStep);
            _model.GameOver += new EventHandler<SnakeEventArgs>(Model_GameOver);

            _viewModel = new SnakeViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.TurnLeft += new EventHandler(ViewModel_TurnLeft);
            _viewModel.TurnRight += new EventHandler(ViewModel_TurnRight);
            _viewModel.Pause += new EventHandler(ViewModel_Pause);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
        
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);
            _view.Show();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(Timer_Tick);

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.StepGame();
        }

        #endregion

        #region View event handlers

        private void View_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            bool restartTimer = _timer.IsEnabled;

            _timer.Stop();

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Snake", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;

                if (restartTimer)
                {
                    _timer.Start();
                }
            }
        }

        #endregion

        #region ViewModel event handlers

        private async void ViewModel_NewGame(object? sender, EventArgs e)
        {
            bool restartTimer = _timer.IsEnabled;
            _timer.Stop();

            OpenFileDialog _openFileDialog = new OpenFileDialog();

            if (_openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Debug.WriteLine("Model loading");
                    await _model.LoadNewGameAsync(_openFileDialog.FileName);
                    Debug.WriteLine("Model loaded " + _model.Map.MapSize);
                    _viewModel.GenerateMap();
                    restartTimer= true;
                }
                catch (SnakeDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine +
                        "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _timer.Start();
        }

        private void ViewModel_TurnLeft(object? sender, EventArgs e)
        {
            if (_timer.IsEnabled)
                _model.ChangeDirection(LR.Left);
        }

        private void ViewModel_TurnRight(object? sender, EventArgs e)
        {
            if (_timer.IsEnabled)
                _model.ChangeDirection(LR.Right);
        }

        private void ViewModel_Pause(object? sender, EventArgs e)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }

        private void ViewModel_ExitGame(object? sender, EventArgs e)
        {
            _view.Close();
        }

        #endregion

        #region Model event handlers

        private void Model_GameStep(object? sender, SnakeEventArgs e)
        {
            _viewModel.ColorMap();
            
        }

        private void Model_GameOver(object? sender, SnakeEventArgs e)
        {
            _timer.Stop();
            MessageBox.Show("Vesztettél!" + Environment.NewLine + "Pontod: " + e.Score, "Game over", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
