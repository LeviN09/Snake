using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Snake.Model;
using Snake.Persistence;

namespace Snake
{
    public partial class GameForm : Form
    {
        #region Fields

        private ISnakeDataAccess _dataAccess = null!;
        private SnakeGameModel _gameModel = null!;
        private Button[,] _buttons = null!;
        private System.Windows.Forms.Timer _timer;

        private bool _keyHeld = false;

        #endregion

        public GameForm()
        {
            InitializeComponent();

            _dataAccess = new SnakeFileDataAccess();

            _gameModel = new SnakeGameModel(_dataAccess);
            _gameModel.GameStep += new EventHandler<SnakeEventArgs>(Game_GameStep);
            _gameModel.GameOver += new EventHandler<SnakeEventArgs>(Game_GameOver);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 500;
            _timer.Tick += new EventHandler(Timer_Tick);
        }

        private void GenerateMap()
        {
            int mapSize = _gameModel.Map.MapSize;
            _buttons = new Button[mapSize, mapSize];
            double ratio = Size.Height / (mapSize * 1.2);
            int btnSize = (int)Math.Floor(ratio);
            for (Int32 i = 0; i < mapSize; i++)
            {
                for (Int32 j = 0; j < mapSize; j++)
                {
                    _buttons[i, j] = new Button();
                    _buttons[i, j].Location = new Point(5 + btnSize * j, 35 + btnSize * i); // elhelyezkedés
                    _buttons[i, j].Size = new Size(btnSize, btnSize); // méret
                    _buttons[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttons[i, j].Enabled = false; // kikapcsolt állapot
                    _buttons[i, j].TabIndex = 100 + i * mapSize + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttons[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    //_buttons[i, j].MouseClick += new MouseEventHandler(Buttons_MouseClick);
                    // közös eseménykezelõ hozzárendelése minden gombhoz

                    Controls.Add(_buttons[i, j]);
                    // felvesszük az ablakra a gombot
                }
            }
            ColorMap();
        }

        private void DeleteMap()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is Button)
                {
                    Controls.RemoveAt(i);
                    i--;
                }
            }
        }

        private void ColorMap()
        {
            for (Int32 i = 0; i < _gameModel.Map.MapSize; i++)
            {
                for (Int32 j = 0; j < _gameModel.Map.MapSize; j++)
                {
                    switch (_gameModel.Map[i, j])
                    {
                        case CoordType.Empty:
                            _buttons[i, j].BackColor = Color.Aqua;
                            break;
                        case CoordType.Wall:
                            _buttons[i, j].BackColor = Color.Coral;
                            break;
                    }
                }
            }

            List<(int, int)> snakeCoords = _gameModel.Map.SnakeCoords;
            int num = snakeCoords.Count;
            for (int i = 0; i < num; i++)
            {
                int green = (50 + (20 - i) * 10);
                if (green < 0)
                    green = 0;
                else if (green > 255)
                    green = 255;
                _buttons[snakeCoords[i].Item2, snakeCoords[i].Item1].BackColor = Color.FromArgb(255, 0, green, 0);
            }
            (int, int) eggCoords = _gameModel.Map.EggCoords;
            _buttons[eggCoords.Item2, eggCoords.Item1].BackColor = Color.White;
        }

        private void Game_GameStep(Object? sender, SnakeEventArgs e)
        {
            ColorMap();
            //Debug.WriteLine("Advanced");
        }

        private void Game_GameOver(Object? sender, SnakeEventArgs e)
        {
            _timer?.Stop();
            MessageBox.Show("A játéknak vége!" + Environment.NewLine + "Pontszámod: " + e.Score, "Game over", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Timer_Tick(Object? sender, EventArgs e)
        {
            /*Debug.WriteLine("K " + _gameModel.Map.IsNextKilling());
            Debug.WriteLine("E " + _gameModel.Map.IsNextEgg());
            Debug.WriteLine("N " + _gameModel.Map.SnakeNext);
            Debug.WriteLine("D " + _gameModel.Map.SnakeDir);
            Debug.WriteLine("H " + _gameModel.Map.SnakeCoords[0].ToString());*/
            _gameModel.StepGame();
        }

        private void MenuFileExit_Click(Object sender, EventArgs e)
        {
            bool restartTimer = _timer.Enabled;
            _timer.Stop();

            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Snake", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
            else
            {
                if (restartTimer)
                    _timer.Start();
            }
        }

        private async void NewGameMenu_ClickAsync(object sender, EventArgs e)
        {
            bool restartTimer = _timer.Enabled;
            _timer.Stop();

            OpenFileDialog _openFileDialog = new OpenFileDialog();
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _gameModel.LoadNewGameAsync(_openFileDialog.FileName);
                    if (_buttons != null)
                        DeleteMap();
                    GenerateMap();
                    restartTimer = true;
                }
                catch (SnakeDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + 
                        "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            if (restartTimer)
                _timer.Start();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_keyHeld)
            {
                if (e.KeyCode == Keys.Tab && !_gameModel.IsGameOver)
                {
                    if (_timer.Enabled)
                        _timer.Stop();
                    else
                        _timer.Start();
                    _keyHeld = true;
                }
                if (_timer.Enabled)
                {
                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                    {
                        _gameModel.ChangeDirection(LR.Left);
                    }
                    else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                    {
                        _gameModel.ChangeDirection(LR.Right);
                    }
                    _keyHeld = true;
                }
            }

         }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            _keyHeld = false;
        }
    }
}