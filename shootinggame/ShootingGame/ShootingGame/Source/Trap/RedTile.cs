using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static FlatPhysics.FlatBody;

namespace ShootingGame
{
    public class RedTile : Trap
    {

        public readonly static Vector2 Redile_Frames = new Vector2(1, 1);
        public readonly static string RedTile_path = "Trap\\redtile";
        public readonly static Vector2 RedTile_Dims = TileMap.Tile_Dims;

        public static int Addition_variable = 2; //x, y

        public Vector2[] rect_pos;


        public Vector2 Size;
        private Hero hero;
     
        // init_pos는 그대로 계산을 해서 건네주고 Diagnoal_pos 에는 크기 위치만을 기입하자
        public RedTile(Game1 game, Vector2 init_pos, Vector2 Diagnoal_pos) :
            base(game,RedTile.RedTile_path, init_pos, RedTile_Dims * Diagnoal_pos, 0f, FlatBody.ShapeType.Box, RedTile.Redile_Frames, 0, false, null)
        {
            this.rect_pos = new Vector2[4];
            this.Size = Diagnoal_pos;


            rect_pos[0] = init_pos;
            rect_pos[2] = init_pos + Diagnoal_pos * RedTile_Dims;


            rect_pos[1] = new Vector2(rect_pos[2].X, rect_pos[0].Y);
            rect_pos[3] = new Vector2(rect_pos[0].X, rect_pos[2].Y);

            Custom_FlatBody(rect_pos[2] / 2, RedTile_Dims * Diagnoal_pos);

        }


        private void Custom_FlatBody(Vector2 pos, Vector2 dims)
        {

            if (!FlatBody.CreateBoxBody(dims.X, dims.Y,
            1f, true, 0.5f, out FlatBody TrapBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }
            flatBody = TrapBody;


            flatBody.MoveTo((rect_pos[0].X + rect_pos[2].X - TileMap.Tile_Size) / 2, (rect_pos[2].Y + rect_pos[0].Y - TileMap.Tile_Size) / 2);

        }



        //고쳐야될듯
        //흔들기도 추가합시다
        public override void Interact(SpriteEntity spriteEntity)
        {
            if (spriteEntity is not Hero hero) return;

            //if (spriteEntity is Hero hero && hero.pos.Y >= (this.pos.Y + dims.Y / 2))


            if (this.hero == null)
            {
                this.hero = hero;
            }

            hero.Get_Hit(-1);

        }

        public override void Update()
        {
            base.Update();

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    //dims 는 다르기때문에 현재껄 사용

                    Game1.AntiAliasingShader(model, RedTile_Dims);
                    sprite.Draw(model, new Rectangle((int)(pos.X + o.X + RedTile_Dims.X * i), (int)(pos.Y + o.Y + RedTile_Dims.Y * j), (int)RedTile_Dims.X, (int)RedTile_Dims.Y), Color.White,
                     new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
                }

            }
        }

    }
}