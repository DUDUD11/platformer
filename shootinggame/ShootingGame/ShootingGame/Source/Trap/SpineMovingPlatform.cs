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

        public static Vector2 SpineMovingPlatform_dims = MovingPlatForm.MovingPlatForm_Dims;



        private Hero hero;
        //     Trap spine1 = new Spine(game, init_pos,Spineangle * MathHelper.PiOver2);
        public SpineMovingPlatform(Game1 game, Vector2 init_pos, Vector2 Diagnoal_pos, float reach_time, int spine_dir) : 
            base(game, MovingPlatForm.MovingPlatForm_path, init_pos, SpineMovingPlatform_dims, reach_time, FlatBody.ShapeType.Box, MovingPlatForm.MovingPlatForm_Frames, 0,true,null)
        {
            this.rect_pos = new Vector2[4];

            rect_pos[0] = init_pos;
            rect_pos[2] = Diagnoal_pos;

            this.Reach_time = reach_time;

            rect_pos[1] = new Vector2(rect_pos[2].X, rect_pos[0].Y);
            rect_pos[3] = new Vector2(rect_pos[0].X, rect_pos[2].Y);

            SpineList = new List<Spine>();



            if ((spine_dir & 1) == 1)
            {
                for (int i = (int)init_pos.X; i < (int)init_pos.X+ (int)SpineMovingPlatform_dims.X; i += (int)Spine.spine_Dims.X)
                {
      
                    SpineList.Add(new Spine(game, new Vector2(i - (int)Spine.spine_Dims.X / 2, Diagnoal_pos.Y - (int)Spine.spine_Dims.Y/2 + (int)SpineMovingPlatform_dims.Y/2)));

                }
            }

            if ((spine_dir & 2) == 2)
            {
                for (int i = (int)init_pos.Y; i < (int)init_pos.Y + (int)SpineMovingPlatform_dims.Y; i += (int)Spine.spine_Dims.Y)
                {
                    SpineList.Add(new Spine(game, new Vector2(init_pos.X - (int)Spine.spine_Dims.X/2 -(int)SpineMovingPlatform_dims.X/2, i - (int)Spine.spine_Dims.Y/2), MathHelper.PiOver2));

                }
            }

            if ((spine_dir & 4) == 4)
            {
                for (int i = (int)init_pos.X; i < (int)init_pos.X + (int)SpineMovingPlatform_dims.X; i += (int)Spine.spine_Dims.X)
                {

                    SpineList.Add(new Spine(game, new Vector2(i - (int)Spine.spine_Dims.X / 2, init_pos.Y - (int)Spine.spine_Dims.Y / 2 - (int)SpineMovingPlatform_dims.Y/2 ), MathHelper.PiOver2*2));

                }
            }

            if ((spine_dir & 8) == 8)
            {
                for (int i = (int)init_pos.Y; i < (int)init_pos.Y + (int)SpineMovingPlatform_dims.Y; i += (int)Spine.spine_Dims.Y)
                {
                    SpineList.Add(new Spine(game, new Vector2(Diagnoal_pos.X - (int)Spine.spine_Dims.X / 2 + (int)SpineMovingPlatform_dims.X / 2, i - (int)Spine.spine_Dims.Y / 2), MathHelper.PiOver2*3));

                }
            }



        }


        public override void Interact(SpriteEntity spriteEntity)
        {
            if (active) return;

            //if (spriteEntity is Hero hero && hero.pos.Y >= (this.pos.Y + dims.Y / 2))
            if (spriteEntity is Hero hero)
            {
                this.active = true;
                dir = (dir + 1) % 4;

                Vector2 velocity = (rect_pos[dir] - rect_pos[h(dir - 1)]) / (Reach_time);

                if (this.hero == null)
                {
                    this.hero = hero;
                }
                hero.status = Hero.Hero_Status.MovingPlatform;
                hero.FlatBody.LinearVelocity = new FlatVector(velocity.X, velocity.Y);

            }


        }

        private int h(int a)
        {
            return (a + 4) % 4;
        }


        public override void Update()
        {
            base.Update();

            if (!this.active) return;

            Vector2 velocity = (rect_pos[dir] - rect_pos[h(dir - 1)]) / (Reach_time);
            float t = Flat.FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);

            if (this.hero != null)
            {

                //movingflatform 에위치하는지 확인하고 false해줘야함

                //     hero.FlatBody.LinearVelocity = new FlatVector(velocity.X, velocity.Y);
                // 물리에서 마찰력 각속도 free 해줘야할듯 
            }


            if (FlatMath.NearlyEqual(FlatMath.Length(new FlatVector(rect_pos[dir].X, rect_pos[dir].Y) - flatBody.Position), 0f))
            {
                active = false;
            }

            else
            {
                FlatVector delta = new FlatVector(t * velocity.X, t * velocity.Y);

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


