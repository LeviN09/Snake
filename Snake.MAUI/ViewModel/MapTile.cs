using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace Snake.MAUI.ViewModel
{
    public class MapTile : ViewModelBase
    {
        private Color _color;

        private int _x, _y;

        public Color Color {
            get {return _color;}
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public int X { get { return _x;} set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }
}
