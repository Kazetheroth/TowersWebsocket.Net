using System;
using System.Collections.Generic;
using System.Numerics;

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
    public class GridCellDataList
    {
        public List<GridCellData> gridCellDatas { get; set; }
    }
    
    [Serializable]
    public class GridCellData
    {
        public CellType cellType { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public int idNeeded { get; set; }
        public Vector3 rotation { get; set; }

        public int idMeleeWeapon { get; set; }
        public int idDistanceWeapon { get; set; }
        public int idHelmetArmor { get; set; }
        public int idChestplateArmor { get; set; }
        public int idLeggingsArmor { get; set; }
    }
    
    [Serializable]
    public class GameGrid
    {
        public int index { get; set; }
        public int size { get; set; }
        public GridCellDataList gridCellDataList { get; set; }

        public GameGrid()
        {
            gridCellDataList = new GridCellDataList();
            gridCellDataList.gridCellDatas = new List<GridCellData>();
            
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
                    
                    gridCellDataList.gridCellDatas.Add(new GridCellData
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