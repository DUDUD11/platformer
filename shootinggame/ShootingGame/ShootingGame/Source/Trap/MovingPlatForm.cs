
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShootingGame
{
    public class MovingPlatForm : Trap
    {
        //  3      2 
        //
        //
        //  0      1

        public readonly static Vector2 MovingPlatForm_Frames = new Vector2(1, 1);
        public readonly static Vector2 MovingPlatForm_Dims = TileMap.Tile_Dims * 3;
        public readonly static string MovingPlatForm_path = "Trap\\MovingPlatForm";
        public static int Addition_variable = 2;
        public Vector2[] rect_pos;
  
        
     
        public float Reach_time;
        public bool active = false;
        public Vector2 cur_velocity = Vector2.Zero;
        
        private int dir = 0;
        private Hero hero;
        



        public MovingPlatForm(Game1 game, Vector2 init_pos, Vector2 Diagnoal_pos,float reach_time) : base(game, MovingPlatForm_path, init_pos, MovingPlatForm_Dims, reach_time, FlatBody.ShapeType.Box, MovingPlatForm_Frames, 0,true,null)
        {
            this.rect_pos = new Vector2[4];

            rect_pos[0] = init_pos;
            rect_pos[2] = Diagnoal_pos; 

            this.Reach_time = reach_time;

            rect_pos[1] = new Vector2(rect_pos[2].X, rect_pos[0].Y);
            rect_pos[3] = new Vector2(rect_pos[0].X, rect_pos[2].Y);
          

        }

        public override void Interact(SpriteEntity spriteEntity)
        {

            if (spriteEntity is not Hero hero) return;


            if (active)
            {
                hero.FlatBody.LinearVelocity = new FlatVector(cur_velocity.X, cur_velocity.Y) + new FlatVector(hero.delta_Velocity.X,hero.delta_Velocity.Y);
                return;
            }

            if (hero.pos.Y >= (this.pos.Y+dims.Y/2))
            {
                this.active = true;
                dir = (dir + 1) % 4;

                cur_velocity = (rect_pos[dir] - rect_pos[h(dir - 1)]) / (Reach_time);

                if (this.hero == null)
                {
                    this.hero = hero;
                }

                hero.status = Hero.Hero_Status.MovingPlatform;
               
                hero.FlatBody.LinearVelocity = new FlatVector(cur_velocity.X, cur_velocity.Y);
               
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
                if (FlatMath.Length(hero.FlatBody.Position - flatBody.Position) > hero.dims.Length())
                {
                    hero.status = Hero.Hero_Status.aerial;
                }
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
                flatBody.Move(new FlatVector(t * velocity.X, t * velocity.Y));
            }

            this.pos = FlatVector.ToVector2(flatBody.Position);

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o, 0f);
        }

        


    }
}


