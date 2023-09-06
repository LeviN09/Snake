using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Snake.Model;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;
using System.Windows.Input;

namespace Snake.WPF.ViewModel
{
    public class SnakeViewModel : ViewModelBase
    {
        #region Fields

        private SnakeGameModel _model;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand TurnLeftCommand { get; private set; }
        public DelegateCommand TurnRightCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<MapTile> MapTiles { get; private set; }

        #endregion

        #region Events

        public event EventHandler? NewGame;

        public event EventHandler? TurnLeft;

        public event EventHandler? TurnRight;

        public event EventHandler? Pause;

        public event EventHandler? ExitGame;

        #endregion

        #region Constructor

        public SnakeViewModel(SnakeGameModel model)
        {
            _model = model;

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            TurnLeftCommand = new DelegateCommand(param => OnTurnLeft());
            TurnRightCommand = new DelegateCommand(param => OnTurnRight());
            PauseCommand = new DelegateCommand(param => OnPause());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            MapTiles = new ObservableCollection<MapTile>();

        }

        #endregion

        #region View methods

        public void GenerateMap()
        {
            MapTiles.Clear();
            int mapSize = _model.Map.MapSize;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    MapTiles.Add(new MapTile());
                }
            }

            ColorMap();
        }

        public void ColorMap()
        {
            int mapSize = _model.Map.MapSize;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (_model.Map[i, j] == Persistence.CoordType.Empty)
                    {
                        MapTiles[i * mapSize + j].Color = new SolidColorBrush(ToMediaColor(DColor.Aqua));
                    }
                    else
                    {
                        MapTiles[i * mapSize + j].Color = new SolidColorBrush(ToMediaColor(DColor.Orange));
                    }
                }
            }

            for (int i = 0; i < _model.Map.SnakeCoords.Count; i++)
            {
                (int, int) coords = _model.Map.SnakeCoords[i];

                int greenI = (50 + (20 - i) * 150);
                byte green;
                switch (greenI)
                {
                    case < 0:
                        green = 0;
                        break;
                    case > 255:
                        green = 255;
                        break;
                    default:
                        green = (byte)greenI;
                        break;
                }

                MapTiles[coords.Item2 * mapSize + coords.Item1].Color = new SolidColorBrush(MColor.FromRgb(0, green, 0));
            }
            (int, int) eggCoords = _model.Map.EggCoords;
            MapTiles[eggCoords.Item2 * mapSize + eggCoords.Item1].Color = new SolidColorBrush(ToMediaColor(DColor.White));

        }
        public MColor ToMediaColor(DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion

        #region Event methods

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnTurnLeft()
        {
            TurnLeft?.Invoke(this, EventArgs.Empty);
        }

        private void OnTurnRight()
        {
            TurnRight?.Invoke(this, EventArgs.Empty);
        }

        private void OnPause()
        {
            Pause?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
