using System;
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
    
    [Serializable]
    public class GridCellData
    {
        public CellType cellType { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
    
    [Serializable]
    public class Grid
    {
        public int index { get; set; }
        public int size { get; set; }
        public List<GridCellData> gridCellDatas { get; set; }

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