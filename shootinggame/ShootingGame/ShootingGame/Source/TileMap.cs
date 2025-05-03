using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using static FlatPhysics.FlatWorld;

namespace ShootingGame
{
    // 타일은 업데이트는 필요없고 draw만 하면 될듯
    // 하지만 업데이트가 필요한 타일도 있다.
    /*
     <가로>

123
678
111213
53,54,55
85,86,87

<그냥 타일>
4,5,9,10,15,16,17 

<세로>
14,28,42
56,70,84

<얉은 위 가로>
95,96,97,123,124,125

<얉은 세로 왼/오>
99,101

     */
    public static class TileMap 
    {
        public static int Tile_Size = 48;
        public static Vector2 Tile_Dims = new Vector2(Tile_Size, Tile_Size);
        public static Vector2 Tile_Dims_X = new Vector2(Tile_Size, 0);
        public static Vector2 Tile_Dims_Y = new Vector2(0, Tile_Size);
        
     

        public static int StaticTile_Num = 125;
        public static int DynamicTile_Num = 0;
        public static string[] StaticTile_location;

        public static List<StaticTile> StaticTiles = new List<StaticTile>();
        public static List<DynamicTile> DynamicTiles = new List<DynamicTile>();

        // bridge,  
        public static List<FlatBody> SpecialTileFlatBody = new List<FlatBody>();

        public static void Update(Hero hero)
        {
            for (int i = 0; i < SpecialTileFlatBody.Count; i++)
            {
                FlatBody tmp = SpecialTileFlatBody[i];

                if (hero.FlatBody.LinearVelocity.Y > 0)
                {
                    tmp.active = false;
                    // 사실 heroactive 하면 좋을듯 아니면 특정 world type에만
                }

                else
                {
                    tmp.active = true;
                }
            
            }

            for (int i = 0; i < TileMap.DynamicTiles.Count; i++)
            {
                TileMap.DynamicTiles[i].Update();
            }
        }

        public static void Add_DynamicTile(DynamicTile dynamicTile)
        { 
            DynamicTiles.Add(dynamicTile);  

        }


        public static void Add_StaticTiles_Horizontal(Game1 game, Vector2 init_pos, int num,List<string> path)
        {

            Vector2 init_pos_center = Vector2.Add(init_pos, Tile_Dims / 2);
            Vector2 pos_helper = init_pos_center;

            bool bridge = StaticTile.isBridgeTile(path[0]);
            bool seq = false;
            int i = 0;
            FlatBody _;

            for (i = 0; i < num; i++)
            {
                bool tmp = StaticTile.isBridgeTile(path[i]);

                if (tmp != bridge)
                {
                    seq = true;
                    break;
                }

                StaticTiles.Add(new StaticTile(game, path[i], pos_helper,false,true));
                pos_helper = Vector2.Add(pos_helper, Tile_Dims_X);
             
            }

       
            InitFlatBody(game, Vector2.Add(init_pos_center , pos_helper - Tile_Dims_X) / 2, new Vector2(i, 1),out _,true,false);

            if (bridge)
            {
                SpecialTileFlatBody.Add(_);
            }


            if (seq)
            {
                Add_StaticTiles_Horizontal(game, pos_helper, num - i, path.GetRange(i, path.Count - i));
              
            }


        }


        public static void Add_StaticTiles_Vertical(Game1 game, Vector2 init_pos, int num, List<string> path)
        {
            Vector2 init_pos_center = Vector2.Add(init_pos, Tile_Dims / 2);
            Vector2 pos_helper = init_pos_center;

            for (int i = 0; i < num; i++)
            {
                StaticTiles.Add(new StaticTile(game, path[i], pos_helper,true,false));

               
                pos_helper = Vector2.Add(pos_helper, Tile_Dims_Y);
            }

            InitFlatBody(game, Vector2.Add(init_pos_center, pos_helper - Tile_Dims_Y) / 2, new Vector2(1, num), out FlatBody _,false,true);
        }

   
        public static bool FlatBodyIsBottom(FlatBody flatBody, SpriteEntity spriteEntity)
        {
    

            if ((flatBody.Position.Y + flatBody.height / 2 > 1f + spriteEntity.pos.Y - FlatAABB.HitBoxSize * spriteEntity.dims.Y / 2)) return false;

            if (flatBody.isHorizontalTile) return true;

            if (flatBody.isVerticalTile)
            {
                return true;
            
            }   

            return false;

        }

        public static bool FlatBodyIsSideWall(FlatBody flatBody,SpriteEntity spriteEntity)
        {
          


            if ((flatBody.Position.Y - flatBody.height / 2 > spriteEntity.pos.Y + Hero.WallJumpPortion * FlatAABB.HitBoxSize * spriteEntity.dims.Y / 2))
            {
                return false;
            }

            if (flatBody.isVerticalTile || flatBody.isHorizontalTile) return true;


            return false;



        }
        private static void InitFlatBody(Game1 game, Vector2 pos, Vector2 num, out FlatBody TILEBODY, bool horizontal, bool vertical)
        {
       

            if (!FlatBody.CreateBoxBody(TileMap.Tile_Dims.X*num.X , TileMap.Tile_Dims.Y*num.Y,
            1f, true, 0.5f, out FlatBody TileBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            TileBody.MoveTo(pos.X, pos.Y);
            TileBody.isTile = true;
            TileBody.isHorizontalTile = horizontal;
            TileBody.isVerticalTile = vertical;
          
            game.AddBody(TileBody, FlatWorld.Wolrd_layer.Static_allias);

            TILEBODY = TileBody;

        }

        public static void Draw(Sprites sprites, Vector2 o)
        {
            for (int i = 0; i < StaticTiles.Count; i++)
            {
                StaticTiles[i].Draw(sprites,o);
            }

            for (int i = 0; i < DynamicTiles.Count; i++)
            {
                TileMap.DynamicTiles[i].Draw(sprites,o,0f);

            }

        }

        public static void Update(Sprites sprites, Vector2 o)
        {
            //for (int i = 0; i < StaticTiles.Count; i++)
            //{
            //    StaticTiles[i].Draw(sprites, o);
            //}
        }

        //public static int


    }
}

