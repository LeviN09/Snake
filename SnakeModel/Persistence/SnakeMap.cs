using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Snake.Persistence
{
    public enum LR { Left, Right }

    public enum Direction { North, East, South, West }

    public enum CoordType { Empty, Wall }

    public enum MapType { Small, Medium, Large }

    class Snake
    {
        private List<(int, int)> _parts;
        private Direction _direction;

        public Direction Direction { get { return _direction; } }

        public List<(int, int)> Parts { get { return _parts; } }

        public Snake(List<(int, int)> parts, Direction direction)
        {
            _parts = parts;
            _direction = direction;
        }

        //North 0, East 1, South 2, West 3
        public void TurnLeft()
        {
            if (_direction is > Direction.North)
                _direction--;
            else
                _direction = Direction.West;
        }

        public void TurnRight()
        {
            if (_direction is < Direction.West)
                _direction++;
            else
                _direction = Direction.North;
        }

        public void Step()
        {
            Grow();

            _parts.RemoveAt(_parts.Count - 1);
        }

        public void Grow()
        {
            (int, int) newHead = GetNextCoord();

            _parts.Insert(0, newHead);
        }

        public (int, int) GetNextCoord()
        {
            (int, int) newHead = _parts[0];

            switch (_direction)
            {
                case Direction.North:
                    newHead.Item2--;
                    break;
                case Direction.South:
                    newHead.Item2++;
                    break;
                case Direction.West:
                    newHead.Item1--;
                    break;
                case Direction.East:
                    newHead.Item1++;
                    break;
            }

            return newHead;
        }
    }

    class Egg
    {
        private (int, int) _coords;

        public Egg() : this((0, 0)) { }

        public Egg((int, int) coords)
        {
            _coords = coords;
        }

        public (int, int) Coords
        {
            get { return _coords; }
            set { _coords = value; }
        }
    }

    public class SnakeMap
    {
        #region Fields

        private CoordType[,] _mapValues;
        private Snake _snake;
        private Egg _egg;

        #endregion

        #region Properties

        public int MapSize { get { return _mapValues.GetLength(0); } }

        public List<(int, int)> SnakeCoords { get { return _snake.Parts; } }

        public (int, int) SnakeNext { get { return _snake.GetNextCoord(); } }

        public Direction SnakeDir { get { return _snake.Direction; } }

        public (int, int) EggCoords { get { return _egg.Coords; } }

        public CoordType this[int x, int y] { get { return GetType(x, y); } }

        #endregion

        #region Constructor

        public SnakeMap(int mapSize)
        {
            if (mapSize < 0)
                throw new ArgumentOutOfRangeException(nameof(mapSize), "Map size is less than 0.");

            _mapValues = new CoordType[mapSize, mapSize];
            _egg = new Egg();
            InitSnake(new List<(int, int)>(), "South");
        }

        #endregion

        #region Public methods

        public CoordType GetType(int x, int y)
        {
            if (x < 0 || x >= _mapValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "X is out of range.");

            if (y < 0 || y >= _mapValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Y is out of range.");

            return _mapValues[x, y];
        }

        public void SetType(int x, int y, String type)
        {
            if (x < 0 || x >= _mapValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "X is out of range.");

            if (y < 0 || y >= _mapValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Y is out of range.");

            if (!Enum.TryParse(type, out _mapValues[x, y]))
                throw new ArgumentException("No such type found.");
        }

        public bool IsNextKilling()
        {
            (int, int) nextCoord = _snake.GetNextCoord();

            //Ha a térképen kívülre menne
            if (nextCoord.Item1 < 0 || nextCoord.Item2 < 0 || nextCoord.Item1 >= MapSize || nextCoord.Item2 >= MapSize)
                return true;

            //Ha falba ütközne
            if (_mapValues[nextCoord.Item2, nextCoord.Item1] == CoordType.Wall)
                return true;

            //Ha önmagába ütközne
            if (_snake.Parts.Contains(nextCoord))
                return true;

            return false;
        }

        public bool IsNextEgg()
        {
            return _snake.GetNextCoord() == _egg.Coords;
        }

        public void SetNewEggCoords()
        {
            Random rnd = new Random();
            (int, int) newCoord;
            do
            {
                newCoord = (rnd.Next(MapSize), rnd.Next(MapSize));
            }
            while (_mapValues[newCoord.Item2, newCoord.Item1] == CoordType.Wall || _snake.Parts.Contains(newCoord));

            _egg.Coords = newCoord;
        }

        public void InitSnake(List<(int, int)> parts, string direction)
        {
            Direction dir;
            if (!Enum.TryParse(direction, out dir))
                throw new ArgumentException("No such type found.");

            _snake = new Snake(parts, dir);
        }

        public void GrowSnake() { _snake.Grow(); }

        public void StepSnake() { _snake.Step(); }

        public void Turn(LR dir)
        {
            if (dir == LR.Left)
                _snake.TurnLeft();
            else if (dir == LR.Right)
                _snake.TurnRight();
        }

        #endregion
    }
}
