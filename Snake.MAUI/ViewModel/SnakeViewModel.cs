using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Model;
using Snake.Persistence;
using Microsoft.Maui.Graphics;
using System.Diagnostics;

namespace Snake.MAUI.ViewModel
{
    public class SnakeViewModel : ViewModelBase
    {
        private SnakeGameModel _model;
        private int _mapSize;

        public DelegateCommand SmallNewGameCommand { get; private set; }
        public DelegateCommand MediumNewGameCommand { get; private set; }
        public DelegateCommand LargeNewGameCommand { get; private set; }
        public DelegateCommand PauseGameCommand { get; private set; }
        public DelegateCommand TurnLeftCommand { get; private set; }
        public DelegateCommand TurnRightCommand { get; private set; }

        public ObservableCollection<MapTile> MapTiles { get; set; }

        public int Score { get { return _model.Score; } }

        public int MapSize
        {
            get => _mapSize;
            set
            {
                _mapSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MapRows));
                OnPropertyChanged(nameof(MapColumns));

            }
        }

        public RowDefinitionCollection MapRows
        {
            get
            {
                var ret = new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), MapSize).ToArray());
                //OnPropertyChanged(nameof(MapRows));
                return ret;
            }

        }

        public ColumnDefinitionCollection MapColumns
        {
            get
            {
                var ret = new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), MapSize).ToArray());
                //OnPropertyChanged(nameof(MapColumns));
                return ret;
            }
        }

        public event EventHandler<NewGameEventArgs>? LoadNewGame;
        public event EventHandler? TurnLeft;
        public event EventHandler? TurnRight;
        public event EventHandler? PauseGame;
        public event EventHandler? ExitGame;

        public SnakeViewModel(SnakeGameModel model)
        {
            _model = model;
            _model.GameStep += new EventHandler<SnakeEventArgs>(Model_GameStep);
            _model.GameOver += new EventHandler<SnakeEventArgs>(Model_GameOver);

            SmallNewGameCommand = new DelegateCommand(param => OnLoadNewGame(MapType.Small));
            MediumNewGameCommand = new DelegateCommand(param => OnLoadNewGame(MapType.Medium));
            LargeNewGameCommand = new DelegateCommand(param => OnLoadNewGame(MapType.Large));
            PauseGameCommand = new DelegateCommand(param => OnPauseGame());
            TurnLeftCommand = new DelegateCommand(param => OnTurnLeft());
            TurnRightCommand = new DelegateCommand(param => OnTurnRight());


        }

        public void GenerateMap()
        {
            MapSize = _model.Map.MapSize;
            MapTiles = new ObservableCollection<MapTile>();
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    MapTiles.Add(new MapTile
                    {
                        X = i, Y = j
                    });
                }
            }
            OnPropertyChanged(nameof(MapTiles));
            OnPropertyChanged(nameof(MapSize));
            ColorMap();
        }

        public void ColorMap()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    switch (_model.Map[i, j])
                    {
                        case CoordType.Empty:
                            MapTiles[i * MapSize + j].Color = Color.FromRgb(0, 0, 200);
                            break;
                        case CoordType.Wall:
                            MapTiles[i * MapSize + j].Color = Color.FromRgb(200, 100, 100);
                            break;
                    }
                }
            }

            List<(int, int)> SnakeCoords = _model.Map.SnakeCoords;
            for (int i = 0; i < SnakeCoords.Count; i++)
            {
                int green = (50 + (10 - i) * 15);
                switch (green)
                {
                    case < 0:
                        green = 0;
                        break;
                    case > 255:
                        green = 255;
                        break;
                }
                MapTiles[SnakeCoords[i].Item2 * MapSize + SnakeCoords[i].Item1].Color = Color.FromRgb(0, green, 0);
            }

            (int, int) EggCoords = _model.Map.EggCoords;
            MapTiles[EggCoords.Item2 * MapSize + EggCoords.Item1].Color = Color.FromRgb(255, 255, 255);
        }

        public void Model_GameOver(object? sender, SnakeEventArgs e)
        {

        }

        public void Model_GameStep(object? sender, SnakeEventArgs e)
        {
            ColorMap();
        }

        public void OnLoadNewGame(MapType type)
        {
            LoadNewGame?.Invoke(this, new NewGameEventArgs(type));
        }
        public void OnPauseGame()
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }
        public void OnTurnLeft()
        {
            TurnLeft?.Invoke(this, EventArgs.Empty);
        }
        public void OnTurnRight()
        {
            TurnRight?.Invoke(this, EventArgs.Empty);
        }
        public void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
