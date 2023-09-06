using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Persistence;

namespace Snake.MAUI.Persistence
{
    public class SnakeMauiDataAccess : ISnakeDataAccess
    {
        public async Task<SnakeMap> LoadAsync(MapType type)
        {
            string path = String.Empty;
            switch (type)
            {
                case MapType.Small:
                    path = "map1.sm";
                    break;
                case MapType.Medium:
                    path = "map2.sm";
                    break;
                case MapType.Large:
                    path = "map3.sm";
                    break;
            }

            try
            {
                using (var stream = await FileSystem.OpenAppPackageFileAsync(path))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        String line = await reader.ReadLineAsync() ?? String.Empty;

                        int size = int.Parse(line);
                        String[] coords;
                        SnakeMap map = new SnakeMap(size);

                        //Térkép beolvasása
                        for (int i = 0; i < size; i++)
                        {
                            line = await reader.ReadLineAsync() ?? String.Empty;
                            coords = line.Split(' ');

                            for (int j = 0; j < coords.Length; j++)
                            {
                                map.SetType(i, j, coords[j]);
                            }
                        }

                        //Kígyó beolvasása
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        String[] partsStr = line.Split(' ');
                        List<(int, int)> parts = new List<(int, int)>();
                        for (int i = 0; i < partsStr.Length; i++)
                        {
                            string[] partStr = partsStr[i].Split(',');
                            parts.Add((int.Parse(partStr[0]), int.Parse(partStr[1])));
                        }
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        map.InitSnake(parts, line);

                        //Tojás elhelyezése
                        map.SetNewEggCoords();

                        return map;
                    }
                }
            }
            catch
            {
                throw new SnakeDataException();
            }
        }
    }
}
