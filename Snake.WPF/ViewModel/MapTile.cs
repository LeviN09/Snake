using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Snake.WPF.ViewModel
{
    public class MapTile : ViewModelBase
    {
        private SolidColorBrush? _color;
        public SolidColorBrush? Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
    }
}
