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
    public class SawBlade : Trap
    {
        public static int Addition_variable = 2;
        public readonly static Vector2 SawBlade_Frames = new Vector2(7, 1);
        public readonly static Vector2 SawBlade_Dims = TileMap.Tile_Dims * 2;
        public readonly static int SawBlade_millitimeFrame = 100;
        public readonly static string SawBlade_path = "Trap\\SawBlade";
        public readonly static float Reach_Time = (float)(SawBlade_Frames.X * SawBlade_millitimeFrame);
        public Vector2 End_pos;
        public Vector2 Start_pos;
        private bool dir = true;
     

        public SawBlade(Game1 game, Vector2 init_pos, Vector2 End_pos) : base(game, SawBlade_path, init_pos, SawBlade_Dims, Reach_Time, ShapeType.Circle, SawBlade_Frames, SawBlade_millitimeFrame,true,null)
        {
            this.End_pos = End_pos;
            this.Start_pos = init_pos;
         
        }


        public override void Interact(SpriteEntity spriteEntity)
        {

        
            if (spriteEntity is Hero hero)
            {
                hero.Get_Hit(-1);
            }

            else if (spriteEntity is Mob mob)
            {
                mob.Destroy = true;
            }
        }

        public override void Update()
        {
            base.Update();

            Vector2 velocity = (End_pos - Start_pos) / (Reach_Time/1000);
            float t = Flat.FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);


            if (dir)
            {
                if (FlatMath.NearlyEqual(FlatMath.Length(new FlatVector(End_pos.X, End_pos.Y) - flatBody.Position), 0f))
                {
                    dir = !dir;
                }

                else
                {
                    flatBody.Move(new FlatVector(t * velocity.X, t * velocity.Y));
                }
            }

            else
            {
                if (FlatMath.NearlyEqual(FlatMath.Length(new FlatVector(Start_pos.X, Start_pos.Y) - flatBody.Position), 0f))
                {
                    dir = !dir;
                }

                else
                {
                    flatBody.Move(new FlatVector(-t * velocity.X, -t * velocity.Y));
                }
            }

            this.pos = FlatVector.ToVector2(flatBody.Position);

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o, 0f);
        }


    }
}


