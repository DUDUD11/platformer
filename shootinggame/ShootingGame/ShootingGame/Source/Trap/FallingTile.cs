using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static FlatPhysics.FlatBody;

namespace ShootingGame
{

//pos 가시수정하는 위치로 수정해야하긴함
    public class FallingTile : Trap
    {
    

        public readonly static Vector2 FallingTile_Frames = new Vector2(1, 1);
        public readonly static string FallingTile_path = "Trap\\fallingTile";
        public readonly static Vector2 FallingTile_Dims = TileMap.Tile_Dims;

        public static int Addition_variable = 3; //x, y, bit
        public static float falling_Speed = 250f;
        public Vector2[] rect_pos;
        public List<Spine> SpineList;
        public bool active = false;
        public Vector2 Size;


        private int dir = 0;
        private Hero hero;
        private FlatVector falling_vec = new FlatVector(0f, -falling_Speed);



        // init_pos는 그대로 계산을 해서 건네주고 Diagnoal_pos 에는 크기 위치만을 기입하자
        public FallingTile(Game1 game, Vector2 init_pos, Vector2 Diagnoal_pos, int spine_dir) :
            base(game, FallingTile.FallingTile_path, init_pos, FallingTile_Dims * Diagnoal_pos, 0f, FlatBody.ShapeType.Box, FallingTile.FallingTile_Frames, 0, false, null)
        {
            this.rect_pos = new Vector2[4];
            this.Size = Diagnoal_pos;
        
 
            rect_pos[0] = init_pos;
            rect_pos[2] = init_pos + Diagnoal_pos * FallingTile_Dims;


            rect_pos[1] = new Vector2(rect_pos[2].X, rect_pos[0].Y);
            rect_pos[3] = new Vector2(rect_pos[0].X, rect_pos[2].Y);

            Custom_FlatBody(rect_pos[2] / 2, FallingTile_Dims * Diagnoal_pos);

            SpineList = new List<Spine>();



            if ((spine_dir & 1) == 1)
            {
                for (int i = (int)init_pos.X; i < (int)rect_pos[2].X; i += (int)Spine.spine_Dims.X)
                {

                    SpineList.Add(new Spine(game, new Vector2(i , (int)rect_pos[2].Y)));
                }
            }

            if ((spine_dir & 2) == 2)
            {
                for (int i = (int)init_pos.Y; i < (int)rect_pos[2].Y; i += (int)Spine.spine_Dims.Y)
                {
                    SpineList.Add(new Spine(game, new Vector2(init_pos.X - (int)Spine.spine_Dims.X , i), MathHelper.PiOver2));

                }
            }

            if ((spine_dir & 4) == 4)
            {
                for (int i = (int)init_pos.X; i < (int)rect_pos[2].X; i += (int)Spine.spine_Dims.X)
                {

                    SpineList.Add(new Spine(game, new Vector2(i, init_pos.Y - (int)Spine.spine_Dims.Y), MathHelper.PiOver2 * 2));

                }
            }

            if ((spine_dir & 8) == 8)
            {
                for (int i = (int)init_pos.Y; i < (int)rect_pos[2].Y; i += (int)Spine.spine_Dims.Y)
                {
                    SpineList.Add(new Spine(game, new Vector2(rect_pos[2].X  , i ), MathHelper.PiOver2 * 3));

                }
            }



        }


        private void Custom_FlatBody(Vector2 pos, Vector2 dims)
        {
            
                if (!FlatBody.CreateBoxBody(dims.X, dims.Y,
                1f, true, 0.5f, out FlatBody TrapBody, out string errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                flatBody = TrapBody;


            flatBody.MoveTo((rect_pos[0].X + rect_pos[2].X - TileMap.Tile_Size)/2, (rect_pos[2].Y + rect_pos[0].Y-TileMap.Tile_Size)/2);

        }
           


        //고쳐야될듯
        //흔들기도 추가합시다
        public override void Interact(SpriteEntity spriteEntity)
        {
            if (spriteEntity is not Hero hero) return;

            //if (spriteEntity is Hero hero && hero.pos.Y >= (this.pos.Y + dims.Y / 2))
            
                this.active = true;
                dir = (dir + 1) % 4;


                if (this.hero == null)
                {
                    this.hero = hero;
                }
                hero.status = Hero.Hero_Status.MovingPlatform;
                hero.FlatBody.LinearVelocity = falling_vec;

            game.Shake_Effect();

        }

    
        public override void Update()
        {
            base.Update();

            if (this.rect_pos[2].Y < 0f)
            {
                this.Destroy = true;

                for (int i = 0; i < SpineList.Count; i++)
                {
                    if (SpineList[i] != null) // 없어졌을수도있음
                    {
                        SpineList[i].Destroy = true;
                    }

                }

                return;
            }

            if (!this.active) return;

            float t = Flat.FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);

            if (this.hero != null)
            {

                //movingflatform 에위치하는지 확인하고 false해줘야함

                //     hero.FlatBody.LinearVelocity = new FlatVector(velocity.X, velocity.Y);
                // 물리에서 마찰력 각속도 free 해줘야할듯 
            }

            flatBody.Move(t * falling_vec);
            this.pos += FlatVector.ToVector2(t * falling_vec);

            for (int i = 0; i < SpineList.Count; i++)
            {
                if (SpineList[i] != null) // 없어졌을수도있음
                {
                    SpineList[i].Move(t * falling_vec);
                }

            }



        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    //dims 는 다르기때문에 현재껄 사용
                    Game1.AntiAliasingShader(model, FallingTile_Dims);
                    sprite.Draw(model, new Rectangle((int)(pos.X + o.X + FallingTile_Dims.X * i), (int)(pos.Y + o.Y + FallingTile_Dims.Y * j), (int)FallingTile_Dims.X, (int)FallingTile_Dims.Y), Color.White,
                     new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));


                }

            }





        }

    }
}