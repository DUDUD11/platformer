using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatBody;

namespace ShootingGame
{
    public class Door : Trap
    {
        // 가시 삭제, 한번 active = false 면 끝
        // 일단 고정크기로 구성

        public static int Addition_variable = 3;
        public Vector2 End_Pos;

        //  3      2 
        //
        //
        //  0      1

        //      1
        //
        //  2       8
        //
        //      4

        public static float Speed = 100f;
        public bool active = false;
        public int Blue_Points = 0;
        public Vector2 Moving_Dir;

        public static Vector2 Door_Frames = new Vector2(7, 1);

        public static string Door_path = "Trap\\Door";
        public static Vector2 Door_dims = MovingPlatForm.MovingPlatForm_Dims;

        private Hero hero;
        private bool detach=true;
        //     Trap spine1 = new Spine(game, init_pos,Spineangle * MathHelper.PiOver2);

        private bool Reach_End()
        {
            if (!FlatUtil.IsNearlyEqual(this.pos.X - End_Pos.X, 0f)) return false;
            if (!FlatUtil.IsNearlyEqual(this.pos.Y - End_Pos.Y, 0f)) return false;

            return true;

        }

        public Door(Game1 game, Vector2 init_pos, Vector2 Diagnoal_pos, int BluePoint) :
            base(game, Door_path, init_pos, new Vector2(0.6f,0.8f)*Door_dims, 0f, FlatBody.ShapeType.Box, Door_Frames, 100, true, null)
        {
            this.End_Pos = new Vector2(init_pos.X + Diagnoal_pos.X * TileMap.Tile_Dims.X, init_pos.Y + Diagnoal_pos.Y * TileMap.Tile_Dims.Y);
            this.Blue_Points = BluePoint;

            if (FlatUtil.IsNearlyEqual(init_pos.X, End_Pos.X))
            {
                if (init_pos.Y > End_Pos.Y) Moving_Dir = new Vector2(0, -Speed);
                else Moving_Dir = new Vector2(0, Speed);
            }

            else
            {
                if (init_pos.X > End_Pos.X) Moving_Dir = new Vector2(-Speed, 0);
                else Moving_Dir = new Vector2(Speed, 0);
            }
        }


        public void Earn_BluePoint()
        {
            this.Blue_Points--;
        }


        public override void Interact(SpriteEntity spriteEntity)
        {
            detach = false;

            if (spriteEntity is not Hero hero ) return;

            if (this.hero == null)
            {
                this.hero = hero;
            }

            if (hero.pos.Y >= (this.pos.Y +  dims.Y / 2))
            {
                hero.bottomReach();
            }

            else if (hero.pos.Y >= (this.pos.Y - dims.Y / 2))
            {
                hero.WallReach();
            }


            if (active || Blue_Points != 0) return;


            active = true;

          

         

        }


        public override void Update()
        {
            base.Update();

            if (!this.active) return;

            

            float t = Flat.FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);


            if (this.hero != null && !detach)
            {

                if (!Collisions.ContactAABBs(hero.FlatBody.GetAABB(), this.flatBody.GetAABB()))
                {
                    hero.status = Hero.Hero_Status.aerial;
                    detach = true;
                }
            }

            if (Reach_End())
            {
                active = false;
            }

            else
            {
                FlatVector delta = new FlatVector(Moving_Dir.X,Moving_Dir.Y) * t;

                flatBody.Move(delta);
                this.pos = FlatVector.ToVector2(flatBody.Position);

            }

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o, 0f);

        }


    }
}


