using Flat;
using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ShootingGame
{
    class EnvEntity :SpriteEntity
    {
        public EnvEntity(Game1 game, string path,Vector2 pos,Vector2 dims):base(game,path,RoundPos(dims,pos), RoundVec2(dims),Vector2.Zero,FlatWorld.Wolrd_layer.None_Interact)
        { 
            
        }

        //16픽셀당 한칸 (48픽셀로 간주한다)
        //위치변환필요
        public static Vector2 RoundVec2(Vector2 dims)
        {
            int pixelSize = 16;
            float x = (float)Math.Round(dims.X / pixelSize) * TileMap.Tile_Size;
            float y = (float)Math.Round(dims.Y / pixelSize) * TileMap.Tile_Size;

            if (x == 0) x = TileMap.Tile_Size;
            if (y == 0) y = TileMap.Tile_Size;

            return new Vector2(x, y);
        }

        public static Vector2 RoundPos(Vector2 dims, Vector2 pos)
        {
            Vector2 tmp = RoundVec2(dims);
            Vector2 offset = (tmp / TileMap.Tile_Size - Vector2.One) / 2 * TileMap.Tile_Size;
          
            return pos + offset;
        }

        // MapDeployment에서 배치를 할때 중앙처리를 하기 때문에 약간의 차이가 있다.
        // 클릭을할때는 왼쪽 좌표로, 배치는 중앙에 하므로 이것에도 차이가 있다.
        public static Vector2 RoundPosforEditor(Vector2 dims, Vector2 pos)
        {
            Vector2 tmp = RoundVec2(dims);
            Vector2 offset = (tmp / TileMap.Tile_Size - Vector2.One) / 2 * TileMap.Tile_Size;
            offset.X %= TileMap.Tile_Size;
            offset.Y = 0;

            return pos + offset;
        }


        public override void Draw(Sprites sprite, Vector2 o)
        {

            Game1.AntiAliasingShader(model, dims);
            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White,
               new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
        }

    }
}
