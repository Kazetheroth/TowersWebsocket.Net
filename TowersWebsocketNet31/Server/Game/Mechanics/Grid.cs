using System.Collections.Generic;

namespace TowersWebsocketNet31.Server.Game.Mechanics
{
    public enum CellType
    {
        Group,
        Trap,
        Wall,
        Hole,
        Spawn,
        End,
        None
    }
    
    public class GridCellData
    {
        public CellType cellType;
        public int x;
        public int y;
    }
    
    public class Grid
    {
        public int index;
        public int size;
        public List<GridCellData> gridCellDatas;

        public Grid()
        {
            gridCellDatas = new List<GridCellData>();
            
            size = 20;
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    CellType cellType = CellType.None;

                    if (i == 1 && j == 1)
                    {
                        cellType = CellType.Spawn;
                    } else if (i == 19 && j == 19)
                    {
                        cellType = CellType.End;
                    }
                    
                    gridCellDatas.Add(new GridCellData
                    {
                        x = i,
                        y = j,
                        cellType = cellType
                    });
                }
            }
        }
    }
}