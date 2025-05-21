using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShootingGame
{
    public enum D1
    { 
        Tile = 1,
        Trap = 2,
        Mobs = 3,
        Hero = 4,
        Env =5,
    }

    public enum D2_Tile
    {

        l1 = 1,
        m1 = 2,
        r1 = 3,
        l2 = 6,
        m2 = 7,
        r2 = 8,
        l3 = 11,
        m3 = 12,
        r3 = 13,
        l4 = 53,
        m4 = 54,
        r4 = 55,
        l5 = 85,
        m5 = 86,
        r5 = 87,

        n1 = 4,
        n2 = 5,
        n3 = 9,
        n4 = 10,
        n5 = 15,
        n6 = 16,
        n7 = 17,

        wt1 = 14,
        wm1 = 28,
        wd1 = 42,

        wt2 = 56,
        wm2 = 70,
        wd2 = 84,

        ls1 = 95,
        ms1 = 96,
        rs1 = 97,
        ls2 = 123,
        ms2 = 124,
        rs2 = 125,

        wts1 = 99,
        wtd1 = 101,

    }

    public enum D2_Trap
    {

        electric = 0,
        JumpTrap = 1,
        MovingPlatform = 2,
        SawBlade = 3,
        static_spine = 4,
        distile = 5,
        SpineMovingPlatform = 6,
        fallingTile = 7,


    }

    public enum D2_Env
    { 
        star =1,
        diamond=2,
    }

    public class Map
    {
        public static int width_pixel = 1920 / TileMap.Tile_Size;
        public static int height_pixel = (1080 / TileMap.Tile_Size) + 1;

        public int Map_width = 1920 / TileMap.Tile_Size;
        public int Map_height = (1080 / TileMap.Tile_Size)+1;
        public Tuple<int, int> ResetPoint = Tuple.Create(0, 0);
        public Tuple<int, int> Offset = Tuple.Create(0, 0);
        public List<List<Tuple<int, int>>> Deploy;
        public List<List<Object>> DeployInfo;

        public string LeftMap;
        public string RightMap;
        public string DownMap;
        public string UpMap;


        public void Set_MapSize(int width , int height)
        {
            this.Map_width = width;
            this.Map_height = height;

        }

        public void Change_MapSize(int width, int height)
        {
            Set_MapSize(width, height);

            Deploy = new List<List<Tuple<int, int>>>();
            DeployInfo = new List<List<Object>>();

            // Map_width와 Map_height에 맞게 (0, 0)으로 초기화
            for (int i = 0; i < Map_height; i++)  // 높이만큼 반복
            {
                var row = new List<Tuple<int, int>>();
                var row2 = new List<Object>();

                for (int j = 0; j < Map_width; j++)  // 너비만큼 반복
                {
                    row.Add(Tuple.Create(0, 0));  // 각 (x, y)를 (0, 0)으로 설정
                    row2.Add(null);
                }
                Deploy.Add(row);  // 한 줄을 Deploy에 추가
                DeployInfo.Add(row2);
            }
        }


        public string NextMap(int a)
        {
            if (a == 0) return LeftMap;
            if (a == 1) return RightMap;
            if (a == 2) return UpMap;
            if (a == 3) return DownMap;

            return null;

        }

        private void Init(Tuple<int, int> resetPoint, Tuple<int,int > Offset)
        {
            this.ResetPoint = new Tuple<int, int>(resetPoint.Item1 * (int)TileMap.Tile_Dims.X, resetPoint.Item2 * (int)TileMap.Tile_Dims.Y);
            this.Offset = new Tuple<int, int>(Offset.Item1 * (int)TileMap.Tile_Dims.X, Offset.Item2 * (int)TileMap.Tile_Dims.Y);
            Deploy = new List<List<Tuple<int, int>>>();
            DeployInfo = new List<List<Object>>();

            // Map_width와 Map_height에 맞게 (0, 0)으로 초기화
            for (int i = 0; i < Map_height; i++)  // 높이만큼 반복
            {
                var row = new List<Tuple<int, int>>();
                var row2 = new List<Object>();

                for (int j = 0; j < Map_width; j++)  // 너비만큼 반복
                {
                    row.Add(Tuple.Create(0, 0));  // 각 (x, y)를 (0, 0)으로 설정
                    row2.Add(null);
                }
                Deploy.Add(row);  // 한 줄을 Deploy에 추가
                DeployInfo.Add(row2);
            }

        }

        public Vector2 MapSize()
        {
            return new Vector2(Map_width, Map_height) * TileMap.Tile_Size;
        }



        public Map(Tuple<int, int> resetPoint, Tuple<int, int> Offset)
        {

            Init(resetPoint, Offset);

        }

        [JsonConstructor]
        public Map(Tuple<int, int> resetPoint, Tuple<int, int> Offset, int map_width, int map_height)
        {
            Set_MapSize(map_width, map_height);
            Init(resetPoint, Offset);
        }

        


        public void Map_Update(int x, int y,Tuple<int,int> val)
        {
            if (y >= Map_height || y < 0 || x < 0 || x >= Map_width) return;

            Deploy[y][x] = val;
        }

        public static string GetTexture(Tuple<int,int> pos)
        { 

            switch (pos.Item1)
            {
                case 0:
                    throw new Exception("ERROR");

                case 1:

                    return TileMap.StaticTile_location[pos.Item2];
                    
                case 2:
                    
                        D2_Trap trap = (D2_Trap)pos.Item2;
                        if (trap == D2_Trap.SpineMovingPlatform) trap = D2_Trap.MovingPlatform;

                        string trapName = Enum.GetName(typeof(D2_Trap), trap);
                        return "Trap\\" + trapName;
                case 3:
                    break;

                case 5:
                    switch (pos.Item2)
                    {
                        case 1:
                            return Star.star_path;
                        case 2:
                            return Diamond.Diamond_path;
                    }

                    break;

            }

            

            return "";
        }
    


        

    }
}
