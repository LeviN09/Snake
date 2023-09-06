using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Persistence
{
    public interface ISnakeDataAccess
    {
        Task<SnakeMap> LoadAsync(MapType type);
    }
}
