using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Persistence;

namespace Snake.MAUI.ViewModel
{
    public class NewGameEventArgs : EventArgs
    {
        private MapType _type;

        public MapType Type { get { return _type; } }

        public NewGameEventArgs(MapType type)
        {
            _type = type;
        }
    }
}
