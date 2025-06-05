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
    class SpineMovingPlatform : Trap
    {
        public static int Addition_variable = 3;
        public Vector2[] rect_pos;

        //  3      2 
        //
        //
        //  0      1

        //      1
        //
        //  2       8
        //
        //      4

        public float Reach_time;
        public List<Spine> SpineList;
        public bool active = false;
        private int dir = 0;
        private bool side = false;
        private bool left = false;
        private bool right = false;
        private bool detach = false;
        
        public Vector2 cur_velocity = Vector2.Zero;

        public static Vector2 SpineMovingPlatform_dims = MovingPlatForm.MovingPlatForm_Dims;



        private Hero hero;
        //     Trap spine1 = new Spine(game, init_pos,Spineangle * MathHelper.PiOver2);
        public SpineMovingPlatform(Game1 game, Vector2 init_pos, Vector2 Diagnoal_pos, float reach_time, int spine_dir) : 
            base(game, MovingPlatForm.MovingPlatForm_path, init_pos, SpineMovingPlatform_dims, reach_time, FlatBody.ShapeType.Box, MovingPlatForm.MovingPlatForm_Frames, 0,true,null)
        {
            this.rect_pos = new Vector2[4];

            if (!FlatUtil.IsNearlyEqual(init_pos.X - Diagnoal_pos.X, 0f)) left = true;
            if (!FlatUtil.IsNearlyEqual(init_pos.Y - Diagnoal_pos.Y, 0f)) right = true;

            rect_pos[0] = init_pos;
            rect_pos[2] = Diagnoal_pos;

            this.Reach_time = reach_time;

            rect_pos[1] = new Vector2(rect_pos[2].X, rect_pos[0].Y);
            rect_pos[3] = new Vector2(rect_pos[0].X, rect_pos[2].Y);

            SpineList = new List<Spine>();



            if ((spine_dir & 1) == 1)
            {
                int startX = (int)init_pos.X;
                int endX = startX + (int)SpineMovingPlatform_dims.X;
                int stepX = (int)Spine.spine_Dims.X;
                int posY = (int)(init_pos.Y - Spine.spine_Dims.Y + SpineMovingPlatform_dims.Y);
                side = true;
                for (int x = startX; x < endX; x += stepX)
                {
                    var position = new Vector2(x - stepX, posY);
                    SpineList.Add(new Spine(game, position));
                }
            }

            if ((spine_dir & 2) == 2)
            {
                for (int i = (int)init_pos.Y; i < (int)init_pos.Y + (int)SpineMovingPlatform_dims.Y; i += (int)Spine.spine_Dims.Y)
                {
                    SpineList.Add(new Spine(game, new Vector2(init_pos.X - (int)Spine.spine_Dims.X/2 -(int)SpineMovingPlatform_dims.X/2, i - (int)Spine.spine_Dims.Y), MathHelper.PiOver2));

                }
            }

            if ((spine_dir & 4) == 4)
            {
                int startX = (int)init_pos.X;
                int endX = startX + (int)SpineMovingPlatform_dims.X;
                int stepX = (int)Spine.spine_Dims.X;
                int posY = (int)(init_pos.Y -  Spine.spine_Dims.Y/2 - SpineMovingPlatform_dims.Y/2);


                for (int x = startX; x < endX; x += stepX)
                {
                    var position = new Vector2(x - stepX, posY);
                    SpineList.Add(new Spine(game, position, MathHelper.PiOver2 * 2));
                }
            }

            if ((spine_dir & 8) == 8)
            {
                for (int i = (int)init_pos.Y; i < (int)init_pos.Y + (int)SpineMovingPlatform_dims.Y; i += (int)Spine.spine_Dims.Y)
                {
                    SpineList.Add(new Spine(game, new Vector2(init_pos.X + (int)Spine.spine_Dims.X / 2 + (int)SpineMovingPlatform_dims.X / 2, i - (int)Spine.spine_Dims.Y ), MathHelper.PiOver2*3));

                }
            }



        }








        public override void Interact(SpriteEntity spriteEntity)
        {


            if (spriteEntity is not Hero hero) return;

            detach = false;


            if (active)
            {
               
                

                hero.FlatBody.LinearVelocity = new FlatVector(cur_velocity.X, cur_velocity.Y) + new FlatVector(hero.delta_Velocity.X, hero.delta_Velocity.Y);
                return;
            }

            if (hero.pos.Y >= (this.pos.Y + dims.Y / 2))
            {
                this.active = true;
                int prev_dir = dir;
                dir = direction(dir);

                cur_velocity = (rect_pos[dir] - rect_pos[prev_dir]) / (Reach_time);

                if (this.hero == null)
                {
                    this.hero = hero;
                }

                hero.status = Hero.Hero_Status.MovingPlatform;
            
                hero.FlatBody.LinearVelocity = new FlatVector(cur_velocity.X, cur_velocity.Y);

            }

            else if (side && hero.pos.Y >= (this.pos.Y - dims.Y / 2))
            {
                this.active = true;
                int prev_dir = dir;
                dir = direction(dir);

                cur_velocity = (rect_pos[dir] - rect_pos[prev_dir]) / (Reach_time);

                if (this.hero == null)
                {
                    this.hero = hero;
                }

                hero.status = Hero.Hero_Status.MovingPlatform;         

                hero.FlatBody.LinearVelocity = new FlatVector(cur_velocity.X, cur_velocity.Y );


            }


        }

        private int direction(int a)
        {
            if (left && right) a = (a + 1) % 4;
            if (left)
            {
                a = (a + 1) % 2;
            }

            if (right)
            {
                if (a == 0) a = 3;
                else a = 0;
            }    


            return a;
        }


        public override void Update()
        {
            base.Update();

            if (!this.active) return;

            int prev_dir = dir;

            float t = Flat.FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);


            if (this.hero != null && !detach)
            {

                if (!Collisions.ContactAABBs(hero.FlatBody.GetAABB(), this.flatBody.GetAABB()))
                {
                    hero.status = Hero.Hero_Status.aerial;
                    hero.Jumped = false;
                    detach = true;
                }

                else
                { 
                
                }
            }

            if (FlatPhysics.FlatMath.NearlyEqual(FlatPhysics.FlatMath.Length(new FlatVector(rect_pos[dir].X, rect_pos[dir].Y) - flatBody.Position), 0f))
            {
                active = false;
               

            }

            else
            {
                FlatVector delta = new FlatVector(t * cur_velocity.X, t * cur_velocity.Y);

                flatBody.Move(delta);
                this.pos = FlatVector.ToVector2(flatBody.Position);

                for (int i = 0; i < SpineList.Count; i++)
                {
                    if (SpineList[i] != null) // 없어졌을수도있음
                    {
               
                        SpineList[i].Move(delta);
                   

                    }
                
                }


            }

         

           

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o, 0f);

        }




    }
}


