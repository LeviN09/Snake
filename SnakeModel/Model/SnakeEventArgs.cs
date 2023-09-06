using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Model
{
    public class SnakeEventArgs : EventArgs
    {
        private bool _isGameOver;
        private int _score;

        public bool IsOver { get { return _isGameOver; } }

        public int Score { get { return _score; } }
    
        public SnakeEventArgs(int score, bool isGameOver)
        {
            _score = score;
            _isGameOver = isGameOver;
        }
    }
}
